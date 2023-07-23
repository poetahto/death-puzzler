using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class PuzzleWorldGrid : IEnumerable<PuzzleWorldEntity>
    {
        private readonly Vector3Int _size;
        private readonly PuzzleWorldEntity _defaultEntityPrefab;
        private readonly List<PuzzleWorldEntity> _entityInstances;
        private readonly Transform _parent;

        public event Action<PuzzleWorldEntity> OnCreateEntity;

        public PuzzleWorldGrid(Vector3Int size, PuzzleWorldEntity defaultEntityPrefab, Transform parent)
        {
            _size = size;
            _defaultEntityPrefab = defaultEntityPrefab;
            _entityInstances = new List<PuzzleWorldEntity>(size.x * size.y * size.z);
            _parent = parent;

            for (int z = 0; z < _size.z; z++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int x = 0; x < _size.x; x++)
                    {
                        var position = new Vector3Int(x, y, z);
                        _entityInstances.Add(CreateEntity(position, defaultEntityPrefab));
                    }
                }
            }
        }

        public PuzzleWorldEntity Get(Vector3Int position)
        {
            position = ClampPosition(position);
            return _entityInstances[GetIndex(position)];
        }

        public void Set(Vector3Int position, PuzzleWorldEntity instance)
        {
            position = ClampPosition(position);

            PuzzleWorldEntity currentEntity = Get(position);
            currentEntity.PuzzleDestroy();

            instance.Position = position;
            instance.PuzzleCreate(this);
            _entityInstances[GetIndex(position)] = instance;
        }

        public void Delete(Vector3Int position)
        {
            position = ClampPosition(position);
            int index = GetIndex(position);

            PuzzleWorldEntity oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            _entityInstances[index] = CreateEntity(position, _defaultEntityPrefab);
        }

        public void Move(PuzzleWorldEntity entity, Vector3Int position)
        {
            position = ClampPosition(position);
            var index = GetIndex(position);

            var oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            _entityInstances[index] = entity;
            _entityInstances[GetIndex(entity.Position)] = CreateEntity(entity.Position, _defaultEntityPrefab);
            entity.Position = position;
        }

        private PuzzleWorldEntity CreateEntity(Vector3Int position, PuzzleWorldEntity prefab)
        {
            PuzzleWorldEntity instance = Object.Instantiate(prefab, _parent);
            instance.Position = position;
            instance.PuzzleCreate(this);
            OnCreateEntity?.Invoke(instance);
            return instance;
        }

        private int GetIndex(Vector3Int position)
        {
            return position.x + (position.y * _size.x) + (position.z * (_size.y * _size.x));
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

        public IEnumerator<PuzzleWorldEntity> GetEnumerator()
        {
            return _entityInstances.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
