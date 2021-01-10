﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    List<GameObject> _enemies = new List<GameObject>();
    Transform target;

    public Transform fireLocation;
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;
    public string towerName;

    [HideInInspector]
    public int towerSellCost;

    [SerializeField] float towerFireTime;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            if (!_enemies.Contains(go))
            {
                _enemies.Add(go);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            _enemies.Remove(go);
        }
    }
    void OnTriggerStay(Collider other)
    {
        for(int i = _enemies.Count - 1; i >= 0; i--)
        {
            if(_enemies[i] != null)
            {
                target = _enemies[i].transform;
            }
            else
            {
                _enemies.RemoveAt(i);
            }
        }
        
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            Fire();
        }
    }

    bool ReadyToFire() => Time.time >= towerFireTime;
    void Fire()
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if(tower != null)
        {
            if (ReadyToFire())
            {
                if (tower.BulletPrefab != null)
                {
                    var instance = (GameObject)Instantiate(tower.BulletPrefab, fireLocation.position, Quaternion.identity);
                    Bullet bullet = instance.GetComponent<Bullet>();

                    if (bullet != null)
                    {
                        bullet.LookForTarget(target);
                        bullet.bulletDamage = tower.TowerDamage;
                    }

                    towerFireTime = Time.time + tower.TowerRateOfFire;
                }
                else
                {
                    return;
                }
            }
        }
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!BuildManager.Instance.buildMode)
        {
            BuildManager.Instance.SelectTower(this);
            return;
        }
    }
}
