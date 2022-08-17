using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;

    int amountOfAmmoToSpawn = 10;

    List<GameObject> allBulletsCreated = new List<GameObject>();
    List<GameObject> allRocketsCreated = new List<GameObject>();

    public List<GameObject> AllBulletsCreated { get { return allBulletsCreated; } }
    public List<GameObject> AllRocketsCreated { get { return allRocketsCreated; } }
    public int AmountOfAmmoToSpawn { get { return amountOfAmmoToSpawn; } }
    private void Start()
    {
        for(int i = 0; i < amountOfAmmoToSpawn; i++)
        {
            CreateAmmo(bulletPrefab, allBulletsCreated);
            CreateAmmo(rocketPrefab, allRocketsCreated);
        }
    }

    private GameObject CreateAmmo(GameObject objectPrefab, List<GameObject> objectList)
    {
        GameObject go = Instantiate(objectPrefab);
        go.SetActive(false);
        objectList.Add(go);
        return go;
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < AllBulletsCreated.Count; i++)
        {
            if (!AllBulletsCreated[i].activeInHierarchy)
            {
                return AllBulletsCreated[i];
            }
        }
        return CreateAmmo(bulletPrefab, allBulletsCreated);
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
        return CreateAmmo(rocketPrefab, allRocketsCreated);
    }
}
