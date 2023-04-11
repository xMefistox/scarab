using Scarab.Input;
using Scarab.RiddleVisual;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scarab.RiddleLogic
{
    public class RiddleController : MonoBehaviour
    {
        [SerializeField] private InteractController interactController;
        [SerializeField] private ScarabController[] scarabControllers;
        [SerializeField] private ConnectionVisualSpawner connectionVisualSpawner;
        [SerializeField] private GameObject ResetButton;

        private ScarabController lastSelectedScarab;
        private Stack<ScarabController> activatedScarabs = new Stack<ScarabController>();

        private void OnEnable()
        {
            interactController.OnScarabClicked += OnScarabClicked;
            interactController.OnResetButtonClicked += OnResetButtonClicked;
            ResetButton.SetActive(false);
        }

        private void OnDisable()
        {
            interactController.OnScarabClicked -= OnScarabClicked;
            interactController.OnResetButtonClicked -= OnResetButtonClicked;
        }

        private void OnResetButtonClicked()
        {
            ResetButton.gameObject.SetActive(false);
            while(activatedScarabs.Count > 0)
            {
                RevertLastSelectionAndConnection(lastSelectedScarab);
            }
        }

        private void OnScarabClicked(GameObject scarabVisual)
        {
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
            if (scarabControllers.Any(scarab => !scarab.AllNeighboursConnected()))
            {
                return;
            }
            ActivateScarab(lastSelectedScarab);
            Debug.Log("win!");
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
