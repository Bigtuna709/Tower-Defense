using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class TowerController : MonoBehaviour
{
    public List<GameObject> _Enemies = new List<GameObject>(); 
    public Transform target;
    float towerFireTime;

    public Transform fireLocation;
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;
    public VisualEffect flameEffect;
    public bool flameEffectIsPlaying = false;
    public GameObject flameParticleGO;
    public ParticleSystem flameParticle;

    [HideInInspector]
    public int towerSellCost;

    private void OnEnable()
    {
        CheckFlameParticle();
    }
    private void Update()
    {
        if(_Enemies.Count == 0)
        {
            target = null;
        }
    }

    private void CheckFlameParticle()
    {
        if (flameParticleGO != null)
        {
            flameParticleGO.SetActive(false);
        }
        else
        {
            return;
        }
    }

    public void FlameController()
    {
        CheckFlameParticle();
        if(flameEffect == null)
        {
            return;
        }
        if(flameEffectIsPlaying)
        {
            flameEffect.Play();
            flameParticleGO.SetActive(true);
            Debug.Log("Played part");
        }
        else
        {
            flameEffect.Stop();
            flameParticleGO.SetActive(false);
            Debug.Log("Stopped part");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject;
            if (!_Enemies.Contains(go))
            {
                _Enemies.Add(go);
                ShootFlame();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //GameManager.Instance.RemoveEnemyFromTowers(other.gameObject);
            _Enemies.Remove(other.gameObject);
            if(_Enemies.Count > 0)
            {
                target = null;
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
            if (_Enemies.Count > 0)
            {
                // *Fix targetting* //
                target = _Enemies.FirstOrDefault().transform;
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
                var flame = flameParticle.GetComponent<Flamethrower>();
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
                        rocket.rocketDamageArea = tower.TowerAreaDamageRadius;                    }
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
