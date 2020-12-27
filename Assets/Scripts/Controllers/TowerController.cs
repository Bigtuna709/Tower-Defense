using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class TowerController : MonoBehaviour
{
    Transform target;

    public Transform fireLocation;
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;
    public string towerName;

    [SerializeField] float towerFireTime;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach(GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if(nearestEnemy != null && shortestDistance <= GetComponent<SphereCollider>().radius)
            {
                target = nearestEnemy.transform;                
            }
            else
            {
                target = null;
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
    }

    bool ReadyToFire() => Time.time >= towerFireTime;
    void Fire()
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if(tower != null)
        {
            if (ReadyToFire())
            {
                var instance = (GameObject)Instantiate(tower.BulletPrefab, fireLocation.position, transform.rotation);
                Bullet bullet = instance.GetComponent<Bullet>();

                if(bullet != null)
                {
                    bullet.LookForTarget(target);
                    bullet.bulletDamage = tower.TowerDamage;
                }

                towerFireTime = Time.time + tower.TowerRateOfFire;
            }
        }
    }

}
