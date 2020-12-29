using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class GameManager : Singleton<GameManager>
{
    public List<EnemySO> _enemies = new List<EnemySO>();
    public EnemyController enemyController;

    public int playerTotalGold;
    public int playerTotalLives;
    public int _waveNumber;

    public GameObject startWaveBTN;
    public GameObject playerHUD;
    public GameObject buildModePanel;

    public Text playerLivesText;
    public Text playerGoldText;
    public Text waveNumText;

    public Transform finishLine;
    [SerializeField] Transform spawnLocation;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        playerTotalGold = 60;
        playerTotalLives = 20;
        playerGoldText.text = playerTotalGold.ToString();
        playerLivesText.text = playerTotalLives.ToString();
        waveNumText.text = "Wave Number: " + _waveNumber;
    }

    public void StartWave()
    {
        Debug.Log("<color=green>Wave Incoming! - TODO: Display on screen</color>");
        startWaveBTN.GetComponent<Button>().interactable = false;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        waveNumText.text = "Wave Number: " + _waveNumber.ToString();

        foreach (EnemySO enemy in _enemies)
        {
            if (enemy.WaveNumber == _waveNumber)
            {
                for (int i = 0; i < enemy.SpawnCount; i++)
                {
                    yield return new WaitForSeconds(enemy.SpawnDelay);
                    var instance = GameObject.Instantiate(enemy.EnemyPrefab, spawnLocation.position, transform.rotation);
                    instance.GetComponent<NavMeshAgent>().speed = enemy.EnemySpeed;
                    instance.GetComponent<EnemyController>().totalEnemyHealth = enemy.EnemyHealth;
                    instance.GetComponent<EnemyController>().enemyReward = enemy.GoldReward;
                }
            }
        }

        _waveNumber++;
        startWaveBTN.GetComponent<Button>().interactable = true;
    }

    public void GameOver()
    {
        Debug.Log("<color=blue>Game Over - TODO: Display on screen</color>");

    }

}
