﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
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
            GameManager.Instance.EnemyDiedOrRemoved();
        }   
    }

    public void AddDamage(int damage)
    {
        totalEnemyHealth -= damage;
        if(totalEnemyHealth <= 0)
        {
            if (this.gameObject != null)
            {
                Destroy(this.gameObject);
                GameManager.Instance.ShowGoldChange(enemyReward);
                GameManager.Instance.EnemyDiedOrRemoved();
            }
        }
    }
    public IEnumerator AddDamageOverTime(int damage)
    {
        totalEnemyHealth -= damage;
        if (totalEnemyHealth <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.ShowGoldChange(enemyReward);
            GameManager.Instance.EnemyDiedOrRemoved();
        }
        yield return new WaitForSeconds(1f);

    }
}
