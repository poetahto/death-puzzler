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

        public void Select()
        {
            virtualCamera.Priority = 100;
            particles.Play();
        }

        public void Deselect()
        {
            virtualCamera.Priority = 0;
            particles.Stop();
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
