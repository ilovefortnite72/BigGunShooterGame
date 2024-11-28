using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AI;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject objectPoolEmptyHolder;
    private GameObject _particleSystemPoolEmptyHolder;
    private GameObject _gameObjectPoolEmptyHolder;
    private GameObject _vfxPoolEmptyHolder;
    private GameObject _enemyPoolEmptyHolder;
    private GameObject _bulletPoolEmptyHolder;

    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        VFX,
        Enemy,
        Bullet,
        None
    }
    public static PoolType PoolingType;

    public void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");
        _particleSystemPoolEmptyHolder = new GameObject("Pooled Particle Systems");
        _particleSystemPoolEmptyHolder.transform.SetParent(objectPoolEmptyHolder.transform);
        _gameObjectPoolEmptyHolder = new GameObject("Pooled Game Objects");
        _gameObjectPoolEmptyHolder.transform.SetParent(objectPoolEmptyHolder.transform);
        _vfxPoolEmptyHolder = new GameObject("Pooled VFX");
        _vfxPoolEmptyHolder.transform.SetParent(objectPoolEmptyHolder.transform);
        _enemyPoolEmptyHolder = new GameObject("Pooled Enemies");
        _enemyPoolEmptyHolder.transform.SetParent(objectPoolEmptyHolder.transform);
        _bulletPoolEmptyHolder = new GameObject("Pooled Bullets");
        _bulletPoolEmptyHolder.transform.SetParent(objectPoolEmptyHolder.transform);

    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion spawnrotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);


        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);

        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();


        if(spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObj = Instantiate(objectToSpawn, position, spawnrotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = position;
            spawnableObj.transform.rotation = spawnrotation;
            spawnableObj.SetActive(true);
            pool.InactiveObjects.Remove(spawnableObj);
        }

        resetNavMeshAgent(spawnableObj);

        return spawnableObj;
    }

    private static void resetNavMeshAgent(GameObject obj)
    {
        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
        }
    }


    public static void ReturnObjectToPool(GameObject objectToReturn)
    {
        string goName = objectToReturn.name.Substring(0, objectToReturn.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("No pool found for object: " + objectToReturn.name);
            return;
        }
        else
        {
            objectToReturn.SetActive(false);
            pool.InactiveObjects.Add(objectToReturn);
        }
        
    }


    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.ParticleSystem:
                return GameObject.Find("Pooled Particle Systems");
            case PoolType.GameObject:
                return GameObject.Find("Pooled Game Objects");
            case PoolType.VFX:
                return GameObject.Find("Pooled VFX");
            default:
                return null;
        } 
    }

}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
