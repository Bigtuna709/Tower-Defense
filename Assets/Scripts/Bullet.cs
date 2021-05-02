using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    Transform target;

    [HideInInspector]
    public int bulletDamage;
    public float rocketDamageArea;
    public int rocketAreaDamage;

    public float speed;
    public void LookForTarget(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distancePerFrame, Space.World);
        transform.LookAt(target);

        if(target == null)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().AddDamage(bulletDamage);

            CheckforEnemiesInBlastRadius();

            gameObject.SetActive(false);
        }
    }

    void CheckforEnemiesInBlastRadius()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, rocketDamageArea);
        foreach(Collider enemy in enemies)
        {
            if(enemy.GetComponent<EnemyController>())
            {
                enemy.GetComponent<EnemyController>().AddDamage(rocketAreaDamage);
            }
        }
    }
}
