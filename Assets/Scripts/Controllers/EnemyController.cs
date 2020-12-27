﻿using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;

    public int totalEnemyHealth;
    public int enemyReward;
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void OnEnable()
    {
        Vector3 finishLine = GameObject.Find("FinishLine").transform.position;
        _navMeshAgent.SetDestination(finishLine);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            Destroy(gameObject);
            GameManager.Instance.playerTotalLives--;
            GameManager.Instance.playerLivesText.text = GameManager.Instance.playerTotalLives.ToString();
            Debug.Log("<color=cyan>Enemy reached the end!</color>");

            if (GameManager.Instance.playerTotalLives == 0)
            {
                GameManager.Instance.GameOver();
            }
        }   
    }

    public void AddDamage(int damage)
    {
        totalEnemyHealth -= damage;
        if(totalEnemyHealth <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.playerTotalGold += enemyReward;
            GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();
        }
    }
}
