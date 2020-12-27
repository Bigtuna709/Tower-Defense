using UnityEngine;
using System.Collections.Generic;

    public enum TowerType
    {
        Basic,
        Power,
        Flame,
        Magic
    }

[CreateAssetMenu(fileName = "Tower", menuName = "TowerSO", order = 2)]

public class TowerSO : ScriptableObject
{
    [SerializeField] List<TowerSO> _towerUpgrades;
    [SerializeField] string _towerName;
    [SerializeField] int _towerCost;
    [SerializeField] int _towerDamage;
    [SerializeField] float _towerRateOfFire;
    [SerializeField] GameObject _towerPrefab;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Sprite _towerImage;
    [SerializeField] TowerType _towerType;

    public List<TowerSO> TowerUpgrades
    {
        get
        {
            return _towerUpgrades;
        }
    }
    public string TowerName
    {
        get
        {
            return _towerName;
        }
    }
    public int TowerCost
    {
        get
        {
            return _towerCost;
        }
    }
    public int TowerDamage
    {
        get
        {
            return _towerDamage;
        }
    }
    public float TowerRateOfFire
    {
        get
        {
            return _towerRateOfFire;
        }
    }
    public GameObject TowerPrefab
    {
        get
        {
            return _towerPrefab;
        }
    }
    public GameObject BulletPrefab
    {
        get
        {
            return _bulletPrefab;
        }
    }
    public Sprite TowerSprite
    {
        get
        {
            return _towerImage;
        }
    }
    public TowerType TowerType
    {
        get
        {
            return _towerType;
        }
    }
}
