using System.Collections;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public int flameDamage;
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().AddDamage(flameDamage);            
        }
    }
}
