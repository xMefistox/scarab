using Scarrab.RiddleVisual;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scarrab.RiddleLogic
{
    public class ScarrabController : MonoBehaviour
    {
        [field: SerializeField] public ScarrabState State { get; private set; }
        [field: SerializeField] public ScarrabVisualController VisualController { get; private set; }

        [SerializeField] public List<ScarrabController> neighboursList = new List<ScarrabController>();

        private Dictionary<ScarrabController, bool> neighboursDictionary = new Dictionary<ScarrabController, bool>();

        private int activationCount;

        private void Awake()
        {
            neighboursList.ForEach(neighbours => neighboursDictionary.Add(neighbours, false));
        }

        internal void SetScarrabActivation(bool isActivated)
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

        internal bool IsNeighbour(ScarrabController potentialScarrabNeighbour)
        {
            return neighboursDictionary.ContainsKey(potentialScarrabNeighbour);
        }

        internal void SetConnection(ScarrabController secondScarrab, bool isSet)
        {
            neighboursDictionary[secondScarrab] = isSet;
        }

        internal bool IsConnectionActive(ScarrabController neighbourScarrab)
        {
            if (neighboursDictionary.ContainsKey(neighbourScarrab))
            {
                return neighboursDictionary[neighbourScarrab];
            }
            return false;
        }

        internal void SetScarrabSelection(bool isSelected)
        {
            if (isSelected)
            {
                SetScarrabState(ScarrabState.Selected);
            }
            else
            {
                if (activationCount == 0)
                {
                    SetScarrabState(ScarrabState.Inactive);
                }
                else
                {
                    SetScarrabState(ScarrabState.Active);
                }
            }
        }

        internal void Reset()
        {
            activationCount = 0;
            SetScarrabState(ScarrabState.Inactive);
            foreach (ScarrabController neighbour in neighboursList)
            {
                neighboursDictionary[neighbour] = false;

            }
        }

        private void SetScarrabState(ScarrabState state)
        {
            if(State != state)
            {
                State = state;
                VisualController.ChangeScarrabVisual((ScarrabVisualState)state);
            }
        }

        internal bool AtLeastOneNeighbourIsNotConnected()
        {
            return neighboursDictionary.Values.Any(isConnection => isConnection == false);
        }
    }
}
