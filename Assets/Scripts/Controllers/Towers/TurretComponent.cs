using UnityEngine;
using System.Linq;

public class TurretComponent : MonoBehaviour, IFireable
{
    [SerializeField] private float towerFireTime;
    [SerializeField] private TowerType towerType;
    [SerializeField] private Transform fireLocation;
    public Transform target;

    bool ReadyToFire() => Time.time >= towerFireTime;
    public void FireWeapon()
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if (tower != null)
        {
            if (ReadyToFire())
            {
                if (tower.BulletPrefab != null)
                {
                    if (tower.BulletPrefab.CompareTag("Bullet"))
                    {
                        Transform newBullet = ObjectPoolManager.Instance.GetBullet().transform;
                        newBullet.transform.position = fireLocation.transform.position;
                        newBullet.gameObject.SetActive(true);
                        Bullet bullet = newBullet.gameObject.GetComponent<Bullet>();
                        bullet.LookForTarget(target);
                        bullet.bulletDamage = tower.TowerDamage;
                    }
                    else
                    {
                        Transform newRocket = ObjectPoolManager.Instance.GetRocket().transform;
                        newRocket.transform.position = fireLocation.transform.position;
                        newRocket.gameObject.SetActive(true);
                        Bullet rocket = newRocket.gameObject.GetComponent<Bullet>();
                        rocket.LookForTarget(target);
                        rocket.bulletDamage = tower.TowerDamage;
                        rocket.rocketAreaDamage = tower.TowerAreaDamage;
                        rocket.rocketDamageArea = tower.TowerAreaDamageRadius;
                    }
                }
                else
                {
                    return;
                }
                towerFireTime = Time.time + tower.TowerRateOfFire;
            }
        }
    }    
}
