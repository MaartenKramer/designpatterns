using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _objectPrefab;
    private readonly Transform _parentTransform;
    private readonly int _poolCapacity;

    public ObjectPool(T objectPrefab, int poolCapacity, Transform parent = null)
    {
        _objectPrefab = objectPrefab;
        _parentTransform = parent;
        _poolCapacity = poolCapacity;

        for (int i = 0; i < _poolCapacity; i++)
        {
            T newObj = GameObject.Instantiate(_objectPrefab, _parentTransform);
            newObj.gameObject.SetActive(false);
            _pool.Enqueue(newObj);
        }
    }

    public T GetObject()
    {
        if (_pool.Count > 0)
        {
            T pooledObject = _pool.Dequeue();
            pooledObject.gameObject.SetActive(true);
            _pool.Enqueue(pooledObject);
            return pooledObject;
        }
        return null;
    }
}
