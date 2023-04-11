using Scarab.RiddleVisual;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scarab.RiddleLogic
{
    public class ScarabController : MonoBehaviour
    {
        [field: SerializeField] public ScarabState State { get; private set; }
        [field: SerializeField] public ScarabVisualController VisualController { get; private set; }

        [SerializeField] private List<ScarabController> neighboursList = new List<ScarabController>();

        private Dictionary<ScarabController, bool> neighboursDictionary = new Dictionary<ScarabController, bool>();

        private int activationCount;

        private void Awake()
        {
            neighboursList.ForEach(neighbours => neighboursDictionary.Add(neighbours, false));
        }

        internal void SetScarabActivation(bool isActivated)
        {
            if (isActivated)
            {
                activationCount++;
            }
            else
            {
                activationCount--;
                activationCount = Mathf.Clamp(activationCount, 0, neighboursList.Count);
            }
        }

        internal bool IsNeighbour(ScarabController potentialScarabNeighbour)
        {
            return neighboursDictionary.ContainsKey(potentialScarabNeighbour);
        }

        internal void SetConnection(ScarabController secondScarab, bool isSet)
        {
            neighboursDictionary[secondScarab] = isSet;
        }

        internal bool IsConnectionActive(ScarabController neighbourScarab)
        {
            return neighboursDictionary[neighbourScarab];
        }

        internal void SetScarabSelection(bool isSelected)
        {
            if (isSelected)
            {
                SetScarabState(ScarabState.Selected);
            }
            else
            {
                if (activationCount == 0)
                {
                    SetScarabState(ScarabState.Inactive);
                }
                else
                {
                    SetScarabState(ScarabState.Active);
                }
            }
        }

        private void SetScarabState(ScarabState state)
        {
            State = state;
            VisualController.ChangeScarabVisual((ScarabVisualState)state);
        }

        internal bool AllNeighboursConnected()
        {
            return neighboursDictionary.Values.Any(isConnection => isConnection == false);
        }
    }
}
