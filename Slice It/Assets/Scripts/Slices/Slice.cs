using UnityEngine;


namespace SliceIt.Slices
{
    public sealed class Slice : MonoBehaviour
    {
        [SerializeField] private Collider colliderToDisabled;
        [SerializeField] private GameObject toDisabled;
        [SerializeField] private GameObject toEnabled;
        [SerializeField] private GameObject particle;

        public void Execute()
        {
            colliderToDisabled.enabled = false;
            toDisabled.SetActive(false);
            toEnabled.SetActive(true);
            InstantiateParticles();
        }

        private void InstantiateParticles()
        {
            GameObject particle = Instantiate(this.particle, transform.position, Quaternion.identity);
            Destroy(particle, 1f);
        }
    }
}
