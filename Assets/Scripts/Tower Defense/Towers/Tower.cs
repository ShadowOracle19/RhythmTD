using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InstrumentType
{
    Drums, Guitar, Vocal, Piano
}

public enum BuffType
{
    Burn, Multi, Shield, Normal
}

public enum TowerState
{
    Default, Recording, Repeating
}

public class Tower : MonoBehaviour
{
    public float towerAudioVolumeIncrement = 0.05f;

    public TowerTypeCreator towerInfo;

    public TowerAttackPattern currentAttackPattern;

    public GameObject projectile;
    /* 
    Hi Lucy. 
    I made & added some more projectiles so that we could spawn ones with different sprites without having to use GetComponent. 
    Idk how efficient this is so if we gotta change it later that's cool. 
    - Em
    */
    public GameObject nextProjectile;
    
    
    public Tile connectedTile;

    public Collider[] colliders;
    //public RaycastHit[] colliders;

    public int beat;

    [Header("Tower Stats")]
    private int currentHealth = 0;
    public int currentDamage;
    public int tempDamageHolder;
    public int towerRange;

    [Header("Tower Empower Indicator")]
    public bool towerHover = false;
    public GameObject beatIndicator;
    public GameObject beatCircle;
    public bool towerAboutToFire = false;

    [Header("Shield")]
    public GameObject shieldEffect;
    public bool isShielded = false;

    [Header("Record Buff Input")]
    public TowerState currentState = TowerState.Default;
    public List<BuffType> recordedBuffs = new List<BuffType>();
    public bool isInputtingBuffs = false;
    public int beatRecordingStarted = 1;
    public int buffTimer = 0;
    int buffTimerMax = 2;
    public int buffIndex = 0;
    public int buffCountMeasure = 0;
    public int buffBeatCount = 1;

    [Header("Record State Sprites")]
    public GameObject recordingStatus;//RECORDING STATUS CODE
    public Sprite recordingSpr;//RECORDING STATUS CODE
    public List<Sprite> repeatSprites = new List<Sprite>();
    private int repeatSpritesIndex = 0;

    //[Header("Powered UP Tower")]
    //public bool isPoweredUp = false;
    //public GameObject nonPoweredIcon;
    //public GameObject poweredIcon;

    [Header("Projectile Sprites")]
    public Sprite defaultAttackSprite;
    public Sprite buffDefaultAttackSprite;
    public Sprite upgradeAttackSprite01;
    public Sprite upgradeAttackSprite02;
    public Sprite upgradeAttackSprite03;

    public Sprite flameAttackSprite;

    [Header("Tile Interactions")]
    public bool ChargedUp = false;

    [Header("Tower Upgrade")]
    public bool towerUpgradeUnlocked = false;
    public bool upgradePurchased = false;
    //upgrade 1 damage boost
    //public int upgradeCost1;
    public bool upgradeOneActive = false;

    ////upgrade 2 multiple projectile
    //public int upgradeCost2;
    public bool upgradeTwoActive = false;

    ////upgrade 3 burning
    //public int upgradeCost3;
    public bool upgradeThreeActive = false;

    ////upgrade 4 range
    //public int upgradeCost4;
    public bool upgradeFourActive = false;

    [Header("Upgrade Modifiers")]
    public bool feelingItNow = false;
    public bool synthBuff = false;

    [Header("SFX")]
    public AudioClip towerAttackSfx;
    public AudioClip towerHurtSfx;
    public AudioClip towerDeathSfx;
    public AudioClip towerUpgradeSfx;

    [Header("PFX")]
    [SerializeField] private ParticleSystem aoeAttackParticles;
    private ParticleSystem aoeAttackParticlesInstance;
    public Color aoeAttackColour;

    [SerializeField] private ParticleSystem shieldDestructionParticles;
    private ParticleSystem shieldDestructionParticlesInstance;

    [SerializeField] private ParticleSystem clashParticles;
    private ParticleSystem clashParticlesInstance;

    [SerializeField] private ParticleSystem burningParticles;
    private ParticleSystem burningParticlesInstance;



