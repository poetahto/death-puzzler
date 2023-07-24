using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class PuzzleWorldGrid : IEnumerable<PuzzleEntity>
    {
        private readonly Vector3Int _size;
        private readonly PuzzleEntity _defaultEntityPrefab;
        private readonly List<PuzzleEntity> _entityInstances;
        private readonly Transform _parent;

        public event Action<PuzzleEntity> OnCreateEntity;

        public PuzzleWorldGrid(Vector3Int size, PuzzleEntity defaultEntityPrefab, Transform parent)
        {
            _size = size;
            _defaultEntityPrefab = defaultEntityPrefab;
            _entityInstances = new List<PuzzleEntity>(size.x * size.y * size.z);
            _parent = parent;
        }

        public void Initialize()
        {
            Debug.Log("initialied grid");
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

        public PuzzleEntity Get(Vector3Int position)
        {
            position = ClampPosition(position);
            return _entityInstances[GetIndex(position)];
        }

        public void Set(Vector3Int position, PuzzleEntity instance)
        {
            position = ClampPosition(position);

            PuzzleEntity currentEntity = Get(position);
            currentEntity.PuzzleDestroy();

            instance.Position = position;
            instance.PuzzleCreate(this);
            _entityInstances[GetIndex(position)] = instance;
        }

        public void Delete(Vector3Int position)
        {
            position = ClampPosition(position);
            int index = GetIndex(position);

            PuzzleEntity oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            _entityInstances[index] = CreateEntity(position, _defaultEntityPrefab);
        }

        public void Move(PuzzleEntity entity, Vector3Int position)
        {
            position = ClampPosition(position);
            var index = GetIndex(position);

            var oldEntity = _entityInstances[index];
            oldEntity.PuzzleDestroy();

            _entityInstances[index] = entity;
            _entityInstances[GetIndex(entity.Position)] = CreateEntity(entity.Position, _defaultEntityPrefab);
            entity.Position = position;
        }

        private PuzzleEntity CreateEntity(Vector3Int position, PuzzleEntity prefab)
        {
            PuzzleEntity instance = Object.Instantiate(prefab, _parent);
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

        public IEnumerator<PuzzleEntity> GetEnumerator()
        {
            return _entityInstances.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
