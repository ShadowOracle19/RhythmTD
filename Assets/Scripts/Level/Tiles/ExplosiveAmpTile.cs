using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmpTile : Tile
{
    public Collider[] colliders;


    [Header("Attack PFX")]
    [SerializeField] private ParticleSystem aoeAttackParticles;
    private ParticleSystem aoeAttackParticlesInstance;
    public Sprite defaultAttackSprite;


    private void OnTriggerEnter(Collider other)
    {
        //projectile goes over tile
        if (other.gameObject.CompareTag("Projectile_Tower"))
        {
            AOE(other.gameObject.GetComponent<Projectile>().damage);
        }

    }

    public virtual void AOE(int damage)
    {
        int tempRange = 2;
        
        colliders = Physics.OverlapSphere(transform.position, tempRange);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("StageTile"))
            {
                SpawnParticles(item.transform, defaultAttackSprite, aoeAttackParticles, aoeAttackParticlesInstance);
                
            }
            else if (item.transform.CompareTag("Enemy"))
            {
                item.transform.GetComponent<Enemy>().Damage(damage);
            }
        }
        colliders = null;
    }

    private void SpawnParticles(Transform tileTransform, Sprite projectileSprite, ParticleSystem pfxSource, ParticleSystem pfxInstance)
    {
        var pfxTexture = pfxSource.textureSheetAnimation;
        pfxTexture.SetSprite(0, projectileSprite);
        // Create instance of the particle effect
        pfxInstance = Instantiate(pfxSource, tileTransform.position, Quaternion.identity);
    }
}
