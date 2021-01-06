using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

    public enum TowerType
    {
        Basic1,
        Basic2,
        Basic3,
        Power1,
        Power2,
        Power3,
        Spray1,
        Spray2,
        Spray3,
        Magic1,
        Magic2,
        Magic3
    }

[CreateAssetMenu(fileName = "Tower", menuName = "TowerSO", order = 2)]

public class TowerSO : ScriptableObject
{
    [SerializeField] TowerSO _towerUpgrade;
    [SerializeField] string _towerName;
    [SerializeField] int _towerCost;
    [SerializeField] int _towerSellAmount;
    [SerializeField] int _towerDamage;
    [SerializeField] float _towerRateOfFire;
    [SerializeField] GameObject _towerPrefab;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Sprite _towerSprite;
    [SerializeField] TowerType _towerType;

    public TowerSO TowerUpgrade
    {
        get
        {
            return _towerUpgrade;
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
    public int TowerSellAmount
    {
        get
        {
            return _towerSellAmount;
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
            return _towerSprite;
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
