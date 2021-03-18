using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;

    int bulletSpawnAmount = 10;
    int rocketSpawnAmount = 10;

    List<GameObject> allBulletsCreated = new List<GameObject>();
    List<GameObject> allRocketsCreated = new List<GameObject>();

    public List<GameObject> AllBulletsCreated { get { return allBulletsCreated; } }
    public List<GameObject> AllRocketsCreated { get { return allRocketsCreated; } }
    public int BulletSpawnAmount { get { return bulletSpawnAmount; } }
    public int RocketSpawnAmount { get { return rocketSpawnAmount; } }
    private void Start()
    {
        for(int i = 0; i < bulletSpawnAmount; i++)
        {
            CreateBullet();
        }
        for(int i = 0; i < rocketSpawnAmount; i++)
        {
            CreateRocket();
        }
    }

    public GameObject CreateBullet()
    {
        GameObject go = Instantiate(bulletPrefab);
        go.SetActive(false);
        AllBulletsCreated.Add(go);
        return go;
    }
    public GameObject CreateRocket()
    {
        GameObject go = Instantiate(rocketPrefab);
        go.SetActive(false);
        AllRocketsCreated.Add(go);
        return go;
    }

    public GameObject GetBullet()
    {
        {
            for (int i = 0; i < AllBulletsCreated.Count; i++)
            {
                if (!AllBulletsCreated[i].activeInHierarchy)
                {
                    return AllBulletsCreated[i];
                }
            }
        }
            return CreateBullet();
    }
    public GameObject GetRocket()
    {
        for(int i = 0; i < AllRocketsCreated.Count; i++)
        {
            if(!AllRocketsCreated[i].activeInHierarchy)
            {
                return AllRocketsCreated[i];
            }
        }
        return CreateRocket();
    }
}
