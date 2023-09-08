using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class Egg : EntityBehaviour
    {
        [SerializeField] private Entity entityPrefab;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Renderer renderer;
        [SerializeField] private GameObject hatchHint;

        public void Select()
        {
            virtualCamera.Priority = 100;
            particles.Play();
            hatchHint.SetActive(true);
        }

        public void Deselect()
        {
            virtualCamera.Priority = 0;
            particles.Stop();
            hatchHint.SetActive(false);
        }

        public Entity Hatch()
        {
            Deselect();
            var instance = Instantiate(entityPrefab, Entity.Position, Quaternion.identity);
            renderer.enabled = false;
            return instance;
        }
    }
}
