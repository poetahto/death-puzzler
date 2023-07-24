using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class PuzzleWorldGrid : IEnumerable<Entity>
    {
        private readonly Vector3Int _size;
        private readonly Entity _defaultEntityPrefab;
        private readonly List<Entity> _entityInstances;
        private readonly Transform _parent;

        public event Action<Entity> OnCreateEntity;
        public event Action<MoveEvent> OnMove;

        public PuzzleWorldGrid(Vector3Int size, Entity defaultEntityPrefab, Transform parent)
        {
            _size = size;
            _defaultEntityPrefab = defaultEntityPrefab;
            _entityInstances = new List<Entity>(size.x * size.y * size.z);
            _parent = parent;
        }

        public void Initialize()
        {
            for (int z = 0; z < _size.z; z++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int x = 0; x < _size.x; x++)
                    {
                        var position = new Vector3Int(x, y, z);
                        _entityInstances.Add(CreateEntity(position, _defaultEntityPrefab));
                    }
                }
            }
        }

        public Entity Get(Vector3Int position)
        {
            position = ClampPosition(position);
            return _entityInstances[GetIndex(position)];
        }

        public void Set(Vector3Int position, Entity instance)
        {
            position = ClampPosition(position);

            Entity currentEntity = Get(position);

            if (currentEntity == instance)
                return;

            currentEntity.PuzzleDestroy();

            instance.Position = position;
            instance.PuzzleCreate(this);
            _entityInstances[GetIndex(position)] = instance;
        }

        public void Delete(Vector3Int position)
        {
            position = ClampPosition(position);
            int index = GetIndex(position);

            Entity oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            CreateEntity(position, _defaultEntityPrefab);
        }

        public void Move(Entity entity, Vector3Int newPosition)
        {
            Vector3Int oldPosition = entity.Position;
            newPosition = ClampPosition(newPosition);
            var index = GetIndex(newPosition);

            var oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            _entityInstances[index] = entity;
            _entityInstances[GetIndex(oldPosition)] = CreateEntity(oldPosition, _defaultEntityPrefab);
            entity.Position = newPosition;
            OnMove?.Invoke(new MoveEvent{entity = entity, from = oldPosition, to = newPosition});
        }

        private Entity CreateEntity(Vector3Int position, Entity prefab)
        {
            Entity instance = Object.Instantiate(prefab, _parent);
            instance.Position = position;
            instance.PuzzleCreate(this);
            OnCreateEntity?.Invoke(instance);
            return instance;
        }

        private int GetIndex(Vector3Int position)
        {
            return position.x + (position.y * _size.x) + (position.z * (_size.y * _size.x));
        }

        public bool InBounds(Vector3Int position)
        {
            return position is { x: >= 0, y: >= 0, z: >= 0 }
                   && position.x < _size.x
                   && position.y < _size.y
                   && position.z < _size.z;
        }

        private Vector3Int ClampPosition(Vector3Int position)
        {
            return new Vector3Int
            {
                x = Mathf.Clamp(position.x, 0, _size.x - 1),
                y = Mathf.Clamp(position.y, 0, _size.y - 1),
                z = Mathf.Clamp(position.z, 0, _size.z - 1),
            };
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _entityInstances.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // === Structures ===

        public struct MoveEvent
        {
            public Entity entity;
            public Vector3Int from;
            public Vector3Int to;
        }
    }
}
