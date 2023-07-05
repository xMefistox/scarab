using Scarab.Input;
using Scarab.RiddleVisual;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Scarab.RiddleLogic
{
    public class RiddleController : MonoBehaviour
    {
        [SerializeField] private InteractController interactController;
        [SerializeField] private ScarabController[] scarabControllers;
        [SerializeField] private ConnectionVisualSpawner connectionVisualSpawner;
        [SerializeField] private GameObject ResetButton;
        [SerializeField] private TextMeshProUGUI winText;

        private ScarabController lastSelectedScarab;
        private Stack<ScarabController> activatedScarabs = new Stack<ScarabController>();

        private bool riddleSolved;

        private void OnEnable()
        {
            interactController.OnScarabClicked += OnScarabClicked;
            interactController.OnResetButtonClicked += OnResetButtonClicked;
            winText.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            interactController.OnScarabClicked -= OnScarabClicked;
            interactController.OnResetButtonClicked -= OnResetButtonClicked;
        }

/*        private void Update() // CHEAT FOR DEBUG PURPOSES
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Z) && gameObject.name=="RiddleWall")
            {
                foreach(ScarabController scarabController in scarabControllers)
                {
                    foreach(ScarabController neighbour in scarabController.neighboursList)
                    {
                        TryActivateConnectionBetweenScarabs(scarabController, neighbour);
                    }
                }
                ActivateScarab(lastSelectedScarab);
            }
        }*/

        private void OnResetButtonClicked()
        {
            winText.gameObject.SetActive(false);
            connectionVisualSpawner.Reset();
            foreach(ScarabController scarabController in scarabControllers)
            {
                scarabController.Reset();
            }
            activatedScarabs.Clear();
            lastSelectedScarab = null;
            riddleSolved = false;
        }

        private void OnScarabClicked(GameObject scarabVisual)
        {
            if(riddleSolved)
            {
                return;
            }
            ScarabController newSelectedScarab = Array.Find(scarabControllers, scarabController => scarabController.VisualController.gameObject == scarabVisual);
            if (newSelectedScarab != null)
            {
                switch (newSelectedScarab.State)
                {
                    case ScarabState.Inactive:

                        if (lastSelectedScarab == null)
                        {
                            SelectScarab(newSelectedScarab);
                        }
                        else
                        {
                            TryActivateConnectionBetweenScarabs(lastSelectedScarab, newSelectedScarab);
                        }
                        break;
                    case ScarabState.Selected:
                        if (lastSelectedScarab == newSelectedScarab)
                        {
                            RevertLastSelectionAndConnection(lastSelectedScarab);
                        }
                        break;
                    case ScarabState.Active:
                        TryActivateConnectionBetweenScarabs(lastSelectedScarab, newSelectedScarab);
                        break;
                }
            }

            ResetButton.gameObject.SetActive(lastSelectedScarab != null);
        }

        private void CheckSolveConditions()
        {
            if (scarabControllers.Any(scarab => scarab.AtLeastOneNeighbourIsNotConnected()))
            {
                return;
            }
            ActivateScarab(lastSelectedScarab);
            ResetButton.SetActive(true);
            riddleSolved = true;
            winText.gameObject.SetActive(true);
        }

        private void SelectScarab(ScarabController scarabController)
        {
            scarabController.SetScarabSelection(true);
            lastSelectedScarab = scarabController;
        }

        private void ActivateScarab(ScarabController scarabController)
        {
            lastSelectedScarab = scarabController;
            if (activatedScarabs.Count == 0 || activatedScarabs.Last() != scarabController)
            {
                activatedScarabs.Push(lastSelectedScarab);
            }
            scarabController.SetScarabActivation(true);
            scarabController.SetScarabSelection(false);
        }

        private void TryActivateConnectionBetweenScarabs(ScarabController lastSelectedScarab, ScarabController newSelectedScarab)
        {
            if (lastSelectedScarab.IsNeighbour(newSelectedScarab) && !lastSelectedScarab.IsConnectionActive(newSelectedScarab))
            {
                ActivateScarab(lastSelectedScarab);
                SetConnection(lastSelectedScarab, newSelectedScarab, true);
                SelectScarab(newSelectedScarab);
            }
            CheckSolveConditions();
        }

        private void RevertLastSelectionAndConnection(ScarabController selectedScarabToDeactivate)
        {
            selectedScarabToDeactivate.SetScarabSelection(false);
            if (activatedScarabs.Count > 0)
            {
                ScarabController lastScarabInHistory = activatedScarabs.Pop();
                SelectScarab(lastScarabInHistory);
                if (lastScarabInHistory.IsConnectionActive(selectedScarabToDeactivate))
                {
                    lastScarabInHistory.SetScarabActivation(false);
                    SetConnection(lastScarabInHistory, selectedScarabToDeactivate, false);
                }
            }
            else
            {
                selectedScarabToDeactivate.SetScarabSelection(false);
                lastSelectedScarab = null;
            }
        }

        private void SetConnection(ScarabController firstScarab, ScarabController SecondScarab, bool isSet)
        {
            firstScarab.SetConnection(SecondScarab, isSet);
            SecondScarab.SetConnection(firstScarab, isSet);
            if(isSet)
            {
                connectionVisualSpawner.SpawnConnection(firstScarab, SecondScarab);
            }
            else
            {
                connectionVisualSpawner.DespawnConnection(firstScarab, SecondScarab);
            }
        }
    }
}
