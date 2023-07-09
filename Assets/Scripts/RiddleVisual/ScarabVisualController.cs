using UnityEngine;

namespace Scarab.RiddleVisual
{
    public class ScarabVisualController : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer renderer;

        [SerializeField]
        private Material rockMaterial;

        [SerializeField] 
        private Material blueMaterial;

        [SerializeField] 
        private Material goldMaterial;

        public void ChangeScarabVisual(ScarabVisualState state)
        {
            switch(state)
            {
                case ScarabVisualState.Inactive:
                    renderer.material = rockMaterial;
                        break;

                case ScarabVisualState.Active:
                    renderer.material = goldMaterial;
                    break;

                case ScarabVisualState.Selected:
                    renderer.material = blueMaterial;
                    break;
            }
        }
    }
}
