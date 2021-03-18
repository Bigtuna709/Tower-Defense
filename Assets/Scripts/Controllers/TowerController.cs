using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class TowerController : MonoBehaviour
{
    List<GameObject> _enemies = new List<GameObject>();
    public List<GameObject> _Enemies { get { return _enemies; } set { _enemies = value; } } 
    public Transform target;
    float towerFireTime;

    public Transform fireLocation;
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;
    public VisualEffect flameEffect;
    public bool flameEffectIsPlaying = false;
    public ParticleSystem flameParticle;

    [HideInInspector]
    public int towerSellCost;

    public void FlameController()
    {
        if(flameEffect == null)
        {
            return;
        }
        if(flameEffectIsPlaying)
        {
            flameEffect.Play();
            flameParticle.Play();
            Debug.Log("Played part");
        }
        else
        {
            flameEffect.Stop();
            flameParticle.Stop();
            Debug.Log("Stopped part");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            if (!_enemies.Contains(go))
            {
                _enemies.Add(go);
                ShootFlame();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            _enemies.Remove(go);
            _enemies = _enemies.Where(item => item != null).ToList();
            if(_Enemies.Count > 0)
            {
                return;
            }
            flameEffectIsPlaying = false;
            FlameController();
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_enemies.Count > 0)
            {
                // *Fix targetting* //
                _enemies = _enemies.Where(item => item != null).ToList();
                target = _enemies.FirstOrDefault().transform;
            }
        }

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            FireBullet();
        }
    }
    void ShootFlame()
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if (tower != null)
        {
            if (tower.TowerFlame != null)
            {
                flameParticle = tower.TowerFlame;
                flameEffectIsPlaying = true;
                FlameController();
                Flamethrower flame = flameParticle.GetComponent<Flamethrower>();
                if (flame != null)
                {
                    flame.flameDamage = tower.TowerDamage;
                }
            }
        }
        else
        {
            return;
        }
    }

    bool ReadyToFire() => Time.time >= towerFireTime;
    void FireBullet()
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if (tower != null)
        {
            if (ReadyToFire())
            {            
                if (tower.BulletPrefab != null)
                {
                    // var instance = (GameObject)Instantiate(tower.BulletPrefab, fireLocation.position, Quaternion.identity);
                    // Bullet bullet = instance.GetComponent<Bullet>();

                    //Transform newBullet = ObjectPoolManager.Instance.GetBullet(tower.BulletPrefab);
                    if (tower.BulletPrefab.CompareTag("Bullet"))
                    {
                        Transform newBullet = ObjectPoolManager.Instance.GetBullet().transform;
                        newBullet.transform.position = fireLocation.transform.position;
                        newBullet.gameObject.SetActive(true);
                        Bullet bullet = newBullet.gameObject.GetComponent<Bullet>();
                        bullet.LookForTarget(target);
                        bullet.bulletDamage = tower.TowerDamage;
                        //if (bullet != null)
                        //{
                        //    bullet.LookForTarget(target);
                        //    bullet.bulletDamage = tower.TowerDamage;
                        //}
                    }
                    else
                    {
                        Transform newRocket = ObjectPoolManager.Instance.GetRocket().transform;
                        newRocket.transform.position = fireLocation.transform.position;
                        newRocket.gameObject.SetActive(true);
                        Bullet rocket = newRocket.gameObject.GetComponent<Bullet>();
                        rocket.LookForTarget(target);
                        rocket.bulletDamage = tower.TowerDamage;
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
