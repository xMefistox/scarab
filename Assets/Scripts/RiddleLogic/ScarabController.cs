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

        [SerializeField] public List<ScarabController> neighboursList = new List<ScarabController>();

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
            if (neighboursDictionary.ContainsKey(neighbourScarab))
            {
                return neighboursDictionary[neighbourScarab];
            }
            return false;
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

        internal void Reset()
        {
            activationCount = 0;
            SetScarabState(ScarabState.Inactive);
            foreach (ScarabController neighbour in neighboursList)
            {
                neighboursDictionary[neighbour] = false;

            }
        }

        private void SetScarabState(ScarabState state)
        {
            if(State != state)
            {
                State = state;
                VisualController.ChangeScarabVisual((ScarabVisualState)state);
            }
        }

        internal bool AtLeastOneNeighbourIsNotConnected()
        {
            return neighboursDictionary.Values.Any(isConnection => isConnection == false);
        }
    }
}
