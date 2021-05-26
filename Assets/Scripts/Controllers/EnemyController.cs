using UnityEngine;
using UnityEngine.AI;
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
            //when the enemy reaches the end without dying
            Destroy(gameObject);
            GameManager.Instance.playerTotalLives--;
            GameManager.Instance.playerLivesText.text = GameManager.Instance.playerTotalLives.ToString();
            Debug.Log("<color=cyan>Enemy reached the end!</color>");
            GameManager.Instance.EnemyDiedOrRemoved();
        }   
    }
 
    public void AddDamage(int damage)
    { //adds damage to the enemy and checks if they die
        totalEnemyHealth -= damage;
        if(totalEnemyHealth <= 0)
        {
            if (gameObject != null)
            {
                GameManager.Instance.RemoveEnemyFromTowers(gameObject);
                GameManager.Instance.ShowGoldChange(enemyReward);
                GameManager.Instance.EnemyDiedOrRemoved();
                gameObject.SetActive(false);
            }
        }
    }
}
