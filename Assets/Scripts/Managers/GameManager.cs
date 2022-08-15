using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.VFX;
public class GameManager : Singleton<GameManager>
{
    public List<EnemySO> _enemies = new List<EnemySO>();
    public List<GameObject> builtTowers = new List<GameObject>();

    public int playerTotalGold;
    public int playerTotalLives;
    int enemiesSpawned;

    [HideInInspector]
    public bool isPaused;
    bool isSpawning;
    bool isFastForwarding;
    
    [SerializeField] int _waveNumber;

    [Header("UI Elements")]
    public GameObject startWaveBTN;
    public GameObject playerHUD;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject buildModePanel;
    public Text playerLivesText;
    public Text playerGoldText;
    public Text waveNumText;

    public VisualEffect fireworks;

    [SerializeField] Transform spawnLocation;
    [SerializeField] int _winWaveNum;
   
    public Transform finishLine;

    float previousTimeScale;

    private void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        playerGoldText.text = playerTotalGold.ToString();
        playerLivesText.text = playerTotalLives.ToString();
        waveNumText.text = "Current Wave: " + _waveNumber.ToString();
    }

    public void StartWave()
    {
        //start the spawning coroutine when UI button pressed
        Debug.Log("<color=green>Wave Incoming! - TODO: Display on screen</color>");

        startWaveBTN.GetComponent<Button>().interactable = false;
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        isSpawning = true; 
        enemiesSpawned = 0; //make sure the enemy spawn count resets back to 0 before new wave spawn

        foreach (EnemySO enemy in _enemies)
        {
            if (enemy.WaveNumber == _waveNumber)
            {
                for (int i = 0; i < enemy.SpawnCount; i++)
                {
                    //sets enemies stats from their scripable object
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
            //turns off start button while enemies are spawning
            //updates wave num UI
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
        //updates gold UI
        playerTotalGold += amount;
        Instance.playerGoldText.text = playerTotalGold.ToString();
    }
    public void EnemyDiedOrRemoved()
    {
        //checks if the game and/or wave is over
        enemiesSpawned--;
        CheckForWaveOver();
        CheckForGameOver();
    }

    public void RemoveEnemyFromTowers(GameObject enemy)
    {
        //removes dead enemies from all tower's enemy lists
        foreach (GameObject tower in builtTowers)
        {
            var towerController = tower.GetComponent<TowerController>();
            if (towerController._Enemies.Count > 0 && towerController._Enemies.Contains(enemy))
            {

                towerController._Enemies.Remove(enemy);
                Debug.Log("Enemy Removed");
                if (towerController._Enemies.Count == 0 && towerController.flameComponent != null)
                {
                    //for flame towers
                    towerController.flameComponent.flameEffectIsPlaying = false;
                    towerController.flameComponent.FlameController();
                }
            }
            else
            {
                Debug.Log("Enemy not in _Enemies list!");
            }
                     
        }
    }

    void GameOverLose()
    {
        CheckTimeScale();
        playerHUD.SetActive(false);
        loseCanvas.SetActive(true);
        Debug.Log("<color=blue>Game Over - TODO: Display on screen</color>");
    }

    void GameOverWin()
    {
        CheckTimeScale();
        fireworks.Play();
        playerHUD.SetActive(false);
        winCanvas.SetActive(true);
        Debug.Log("<color=blue>You Win! - YODO: Display on screen</color>");
    }

    public void RestartGame()
    {
        playerTotalGold = 1000;
        playerTotalLives = 20;
        _waveNumber = 1;
        UpdateUI();
    }

    private void CheckTimeScale()
    {
        if (isFastForwarding)
        {
            isFastForwarding = false;
            Time.timeScale = 1;
        }
    }

    public void PauseAndUnPauseGame()
    {
        if(Time.timeScale != 0)
        {
            previousTimeScale = Time.timeScale;
        }

        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = previousTimeScale;
            isPaused = false;
        }
    }

    public void ChangeGameSpeed(float speed)
    {
        //Change game speed with UI buttons
        Time.timeScale = speed;
        if(speed > 1)
        {
            isFastForwarding = true;
            isPaused = false;
        }
        else
        {
            isFastForwarding = false;
            isPaused = false;
        }
    }
}
