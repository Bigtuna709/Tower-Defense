using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "EnemySO", order = 1)]
public class EnemySO : ScriptableObject
{
    [SerializeField] string _enemyName;
    [SerializeField] int _enemyHealth;
    [SerializeField] float _enemySpeed;
    [SerializeField] int _goldReward;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] int _waveNum;
    [SerializeField] int _spawnCount;
    [SerializeField] float _spawnDelay;
    [SerializeField] int _amountToPool;

    public string EnemyName
    {
        get
        {
            return _enemyName;
        }
    }
    public int EnemyHealth
    {
        get
        {
        return _enemyHealth;
        }
    }
    public float EnemySpeed
    {
        get
        {
            return _enemySpeed;
        }
    }
    public int GoldReward
    {
        get
        {
            return _goldReward;
        }
    }
    public GameObject EnemyPrefab
    {
        get
        {
            return _enemyPrefab;
        }
    }
    public int WaveNumber
    {
        get
        {
            return _waveNum;
        }
    }
    public int SpawnCount
    {
        get
        {
            return _spawnCount;
        }
    }
    public float SpawnDelay
    {
        get
        {
            return _spawnDelay;
        }
    }
    public int AmountToPool
    {
        get
        {
            return _amountToPool;
        }
    }
}
