using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class GameManager : Singleton<GameManager>
{
    public List<EnemySO> _enemies = new List<EnemySO>();
    public List<GameObject> builtTowers = new List<GameObject>();

    public int playerTotalGold;
    public int playerTotalLives;
    public int enemiesSpawned;

    public bool isSpawning;
    public bool isPaused;
    public bool isFastForwarding;
    
    [SerializeField] int _waveNumber;

    [Header("UI Elements")]
    public GameObject startWaveBTN;
    public GameObject playerHUD;
    public GameObject buildModePanel;
    public Text playerLivesText;
    public Text playerGoldText;
    public Text waveNumText;

    [SerializeField] Transform spawnLocation;
    [SerializeField] int _winWaveNum;
   
    public Transform finishLine;

    private void Start()
    {
        isPaused = false;
        playerGoldText.text = playerTotalGold.ToString();
        playerLivesText.text = playerTotalLives.ToString();
        waveNumText.text = "Current Wave: " + _waveNumber.ToString();
    }
    public void StartWave()
    {
        Debug.Log("<color=green>Wave Incoming! - TODO: Display on screen</color>");

        startWaveBTN.GetComponent<Button>().interactable = false;
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        isSpawning = true;
        enemiesSpawned = 0;

        foreach (EnemySO enemy in _enemies)
        {
            if (enemy.WaveNumber == _waveNumber)
            {
                for (int i = 0; i < enemy.SpawnCount; i++)
                {
                    yield return new WaitForSeconds(enemy.SpawnDelay);
                    var instance = GameObject.Instantiate(enemy.EnemyPrefab, spawnLocation.position, transform.rotation);
                    instance.GetComponent<NavMeshAgent>().speed = enemy.EnemySpeed;

                    var instanceStats = instance.GetComponent<EnemyController>();
                    instanceStats.totalEnemyHealth = enemy.EnemyHealth;
                    instanceStats.enemyReward = enemy.GoldReward;
                    enemiesSpawned++;
                }
            }
        }
            isSpawning = false;
    }

    public void CheckForWaveOver()
    {
        bool WaveOver = !isSpawning && enemiesSpawned <= 0;
        if (WaveOver)
        {
            _waveNumber++;
            startWaveBTN.GetComponent<Button>().interactable = true;
            waveNumText.text = "Current wave: " + _waveNumber.ToString();
        }
    }
    public void CheckForGameOver()
    {
        if (playerTotalLives == 0)
        {
            GameOverLose();
        }
        if(enemiesSpawned == 0 && _waveNumber == _winWaveNum)
        {
            GameOverWin();
        }
    }
    public void ShowGoldChange(int amount)
    {
        playerTotalGold += amount;
        Instance.playerGoldText.text = playerTotalGold.ToString();
    }
    public void EnemyDiedOrRemoved()
    {
        enemiesSpawned--;
        CheckForWaveOver();
        CheckForGameOver();
    }

    public void RemoveEnemyFromTowers(GameObject enemy)
    {
        foreach(GameObject tower in builtTowers)
        {
            var towerController = tower.GetComponent<TowerController>();
            if (towerController._Enemies.Count > 0)
            {
                if(towerController._Enemies.Contains(enemy))
                {
                    towerController._Enemies.Remove(enemy);
                    Debug.Log("Enemy Removed");
                    if (towerController._Enemies.Count == 0)
                    {
                        towerController.flameEffectIsPlaying = false;
                        towerController.FlameController();
                    }
                }
                else
                {
                    Debug.Log("Enemy not in _Enemies list!");
                }
            }           
        }
    }

    void GameOverLose()
    {
        Debug.Log("<color=blue>Game Over - TODO: Display on screen</color>");
    }

    void GameOverWin()
    {
        Debug.Log("<color=blue>You Win! - YODO: Display on screen</color>");
    }
    
    public void PauseAndUnPauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void Fastest()
    {
        Time.timeScale = 6;
    }
    public void Faster()
    {
        Time.timeScale = 4;
    }
    public void Fast()
    {
        Time.timeScale = 2;
    }
    public void NormalSpeed()
    {
        Time.timeScale = 1;
    }
}
