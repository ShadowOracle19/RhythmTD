using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InstrumentType
{
    Flats, Trill, Major, Chromatic, Forte, Poco, Legato, Allegro, Tower9, Tower10, Tower11, Tower12
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
    // VARIABLES //
    #region Variables
    public TowerTypeCreator towerInfo;
    public GameObject projectile;

    public bool enemyInRange = false;

    [Header("Tower Attack & Input")]
    public float songProgress = 0.0f; // progress of current song expressed in time
    public int inputIndex; // the index of the closest input timing
    public int attackIndex; //current upcoming attack index
    public int prevAttackIndex; //previous attack index
    public float measureLength = 0.0f; // length of 1 measure expressed in time
    public int measureCycleCount = 0;
    public int attackCycleCount = 0;
    public float inputTargetTime = 0.0f; // input timing in the song
    public float attackTargetTime = 0.0f;
    public GameObject indicatorPrefab;
    public List<GameObject> indicators = new List<GameObject>();

    public Projectile lastBulletFired;
    public bool isBuffed = false;
    
    [Header("Tower Empower Indicator")]
    public GameObject inputPrompt;
    public bool towerAboutToFire = false;
    public bool towerHover = false;

    [Header("Shield")]
    public GameObject shieldEffect;
    public bool isShielded = false;

    [Header("Record State Sprites")] //moved recording sprites to game manager
    public GameObject recordingStatus;//RECORDING STATUS CODE
    private int repeatSpritesIndex = 0;

    [Header("Projectile Sprites")]
    public Color projectileColor;
    public Sprite[] projectileSprites;

    [Header("Animation")]
    public Animator m_Animator;
    public string[] animationStates;
    public int[] animationHashes;

    [Header("SFX")]
    public AudioClip towerAttackSfx;
    public AudioClip towerHurtSfx;
    public AudioClip towerDeathSfx;
    public AudioClip towerUpgradeSfx;

    [Header("PFX")]
    public ParticleSystem[] particleEffects;

    [Header ("--------- Don't need to touch ---------")]

    public TowerAttackPattern currentAttackPattern;
    public Tile connectedTile;
    public Collider[] colliders;
    public GameObject nextProjectile;
    public float towerAudioVolumeIncrement = 0.05f;
    public int beat;

    [Header("Tower Stats")]
    private int currentHealth = 0;
    public int towerDamage;
    public float attackPower = 1.0f;
    public float attackPowerBonus = 0.0f;
    public int towerRange;

    [Header("Tile Interactions")]
    public bool ChargedUp = false;

    [Header("Tower Upgrade")]
    public bool upgradePurchased = false;

    public int upgradeIndex = 0; //0 = no upgrade purchased

    [Header("Upgrade Modifiers")]
    public bool feelingItNow = false;
    public bool synthBuff = false;

    [Header("Record Buff Input")]
    private TowerState currentState = TowerState.Default;
    private List<BuffType> recordedBuffs = new List<BuffType>();
    private bool isInputtingBuffs = false;
    private int beatRecordingStarted = 1;
    private int buffTimer = 0;
    int buffTimerMax = 2;
    private int buffIndex = 0;
    private int buffCountMeasure = 0;
    private int buffBeatCount = 1;

    [HideInInspector]
    public int towerNum;
    #endregion

    public virtual void Start()
    {
        currentAttackPattern = towerInfo.attackPattern;
        currentHealth = towerInfo.towerHealth;
        towerDamage = towerInfo.damage;

        beat = 1;

        currentState = TowerState.Default;

        towerRange = towerInfo.range;

        recordingStatus.SetActive(false); //RECORDING STATUS CODE

        nextProjectile = projectile; // Set default projectile type

        //Set Animation BPM
        AnimationManager.instance.SetAnimSpeed(m_Animator, 80);
        GenerateAnimationHashes();

        // TOWER PATTERN //
        //Note: Input & attack indexes are tracked separately because they update at different times with different criteria
        measureLength = ConductorV2.instance.crotchet * 4;

        inputIndex = 0;
        attackIndex = 0;
        prevAttackIndex = towerInfo.inputPatterns[upgradeIndex].noteInputs.Count - 1;

        CalculateInputTimes();
        InstantiateIndicators();

        measureCycleCount = ConductorV2.instance.measureTrack;
        attackCycleCount = ConductorV2.instance.measureTrack;

        inputTargetTime = ((measureLength * measureCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[inputIndex].noteTime);
        attackTargetTime = ((measureLength * measureCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[inputIndex].noteTime);
    }

    public virtual void Update()
    { 
        // TOWER INPUT //
        songProgress = ConductorV2.instance.songPosition;

        // Update input tracking index when song progress exceeds threshold
        if (songProgress > (inputTargetTime + ConductorV2.instance.missBeatThreshold))
        {
            UpdateInputIndex();
            lastBulletFired = null;
        }

        if (songProgress >= attackTargetTime)
        {
            UpdateAttackIndex();
            FireTower(inputIndex);
        }

        //  //
        if(FeverSystem.Instance.feverModeActive)
            isShielded = true;

        shieldEffect.SetActive(isShielded);

        towerEffectVisual();

        //Animation switches
        if (upgradeIndex == 1)//(upgradeOneActive)
        {
            // Set animation
            m_Animator.SetBool("Upgrade1", true);

        }
        else if (upgradeIndex == 2)//(upgradeTwoActive)
        {
            // Set animation
            m_Animator.SetBool("Upgrade2", true);

        }
        else if (upgradeIndex == 3)//(upgradeThreeActive)
        {
            // Set animation
            m_Animator.SetBool("Upgrade3", true);

        }
        else if (upgradeIndex == 4)//(upgradeFourActive)
        {
            // Set animation
            m_Animator.SetBool("Upgrade4", true);

        }  
    }

    #region Tower input
    // Calculates input times as time from measure start
    public void CalculateInputTimes()
    {
        int listIndex = 0;

        foreach (var inputList in towerInfo.inputPatterns)
        {
            foreach (var input in towerInfo.inputPatterns[listIndex].noteInputs)
            {
                input.noteTime = (input.notePosition * measureLength);
            }

            listIndex += 1;
        }
    }
    
    public void InstantiateIndicators()
    {
        foreach (var input in towerInfo.inputPatterns[upgradeIndex].noteInputs)
        {
            GameObject newIndicator = Instantiate(indicatorPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
            newIndicator.GetComponent<InputIndicator>().notePosition = input.notePosition;

            //newIndicator.GetComponent<InputIndicator>().SetIndicatorData();

            indicators.Add(newIndicator);
        }
    }

    public void ResetIndicators()
    {
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        
        indicators.Clear();

        InstantiateIndicators();

        if (indicators.Capacity > towerInfo.inputPatterns[upgradeIndex].noteInputs.Count)
        {
            indicators.TrimExcess();
        }
    }

    //NOTE: Ik there's gotta be an easier way to wrap around but GO MY IF STATEMENTS!
    public void UpdateInputIndex()
    {
        if (inputIndex == (towerInfo.inputPatterns[upgradeIndex].noteInputs.Count - 1))
        {
            measureCycleCount += 1;
            inputIndex = 0;
            inputTargetTime = ((measureLength * measureCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[inputIndex].noteTime);
        }
        else
        {
            inputIndex += 1;
            inputTargetTime = ((measureLength * measureCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[inputIndex].noteTime);
        }
    }

    public void UpdateAttackIndex()
    {
        if (attackIndex == (towerInfo.inputPatterns[upgradeIndex].noteInputs.Count - 1))
        {
            attackCycleCount += 1;
            attackIndex = 0;
            attackTargetTime = ((measureLength * attackCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[attackIndex].noteTime);
        }
        else
        {
            attackIndex += 1;
            attackTargetTime = ((measureLength * attackCycleCount) + towerInfo.inputPatterns[upgradeIndex].noteInputs[attackIndex].noteTime);
        }
    }

    public void towerEffectVisual()
    {
        if (towerHover && towerAboutToFire && enemyInRange)
        {
            inputPrompt.SetActive(true);
        }
        else
        {
            inputPrompt.SetActive(false);
        }
    }
    #endregion

    #region Tower attacking
    public virtual void CreateBullet(int damage, Vector3 position)
    {
        int tempRange = towerRange;

        //instatiate bullet
        GameObject bullet = Instantiate(nextProjectile, position, gameObject.transform.rotation, CombatManager.Instance.projectilesParent);
        lastBulletFired = bullet.GetComponent<Projectile>();
        lastBulletFired.InitializeProjectile(towerRange, gameObject, damage, towerInfo.projectilePiercesEnemies, attackTargetTime);

        lastBulletFired.spriteRenderer.sprite = projectileSprites[upgradeIndex];
        lastBulletFired.spriteRenderer.color = projectileColor;

        ConductorV2.instance.projectileEvent.Add(bullet.GetComponent<Projectile>().trigger);

        feelingItNow = false;
        synthBuff = false;
        isBuffed = false;

    }

    public void FireTower(int currentAttackIndex)
    {
        if(currentAttackIndex != prevAttackIndex)
        {
            prevAttackIndex = currentAttackIndex;

            switch (currentAttackPattern)
            {
                case TowerAttackPattern.standard:
                    towerAboutToFire = true;
                    Fire(0f);
                    break;
                case TowerAttackPattern.snake:

                    towerAboutToFire = true;
                    float yPosition = 0f;

                    switch (attackIndex % 4)
                    {
                        case 0:
                            yPosition = 0;
                            break;
                        case 1:
                            yPosition = 1f;
                            break;
                        case 2:
                            yPosition = 0;
                            break;
                        case 3:
                            yPosition = -1f;
                            break;
                    }
                    Fire(yPosition);
                    break;
                default:
                    break;
            }
        }
    }

    public virtual void Fire(float yPos) //default fire
    {
        //play attack sound
        AudioManager.instance.PlaySound(towerAttackSfx, this.gameObject.transform, 1.0f);
        
        if(feelingItNow) // Feeling It Now buff
        {
            attackPowerBonus = 1.0f; //+100% of base
            projectileColor = new Color(1f, 1f, 1f, 1f);
        }
        else if (isBuffed) // Regular buff
        {
            attackPowerBonus = 0.5f; //+50% of base
            projectileColor = new Color(1f, 1f, 1f, 1f);
        }
        else // No buff
        {
            attackPowerBonus = 0.0f;
            projectileColor = new Color(1f, 1f, 1f, 0.3f);
        }

        towerDamage = Mathf.RoundToInt(towerDamage * (attackPower + attackPowerBonus));
    }

    public void ExtraFire() //buff fire
    {
        
        if(towerInfo.isAOETower)
        {
            AOE(towerDamage);
            return;
        }

        CreateBullet(towerDamage, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1));

        CreateBullet(towerDamage, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1));

    }

    public void PlaceCharge(int chargeValue, Tower connectedTower)
    {
        colliders = Physics.OverlapSphere(transform.position, towerRange, LayerMask.GetMask("Stage"));

        int rand = UnityEngine.Random.Range(0, colliders.Length - 1);

        GameObject charge = Instantiate(nextProjectile, transform.position, transform.rotation, CombatManager.Instance.chargesParent);
        charge.GetComponent<Charges>().initalizeCharge(chargeValue,  new Vector3(colliders[rand].transform.position.x, 0.5f, colliders[rand].transform.position.z), connectedTower, true);
        feelingItNow = false;
        synthBuff = false;
    }

    public virtual void AOE(int damage)
    {
        int tempRange = towerRange;

        colliders = Physics.OverlapSphere(transform.position, tempRange);

        foreach (var item in colliders)
        {
            if (item.transform.CompareTag("StageTile"))
            {
                SpawnParticles(item.transform, particleEffects[0]);
            }
            else if (item.transform.CompareTag("Enemy"))
            {
                item.transform.GetComponent<Enemy>().Damage(damage); //To-Do: Call coroutine for damage sounds here too or move to Damage method 

                if(upgradeIndex == 1)
                {
                    item.transform.GetComponent<Enemy>().isStunned = true;
                }
            }
        }
        colliders = null;
        feelingItNow = false;
        synthBuff = false;
    }
    #endregion

    #region Tower animation
    //Generates the hashes for the attack frame animation
    public void GenerateAnimationHashes()
    {
        int stateIndex = 0;
        
        foreach (string animationState in animationStates)
        {
            animationHashes[stateIndex] = Animator.StringToHash(animationState);
            stateIndex += 1;
        }
    }
    
    //Switches to one of many attack frame animations temporarily and then returns to the current animation state (outside of attacking) at an offset based on the position in the current beat
    public IEnumerator InterruptAnimation()
    {
        bool isInAttackFrame = true;

        m_Animator.Play(animationHashes[4], -1, 0.0f); // play temporary action animation
        
        while (isInAttackFrame)
        {
            isInAttackFrame = false;
            yield return new WaitForSecondsRealtime(0.250f);
        }

        //Resume animation
        float animationOffset = ConductorV2.instance.beatDuration / ConductorV2.instance.crotchet;
        m_Animator.Play(animationHashes[upgradeIndex], -1, animationOffset); // return to current animation state
    }
    #endregion

    public void RemoveTower()
    {
        switch (towerInfo.type)
        {
            case InstrumentType.Flats:
                ConductorV2.instance.flats.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.flats.volume = Mathf.Clamp(ConductorV2.instance.flats.volume, 0, 0.5f);
                break;

            case InstrumentType.Trill:
                ConductorV2.instance.trill.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.trill.volume = Mathf.Clamp(ConductorV2.instance.trill.volume, 0, 0.5f);
                break;

            case InstrumentType.Major:
                ConductorV2.instance.major.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.major.volume = Mathf.Clamp(ConductorV2.instance.major.volume, 0, 0.5f);
                break;

            case InstrumentType.Chromatic:
                ConductorV2.instance.chromatic.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.chromatic.volume = Mathf.Clamp(ConductorV2.instance.chromatic.volume, 0, 0.5f);
                break;

            case InstrumentType.Allegro:
                ConductorV2.instance.allegro.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.allegro.volume = Mathf.Clamp(ConductorV2.instance.allegro.volume, 0, 0.5f);
                break;

            case InstrumentType.Poco:
                ConductorV2.instance.poco.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.poco.volume = Mathf.Clamp(ConductorV2.instance.poco.volume, 0, 0.5f);
                break;

            case InstrumentType.Forte:
                ConductorV2.instance.forte.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.forte.volume = Mathf.Clamp(ConductorV2.instance.forte.volume, 0, 0.5f);
                break;

            case InstrumentType.Legato:
                ConductorV2.instance.legato.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.legato.volume = Mathf.Clamp(ConductorV2.instance.legato.volume, 0, 0.5f);
                break;

            case InstrumentType.Tower9:
                ConductorV2.instance.Tower9.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.Tower9.volume = Mathf.Clamp(ConductorV2.instance.Tower9.volume, 0, 0.5f);
                break;

            case InstrumentType.Tower10:
                ConductorV2.instance.Tower10.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.Tower10.volume = Mathf.Clamp(ConductorV2.instance.Tower10.volume, 0, 0.5f);
                break;

            case InstrumentType.Tower11:
                ConductorV2.instance.Tower11.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.Tower11.volume = Mathf.Clamp(ConductorV2.instance.Tower11.volume, 0, 0.5f);
                break;

            case InstrumentType.Tower12:
                ConductorV2.instance.Tower12.volume -= towerAudioVolumeIncrement;
                ConductorV2.instance.Tower12.volume = Mathf.Clamp(ConductorV2.instance.Tower12.volume, 0, 0.5f);
                break;

            default:
                break;
        }

        TowerManager.Instance.RemovedTower(towerNum);
        //TowerManager.Instance.towerList.Remove(this);
        connectedTile.placedTower = null;
        Destroy(gameObject);
    }

    public virtual void Damage(int damage)
    {
        //play hurt sound
        AudioManager.instance.PlaySound(towerHurtSfx, this.gameObject.transform, 1.0f);

        if(isShielded)
        {
            isShielded = false;
            return;
        }
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            //play death sound
            AudioManager.instance.PlaySound(towerDeathSfx, this.gameObject.transform, 1.0f);

            //ADD HERE: Create instance of the tower clash particle effect
            RemoveTower();
        }
    }


    #region Tower buffing
    public void BuffAttack(float inputTime)
    {
        if (inputTime > inputTargetTime) // retroactively buff attack
        {
            if (lastBulletFired != null)
            {
                lastBulletFired.damage = Mathf.RoundToInt(towerDamage * (attackPower + attackPowerBonus));
                lastBulletFired.spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            }    
        }
        else
        {
            isBuffed = true;
        }
    }
    
    /*
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
                
                break;

            default:
                break;
        }
    }

    public void RecordBuff(BuffType buff) //records buff inputs but if more inputs are made than there are attacks in the input sequence it will remove the first recorded buff on the list
    {
        currentState = TowerState.Recording;

        recordingStatus.SetActive(true); //RECORDING STATUS CODE
        recordingStatus.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.recordingSpr;//RECORDING STATUS CODE

        isInputtingBuffs = true;

        recordedBuffs.Add(buff);

        beatRecordingStarted = ConductorV2.instance.beatTrack;
        buffTimer = 0;

        buffIndex = 0;

        if (recordedBuffs.Count > towerInfo.inputPatterns[upgradeIndex].noteInputs.Count)
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
                recordingStatus.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.repeatSprites[repeatSpritesIndex]; //RECORDING STATUS CODE
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
                if (repeatSpritesIndex <= (GameManager.Instance.repeatSprites.Count-1))
                {
                    recordingStatus.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.repeatSprites[repeatSpritesIndex];//RECORDING STATUS CODE
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
    */
    #endregion

    public void SpawnParticles(Transform tileTransform, ParticleSystem pfxSource)
    {
        ParticleSystem pfxInstance = Instantiate(pfxSource, tileTransform.position, Quaternion.identity); // Create instance of the particle effect
    }


}
