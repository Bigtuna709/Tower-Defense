using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    public List<GameObject> _Enemies = new List<GameObject>(); 
    public float turnSpeed = 10;
    public Transform partToRotate;
    public TowerType towerType;

    public FlameComponent flameComponent;
    private TurretComponent turretComponent;

    [HideInInspector]
    public int towerSellCost;

    private void Awake()
    {
        flameComponent = GetComponent<FlameComponent>();
        turretComponent = GetComponent<TurretComponent>();
    }
    //private void OnEnable()
    //{
    //    flameComponent.CheckFlameParticle();
    //}
    private void Update()
    {
        if(_Enemies.Count == 0)
        {
            turretComponent.target = null;
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
                    flameComponent.FireWeapon();
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
                turretComponent.target = null;
                return;
            }
            flameComponent.flameEffectIsPlaying = false;
            flameComponent.FlameController();
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_Enemies.Count > 0)
            {
                // *Fix targetting* //
                turretComponent.target = _Enemies.FirstOrDefault().transform;
            }
        }
        if (turretComponent.target != null)
        {
            Vector3 dir = turretComponent.target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            turretComponent.FireWeapon();
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
