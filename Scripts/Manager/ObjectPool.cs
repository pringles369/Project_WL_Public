using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    public static ObjectPool<T> Instance;

    [SerializeField]
    private T prefab;
    private Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        Instance = this;
    }

    public T SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        if (objects.Count == 0)
        {
            AddObjectToPool();
        }

        T obj = objects.Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);

        IPooledObject pooledObj = obj as IPooledObject;
        pooledObj?.OnObjectSpawn();

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }

    private void AddObjectToPool()
    {
        T newObj = Instantiate(prefab);
        newObj.gameObject.SetActive(false);
        objects.Enqueue(newObj);
    }
}