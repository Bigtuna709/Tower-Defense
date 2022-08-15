using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class FlameComponent : MonoBehaviour, IFireable
{
    [SerializeField] private GameObject flameParticleGO;
    [SerializeField] private ParticleSystem flameParticle;
    [SerializeField] private VisualEffect flameEffect;
    [SerializeField] public bool flameEffectIsPlaying = false;
    [SerializeField] private TowerType towerType;

    public void FireWeapon(Transform target)
    {
        var tower = BuildManager.Instance._towers.FirstOrDefault(x => x.TowerType == towerType);
        if (tower != null)
        {
            if (tower.TowerFlame != null)
            {
                flameEffectIsPlaying = true;
                flameParticle = tower.TowerFlame;
                FlameController();
                var flame = flameParticle.GetComponentInChildren<Flamethrower>();
                if (flame != null)
                {
                    Debug.Log("Changed Dmg " + tower.TowerDamage);
                    flame.flameDamage = tower.TowerDamage;
                }
            }
        }
        else
            return;
    }
    public void FlameController()
    {
        if (flameEffect == null)
            return;
        
        if (flameEffectIsPlaying)
        {
            flameEffect.Play();
            flameParticleGO.SetActive(true);
            Debug.Log("Played part");
        }
        else
        {
            flameEffect.Stop();
            Debug.Log("Stopped part");
        }
    }
}
