using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InstrumentType
{
    Drums, Guitar, Bass, Piano
}

public enum BuffType
{
    Fayruz, Sonu, Niimi, Normal
}

public enum TowerState
{
    Default, Recording, Repeating
}

public class Tower : MonoBehaviour
{
    public AudioSource towerAttackSFX;
    public float towerAudioVolumeIncrement = 0.05f;

    public TowerTypeCreator towerInfo;

    public bool rotateStarted = false;

    public Transform firePoint;
    public GameObject projectile;

    private RaycastHit[] colliders;

    public int currentHealth = 0;

    public Tile connectedTile;
    public int currentDamage;
    public int tempDamageHolder;

    public bool burningBullet = false;
    public bool increaseBulletDamage = false;

    public int towerRange;

    [Header("Tower Empower Indicator")]
    public bool towerHover = false;
    public GameObject beatIndicator;
    public GameObject beatCircle;
    public bool towerAboutToFire = false;

    [Header("Shield")]
    public GameObject shieldEffect;
    public bool isShielded = false;

    public int beat;
    public bool everyOtherBeat = false;

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

    [Header("Powered UP Tower")]
    public bool isPoweredUp = false;
    public GameObject nonPoweredIcon;
    public GameObject poweredIcon;

    // Projectile Sprites
    private Sprite nextAttackSprite;
    
    public Sprite defaultAttackSprite;
    public Sprite increasedAttackSprite;
    public Sprite multiAttackSprite;
    public Sprite flameAttackSprite;
    
    // Attack PFX
    [SerializeField] private ParticleSystem aoeAttackParticles;
    private ParticleSystem aoeAttackParticlesInstance;
    public Color aoeAttackColour;

    [SerializeField] private ParticleSystem shieldDestructionParticles;
    private ParticleSystem shieldDestructionParticlesInstance;

    [SerializeField] private ParticleSystem clashParticles;
    private ParticleSystem clashParticlesInstance;

    [SerializeField] private ParticleSystem burningParticles;
    private ParticleSystem burningParticlesInstance;

    private void Start()
    {
        currentHealth = towerInfo.towerHealth;

        if(isPoweredUp && towerInfo.type == InstrumentType.Piano)
        {
            currentHealth = currentHealth * 2;
        }

        beat = 1;
        currentState = TowerState.Default;
        towerRange = towerInfo.range;
    }

    private void Update()
    { 
        
        if(FeverSystem.Instance.feverModeActive)
            isShielded = true;

        shieldEffect.SetActive(isShielded);

        if (isPoweredUp) 
        { 
            poweredIcon.SetActive(true);
            nonPoweredIcon.SetActive(false);

            if(towerInfo.type == InstrumentType.Guitar)
            {
                towerRange = 6;
            }

        }

        towerEffectVisual();

    }

    public void towerEffectVisual()
    {
        float beatDuration = ConductorV2.instance.beatDuration;
        if (towerHover && towerAboutToFire)
        {
            beatIndicator.SetActive(true);
            beatCircle.SetActive(true);
            //beatIndicator.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one * 0.75f, beatDuration);

            //if (ConductorV2.instance.beatDuration < 0.2)
            //{
            //    beatIndicator.transform.localScale = Vector3.one * 1.5f;
            //}
        }
        else
        {
            beatIndicator.SetActive(false);
            beatCircle.SetActive(false);
        }
    }


    public void Fire() //default fire
    {
        if (!rotateStarted) return;

        //Audio SFX
        towerAttackSFX.Play();
        
        
        currentDamage = tempDamageHolder;

        nextAttackSprite = defaultAttackSprite;

        if (increaseBulletDamage)
        {
            currentDamage = currentDamage * 5;

            nextAttackSprite = increasedAttackSprite;
        }
        else if (burningBullet)
        {
            nextAttackSprite = flameAttackSprite;
        }

        if (towerInfo.isAOETower)
        {
            AOE();
            return;
        }

        GameObject bullet = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, burningBullet);
        