    public virtual void Start()
    {
        currentAttackPattern = towerInfo.attackPattern;
        currentHealth = towerInfo.towerHealth;
        currentDamage = towerInfo.damage;
        //if(isPoweredUp && towerInfo.type == InstrumentType.Piano)
        //{
        //    currentHealth = currentHealth * 2;
        //}

        beat = 1;

        currentState = TowerState.Default;

        towerRange = towerInfo.range;

        recordingStatus.SetActive(false); //RECORDING STATUS CODE

        nextProjectile = projectile; // Set default projectile type
    }

    public virtual void Update()
    { 
        
        if(FeverSystem.Instance.feverModeActive)
            isShielded = true;

        shieldEffect.SetActive(isShielded);

        //if (isPoweredUp) 
        //{ 
        //    poweredIcon.SetActive(true);
        //    nonPoweredIcon.SetActive(false);

        //    if(towerInfo.type == InstrumentType.Guitar)
        //    {
        //        towerRange = 6;
        //    }

        //}

        towerEffectVisual();

    }



    public void towerEffectVisual()
    {
        if (towerHover && towerAboutToFire)
        {
            beatIndicator.SetActive(true);
            beatCircle.SetActive(true);
        }
        else
        {
            beatIndicator.SetActive(false);
            beatCircle.SetActive(false);
        }
    }

    public virtual void CreateBullet(int damage, Vector3 position)
    {
        int tempRange = towerRange;

        //instatiate bullet
        GameObject bullet = Instantiate(nextProjectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);

        
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies);

