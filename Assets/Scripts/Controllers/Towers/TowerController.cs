using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TowerController : MonoBehaviour
{
    public List<GameObject> _Enemies = new List<GameObject>(); 
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;
    public Transform target;

    public FlameComponent flameComponent;
    private TurretComponent turretComponent;
    public GameObject towerFireRadiusIMG; 

    [HideInInspector]
    public int towerSellCost;

    private void Awake()
    {
        flameComponent = GetComponent<FlameComponent>();
        turretComponent = GetComponent<TurretComponent>();
    }
    private void Update()
    {
        if(_Enemies.Count == 0)
        {
            target = null;
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
                if (flameComponent != null)
                    flameComponent.FireWeapon(target);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _Enemies.Remove(other.gameObject);
            if(_Enemies.Count > 0)
            {
                target = null;
                return;
            }
            if (flameComponent != null)
            {
                flameComponent.flameEffectIsPlaying = false;
                flameComponent.FlameController();
            }
            
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

            if(turretComponent != null)
            turretComponent.FireWeapon(target);
        }
    }
}