        if(increaseBulletDamage || FeverSystem.Instance.feverModeActive)
            bullet.GetComponent<Projectile>().spriteRenderer.sprite = increasedAttackSprite;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);

        //empowered guitar function
        if (isPoweredUp && towerInfo.type == InstrumentType.Guitar)
        {
            GameObject _bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x + 1f, gameObject.transform.position.y), gameObject.transform.rotation, GameManager.Instance.projectileParent);
            _bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, burningBullet);


            if (increaseBulletDamage || FeverSystem.Instance.feverModeActive)
                _bullet.GetComponent<SpriteRenderer>().sprite = increasedAttackSprite;

            ConductorV2.instance.triggerEvent.Add(_bullet.GetComponent<Projectile>().trigger);
        }

        burningBullet = false;
        increaseBulletDamage = false;
        
    }

    public void Fire(float yPos) //Fire on specific ypos mainly for viola
    {
        if (!rotateStarted) return;

        //Audio SFX
        towerAttackSFX.Play();

        
        currentDamage = tempDamageHolder;

        if (increaseBulletDamage)
        {
            currentDamage = currentDamage * 5;
        }

        GameObject bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x + 1.2f, gameObject.transform.position.y + yPos), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, burningBullet);


        if (increaseBulletDamage || FeverSystem.Instance.feverModeActive)
            bullet.GetComponent<Projectile>().spriteRenderer.sprite = increasedAttackSprite;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);

        if(isPoweredUp && towerInfo.type == InstrumentType.Bass)
        {
            GameObject _bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x + 1.2f, gameObject.transform.position.y + -yPos), gameObject.transform.rotation, GameManager.Instance.projectileParent);
            _bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, burningBullet);


            if (increaseBulletDamage || FeverSystem.Instance.feverModeActive)
                _bullet.GetComponent<Projectile>().spriteRenderer.sprite = increasedAttackSprite;

            ConductorV2.instance.triggerEvent.Add(_bullet.GetComponent<Projectile>().trigger);
        }

        burningBullet = false;
        increaseBulletDamage = false;

    } 

    public void ExtraFire() //buff fire
    {
        if (!rotateStarted) return;
        
        if(towerInfo.isAOETower)
        {
            AOE();
            return;
        }

        GameObject bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y + 1), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, false);

        bullet.GetComponent<Projectile>().spriteRenderer.sprite = multiAttackSprite;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);

        GameObject bullet2 = Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y + 1), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet2.GetComponent<Projectile>().InitializeProjectile(towerRange, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, false);

        bullet2.GetComponent<Projectile>().spriteRenderer.sprite = multiAttackSprite;

        ConductorV2.instance.triggerEvent.Add(bullet2.GetComponent<Projectile>().trigger);
        
    }

    public void AOE()
    {
        colliders = Physics.BoxCastAll(transform.position, Vector2.one * towerRange * 2, Vector3.zero, Quaternion.identity);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("StageTile"))
            {
                //item.transform.GetComponent<Tile>().Pulse(Color.blue);

                SpawnParticles(item.transform, multiAttackSprite, aoeAttackParticles, aoeAttackParticlesInstance, false, burningBullet);
            }
            else if (item.transform.CompareTag("Enemy"))
            {
                if (isPoweredUp && item.transform.GetComponent<Enemy>().isStunned == false)//empowered drum effect
                {
                    item.transform.GetComponent<Enemy>().isStunned = true;
                }

                item.transform.GetComponent<Enemy>().Damage(currentDamage);
            }
        }
        colliders = null;
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

            case InstrumentType.Bass:
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
        TowerManager.Instance.towers.Remove(gameObject);
        connectedTile.placedTower = null;
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        if(isShielded)
        {
            SpawnParticles(this.transform, defaultAttackSprite, shieldDestructionParticles, shieldDestructionParticlesInstance, true, false);

            isShielded = false;
            return;
        }
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
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
                CombatManager.Instance.controls.SetActive(true);

                CursorTD.Instance.feverModeSequence = true;
                FeverSystem.Instance.feverBarNum = 50;

                EnemySpawner.Instance.ForceEnemySpawn(CursorTD.Instance.gameObject.transform.position.y, EnemyType.Walker);
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
            case BuffType.Sonu://Sonu's Buff
                ExtraFire();
                break;
            case BuffType.Fayruz://Fayruz's Buff
                burningBullet = true;
                break;
            case BuffType.Niimi: //Niimi's Buff
                isShielded = true;

                break;
            case BuffType.Normal:
                increaseBulletDamage = true;
                break;
            default:
                break;
        }
    }

    public void RecordBuff(BuffType buff) //records the buff but if a 5th buff is pressed it will remove the first buff on the list
    {
        currentState = TowerState.Recording;

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
            }
            return;
        }
        else if(currentState == TowerState.Repeating)
        {
            if(buffIndex > recordedBuffs.Count - 1)
            {

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
                if(buffCountMeasure == 4)
                {
                    buffCountMeasure = 0;
                    recordedBuffs.Clear();
                    currentState = TowerState.Default;
                }
            }

            if (buffIndex >= 4)
            {
                buffIndex = 0;
            }
        }

    }

    private void SpawnParticles(Transform tileTransform, Sprite projectileSprite, ParticleSystem pfxSource, ParticleSystem pfxInstance, bool shielded, bool burning)
    {
        if (!shielded)
        {
            // Set sprite
            var pfxTexture = pfxSource.textureSheetAnimation;
            pfxTexture.SetSprite(0, projectileSprite);
        }

        if (burning == true)
        {
            // Set sprite
            burningParticlesInstance = Instantiate(burningParticles, tileTransform.position, Quaternion.identity); 
        }
        
        // Create instance of the particle effect
        pfxInstance = Instantiate(pfxSource, tileTransform.position, Quaternion.identity);
    }


}