        ConductorV2.instance.projectileEvent.Add(bullet.GetComponent<Projectile>().trigger);
        //towerUpgradeUnlocked = false;
        feelingItNow = false;
        synthBuff = false;

    }


    public virtual void Fire() //default fire
    {
        //play attack sound
        SoundEffectsManager.instance.PlaySound(towerAttackSfx, this.gameObject.transform, 1.0f);
        
        //if feeling it now is active
        if(feelingItNow)
        {
            //nextProjectile.GetComponent<Projectile>().spriteRenderer.sprite = buffDefaultAttackSprite;

            tempDamageHolder = currentDamage;
            currentDamage = currentDamage * 2;
        }
        //feeling it now inactive
        else if (upgradePurchased)
        {
            currentDamage = tempDamageHolder;
            //nextProjectile.GetComponent<Projectile>().spriteRenderer.sprite = upgradeAttackSprite01;
        }
        else
        {
            currentDamage = tempDamageHolder;
            nextProjectile = projectile;
        }

        
        
    }

    public virtual void Fire(float yPos) //Fire on specific ypos mainly for viola
    {

        //play attack sound
        SoundEffectsManager.instance.PlaySound(towerAttackSfx, this.gameObject.transform, 1.0f);

        int damage = currentDamage;

        nextProjectile = projectile;

        //if (ChargedUp || FeverSystem.Instance.feverModeActive)
        //{
        //    damage = damage * 5;

        //    nextProjectile = buffProjectile;
        //}
        //else if (burningBullet)
        //{
        //    nextProjectile = buffProjectile;
        //}

        

        
        //if(isPoweredUp && towerInfo.type == InstrumentType.Bass)
        //{
        //    CreateBullet(damage, burningBullet, false, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y, gameObject.transform.position.z + -yPos));
            
        //}

        towerUpgradeUnlocked = false;
    } 

    public void ExtraFire() //buff fire
    {
        
        if(towerInfo.isAOETower)
        {
            AOE(currentDamage);
            return;
        }

        CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1));

        CreateBullet(currentDamage, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1));

    }

    public void PlaceCharge(int chargeValue, Tower connectedTower)
    {
        colliders = Physics.OverlapSphere(transform.position, towerRange, LayerMask.GetMask("Stage"));

        int rand = Random.Range(0, colliders.Length - 1);

        GameObject charge = Instantiate(nextProjectile, transform.position, transform.rotation, CombatManager.Instance.chargesParent);
        charge.GetComponent<Charges>().initalizeCharge(chargeValue,  new Vector3(colliders[rand].transform.position.x, 0.5f, colliders[rand].transform.position.z), connectedTower, true);
        feelingItNow = false;
        synthBuff = false;
    }

    public virtual void AOE(int damage)
    {
        int tempRange = towerRange;
        //if(towerUpgrades)
        //{
        //    if(rangeUpgrade)
        //    {
        //        tempRange *= 2;

        //    }
        //    if(damageBoostUpgrade)
        //    {
        //        damage *= 2;
        //    }

        //    if(multiShotUpgrade)
        //    {
        //        ExtraFire();
        //    }

        //}

        colliders = Physics.OverlapSphere(transform.position, tempRange);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("StageTile"))
            {
                //item.transform.GetComponent<Tile>().Pulse(Color.blue);

                //Depending on the upgrade, change the sprite
                if (upgradePurchased)
                {
                    SpawnParticles(item.transform, flameAttackSprite, aoeAttackParticles, aoeAttackParticlesInstance, false, true);
                    //Debug.Log("[Tower.cs] BurnUpgrade");
                }
                else
                {
                    SpawnParticles(item.transform, defaultAttackSprite, aoeAttackParticles, aoeAttackParticlesInstance, false, false);
                    //Debug.LogWarning("[Tower.cs] AOE sprite display broke :(");
                }
            }
            else if (item.transform.CompareTag("Enemy"))
            {
                item.transform.GetComponent<Enemy>().Damage(damage);

                if(upgradeOneActive)
                {
                    item.transform.GetComponent<Enemy>().isStunned = true;
                }
                //if(towerUpgrades && burningUpgrade)
                //{
                //    item.transform.GetComponent<Enemy>().burnt = true;
                //    item.transform.GetComponent<Enemy>().burnDamage += 2;
                //}
            }
        }
        colliders = null;
        //towerUpgradeUnlocked = false;
        feelingItNow = false;
        synthBuff = false;
    }

    public void RemoveTower()
    {
        switch (towerInfo.type)
        {
            case InstrumentType.Drums:
                ConductorV2.instance.drums.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.drums.volume = Mathf.Clamp(ConductorV2.instance.drums.volume, 0, 0.5f);
                break;

            case InstrumentType.Guitar:
                ConductorV2.instance.guitarH.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.guitarM.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.guitarH.volume = Mathf.Clamp(ConductorV2.instance.guitarH.volume, 0, 0.5f);
                ConductorV2.instance.guitarM.volume = Mathf.Clamp(ConductorV2.instance.guitarM.volume, 0, 0.5f);
                break;

            case InstrumentType.Vocal:
                ConductorV2.instance.bass.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.bass.volume = Mathf.Clamp(ConductorV2.instance.bass.volume, 0, 0.5f);
                break;

            case InstrumentType.Piano:
                ConductorV2.instance.piano.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.piano.volume = Mathf.Clamp(ConductorV2.instance.piano.volume, 0, 0.5f);
                break;

            default:
                break;
        }
        //TowerManager.Instance.towerList.Remove(this);
        connectedTile.placedTower = null;
        Destroy(gameObject);
    }

    public virtual void Damage(int damage)
    {
        //play hurt sound
        SoundEffectsManager.instance.PlaySound(towerHurtSfx, this.gameObject.transform, 1.0f);

        if(isShielded)
        {
            SpawnParticles(this.transform, defaultAttackSprite, shieldDestructionParticles, shieldDestructionParticlesInstance, true, false);

            isShielded = false;
            return;
        }
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            //play death sound
            SoundEffectsManager.instance.PlaySound(towerDeathSfx, this.gameObject.transform, 1.0f);

            clashParticlesInstance = Instantiate(clashParticles, this.transform.position, Quaternion.identity); // Create instance of the tower clash particle effect
            RemoveTower();
        }
    }

    public void ActivateBuff(BuffType buffType)
    {
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerBuffSequence) //post buff sequence in tutorial
        {
            if (TutorialManager.Instance.index == 11)
            {
                CursorTD.Instance.buffCounter += 1;
            }
                
            if(CursorTD.Instance.buffCounter == 4)
            {
                // Make sure index is set to whichever text says "Press Z, X, C, or V when the ring touches the center circle"
                if (TutorialManager.Instance.index == 11)
                {
                    TutorialManager.Instance.LoadNextTutorialDialogue();
                }

                CursorTD.Instance.towerBuffSequence = false;
                CombatManager.Instance.healthBar.SetActive(true);
                CombatManager.Instance.feverBar.SetActive(true);
                CombatManager.Instance.combo.SetActive(true);
                //CombatManager.Instance.controls.SetActive(true);

                CursorTD.Instance.feverModeSequence = true;
                FeverSystem.Instance.feverBarNum = 50;

                Spawner.Instance.ForceEnemySpawn(-0.5f, EnemyType.Walker);
                CursorTD.Instance.buffCounter = 0;
            }
        }

        RecordBuff(buffType);

        PlayBuffs(buffType);
    }

    public void PlayBuffs(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Multi://Multi Buff
                ExtraFire();
                break;

            case BuffType.Burn://Burn Buff
                //burningBullet = true;
                break;

            case BuffType.Shield: //Shield Buff
                isShielded = true;
                break;

            case BuffType.Normal:
                towerUpgradeUnlocked = true;
                break;

            default:
                break;
        }
    }

    public void RecordBuff(BuffType buff) //records the buff but if a 5th buff is pressed it will remove the first buff on the list
    {
        currentState = TowerState.Recording;

        recordingStatus.SetActive(true); //RECORDING STATUS CODE
        recordingStatus.GetComponent<SpriteRenderer>().sprite = recordingSpr;//RECORDING STATUS CODE

        isInputtingBuffs = true;

        recordedBuffs.Add(buff);

        beatRecordingStarted = ConductorV2.instance.beatTrack;
        buffTimer = 0;

        buffIndex = 0;

        if (recordedBuffs.Count > 4)
        {
            recordedBuffs.RemoveAt(0);
        }
        
    }

    public void BuffPlayback(int _beat)
    {
        if (currentState == TowerState.Default)
        {
            return;
        }
        else if (currentState == TowerState.Recording)
        {
            buffTimer += 1;
            if(buffTimer == buffTimerMax)
            {
                currentState = TowerState.Repeating;
                buffTimer = 0;
                isInputtingBuffs = false;

                repeatSpritesIndex = 0; //RECORDING STATUS CODE
                recordingStatus.GetComponent<SpriteRenderer>().sprite = repeatSprites[repeatSpritesIndex]; //RECORDING STATUS CODE
            }
            return;
        }
        else if(currentState == TowerState.Repeating)
        {
            if(buffIndex > recordedBuffs.Count - 1)
            {
                //When no buff is activated
            }
            else
            {
                PlayBuffs(recordedBuffs[buffIndex]);
            }

            buffIndex += 1;

            buffBeatCount += 1;

            if(buffBeatCount == 4)
            {
                buffBeatCount = 0;
                buffCountMeasure += 1;

                repeatSpritesIndex += 1; //RECORDING STATUS CODE
                if (repeatSpritesIndex <= (repeatSprites.Count-1))
                {
                    recordingStatus.GetComponent<SpriteRenderer>().sprite = repeatSprites[repeatSpritesIndex];//RECORDING STATUS CODE
                }

                if(buffCountMeasure == 4)
                {
                    buffCountMeasure = 0;
                    recordedBuffs.Clear();
                    currentState = TowerState.Default;

                    repeatSpritesIndex = 0; //RECORDING STATUS CODE
                    recordingStatus.SetActive(false); //RECORDING STATUS CODE
                }
            }

            if (buffIndex >= 4)
            {
                buffIndex = 0;

            }
        }

    }

    public void SpawnParticles(Transform tileTransform, Sprite projectileSprite, ParticleSystem pfxSource, ParticleSystem pfxInstance, bool shielded, bool burning)
    {
        if(towerUpgradeUnlocked)
        {
            
            if (upgradeOneActive)
            {
                // Set sprite
                burningParticlesInstance = Instantiate(burningParticles, tileTransform.position, Quaternion.identity);
            }
        }
        if (!shielded)
        {
            // Set sprite
            var pfxTexture = pfxSource.textureSheetAnimation;
            pfxTexture.SetSprite(0, projectileSprite);
        }

        
        
        // Create instance of the particle effect
        pfxInstance = Instantiate(pfxSource, tileTransform.position, Quaternion.identity);
    }


}
