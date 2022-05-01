using UnityEngine;


namespace SliceIt.Slices
{
    public sealed class Slice : MonoBehaviour
    {
        [SerializeField] private Collider colliderToDisabled;
        [SerializeField] private GameObject toDisabled;
        [SerializeField] private GameObject toEnabled;

        public void Execute()
        {
            colliderToDisabled.enabled = false;
            toDisabled.SetActive(false);
            toEnabled.SetActive(true);
        }
    }
}
