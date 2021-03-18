using UnityEngine;

public class Bullet : MonoBehaviour
{

    Transform target;

    [HideInInspector]
    public int bulletDamage;

    public float speed;
    public void LookForTarget(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if(target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distancePerFrame, Space.World);
        transform.LookAt(target);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().AddDamage(bulletDamage);
            gameObject.SetActive(false);
        }
    }

}
