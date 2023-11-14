using UnityEngine;

namespace Scarrab.Zenject
{
    public class ScarrabClicked
    {
        public GameObject ScarrabVisual { get; private set; }

        public ScarrabClicked(GameObject scarrab)
        {
            ScarrabVisual = scarrab;
        }
    }
}