using UnityEngine;

public class Bullet : MonoBehaviour
{

    Transform target;

    public float speed;
    public int bulletDamage;
    public void LookForTarget(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distancePerFrame, Space.World);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().AddDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

}
