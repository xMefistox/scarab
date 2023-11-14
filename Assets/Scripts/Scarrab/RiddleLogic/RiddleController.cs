using Scarrab.Zenject;
using Scarrab.RiddleVisual;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Common;

namespace Scarrab.RiddleLogic
{
    public class RiddleController : MonoBehaviour
    {
        [SerializeField]
        private ScarrabController[] ScarrabControllers;
        [SerializeField]
        private GameObject ResetButton;
        [SerializeField]
        private ParticleSystem winParticles;
        [SerializeField]
        private ConnectionVisual connectionPrefab;
        [SerializeField]
        private Transform connectionsParent;

        [Inject]
        private SignalBus signalBus;
        [Inject]
        private VisualFactory visualFactory;

        private ScarrabController lastSelectedScarrab;
        private Stack<ScarrabController> activatedScarrabs = new Stack<ScarrabController>();
        private List<ConnectionVisual> connections = new();
        private bool riddleSolved;

        public void OnEnable()
        {
            signalBus.Subscribe<ScarrabClicked>(OnScarrabClicked);
            signalBus.Subscribe<ResetButtonClicked>(OnResetButtonClicked);
            ResetButton.SetActive(false);
            winParticles.gameObject.SetActive(false);
        }

        public void OnDisable()
        {
            signalBus.Unsubscribe<ScarrabClicked>(OnScarrabClicked);
            signalBus.Unsubscribe<ResetButtonClicked>(OnResetButtonClicked);
        }

        /*        private void Update() // CHEAT FOR DEBUG PURPOSES
                {
                    if(UnityEngine.Input.GetKeyDown(KeyCode.Z) && gameObject.name=="RiddleWall")
                    {
                        foreach(ScarrabController ScarrabController in ScarrabControllers)
                        {
                            foreach(ScarrabController neighbour in ScarrabController.neighboursList)
                            {
                                TryActivateConnectionBetweenScarrabs(ScarrabController, neighbour);
                            }
                        }
                        ActivateScarrab(lastSelectedScarrab);
                    }
                }*/

        private void OnResetButtonClicked()
        {
            ResetButton.SetActive(false);
            winParticles.gameObject.SetActive(false);
            foreach (ScarrabController ScarrabController in ScarrabControllers)
            {
                ScarrabController.Reset();
            }
            activatedScarrabs.Clear();
            lastSelectedScarrab = null;
            riddleSolved = false;
        }

        private void OnScarrabClicked(ScarrabClicked context)
        {
            if (riddleSolved)
            {
                return;
            }
            ScarrabController newSelectedScarrab = Array.Find(ScarrabControllers, ScarrabController => ScarrabController.VisualController.gameObject == context.ScarrabVisual);
            if (newSelectedScarrab != null)
            {
                switch (newSelectedScarrab.State)
                {
                    case ScarrabState.Inactive:

                        if (lastSelectedScarrab == null)
                        {
                            SelectScarrab(newSelectedScarrab);
                        }
                        else
                        {
                            TryActivateConnectionBetweenScarrabs(lastSelectedScarrab, newSelectedScarrab);
                        }
                        break;
                    case ScarrabState.Selected:
                        if (lastSelectedScarrab == newSelectedScarrab)
                        {
                            RevertLastSelectionAndConnection(lastSelectedScarrab);
                        }
                        break;
                    case ScarrabState.Active:
                        TryActivateConnectionBetweenScarrabs(lastSelectedScarrab, newSelectedScarrab);
                        break;
                }
            }

            ResetButton.gameObject.SetActive(lastSelectedScarrab != null);
        }

        private void CheckSolveConditions()
        {
            if (ScarrabControllers.Any(Scarrab => Scarrab.AtLeastOneNeighbourIsNotConnected()))
            {
                return;
            }
            ActivateScarrab(lastSelectedScarrab);
            winParticles.gameObject.SetActive(true);
            winParticles.Play();
            signalBus.Fire(new RiddleSolved());
            ResetButton.SetActive(true);
            riddleSolved = true;
        }

        private void SelectScarrab(ScarrabController ScarrabController)
        {
            ScarrabController.SetScarrabSelection(true);
            lastSelectedScarrab = ScarrabController;
        }

        private void ActivateScarrab(ScarrabController ScarrabController)
        {
            lastSelectedScarrab = ScarrabController;
            if (activatedScarrabs.Count == 0 || activatedScarrabs.Last() != ScarrabController)
            {
                activatedScarrabs.Push(lastSelectedScarrab);
            }
            ScarrabController.SetScarrabActivation(true);
            ScarrabController.SetScarrabSelection(false);
        }

        private void TryActivateConnectionBetweenScarrabs(ScarrabController lastSelectedScarrab, ScarrabController newSelectedScarrab)
        {
            if (lastSelectedScarrab.IsNeighbour(newSelectedScarrab) && !lastSelectedScarrab.IsConnectionActive(newSelectedScarrab))
            {
                ActivateScarrab(lastSelectedScarrab);
                SetConnection(lastSelectedScarrab, newSelectedScarrab, true);
                SelectScarrab(newSelectedScarrab);
            }
            CheckSolveConditions();
        }

        private void RevertLastSelectionAndConnection(ScarrabController selectedScarrabToDeactivate)
        {
            selectedScarrabToDeactivate.SetScarrabSelection(false);
            if (activatedScarrabs.Count > 0)
            {
                ScarrabController lastScarrabInHistory = activatedScarrabs.Pop();
                SelectScarrab(lastScarrabInHistory);
                if (lastScarrabInHistory.IsConnectionActive(selectedScarrabToDeactivate))
                {
                    lastScarrabInHistory.SetScarrabActivation(false);
                    SetConnection(lastScarrabInHistory, selectedScarrabToDeactivate, false);
                }
            }
            else
            {
                selectedScarrabToDeactivate.SetScarrabSelection(false);
                lastSelectedScarrab = null;
            }
        }

        private void SetConnection(ScarrabController firstScarrab, ScarrabController secondScarrab, bool isSet)
        {
            firstScarrab.SetConnection(secondScarrab, isSet);
            secondScarrab.SetConnection(firstScarrab, isSet);
            if (isSet)
            {
                ConnectionVisual connection = visualFactory.Spawn(connectionPrefab, connectionsParent) as ConnectionVisual;
                connection.SetConnection(firstScarrab, secondScarrab, signalBus);
                connection.gameObject.SetActive(true);
                connections.Add(connection);
            }
            else
            {
                ConnectionVisual connection = connections.Find(connectionVisual => Array.Exists(connectionVisual.Scarrabs, scarab => scarab == firstScarrab)
                                                                                   && Array.Exists(connectionVisual.Scarrabs, scarab => scarab == secondScarrab));
                if (connection != null)
                {
                    connections.Remove(connection);
                    connection.Release();
                }
            }
        }
    }
}
