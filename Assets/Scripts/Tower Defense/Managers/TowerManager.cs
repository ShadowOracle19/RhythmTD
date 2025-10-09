using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TowerManager : MonoBehaviour
{
    #region dont touch this
    private static TowerManager _instance;
    public static TowerManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("tower Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public GameObject towerToPlace;
    public bool isTowerHovering = false;
    

    //Tower background shader management
    /*
    public Material feverModeShader;

    public GameObject stageBackground;
    */
    
    //Tower menu shader management
    public Material greyscaleShader;
    //public Material overchargeShader;


    //Note: After this sprint replace to be more dynamic
    //public GameObject towerOneCooldownSlot;
    //public bool towerOneCooldown;
    //private float towerOneCooldownTimeRemaining = 0;
    //private float towerOneCooldownTime = 0;

    //public GameObject towerTwoCooldownSlot;
    //public bool towerTwoCooldown;
    //private float towerTwoCooldownTimeRemaining = 0;
    //private float towerTwoCooldownTime = 0;

    //public GameObject towerThreeCooldownSlot;
    //public bool towerThreeCooldown;
    //private float towerThreeCooldownTimeRemaining = 0;
    //private float towerThreeCooldownTime = 0;

    //public GameObject towerFourCooldownSlot;
    //public bool towerFourCooldown;
    //private float towerFourCooldownTimeRemaining = 0;
    //private float towerFourCooldownTime = 0;



    //public GameObject drumCooldownSlotPM;
    //public GameObject bassCooldownSlotPM;
    //public GameObject pianoCooldownSlotPM;
    //public GameObject guitarCooldownSlotPM;


    //[Header("Tower Cost Labels")]
    //public Sprite oneBar;
    //public Sprite twoBar;
    //public Sprite threeBar;
    //public Sprite fourBar;

    //public Slider tower1Slider;
    //public Image tower1ResourceSprite;

    //public Slider tower2Slider;
    //public Image tower2ResourceSprite;

    //public Slider tower3Slider;
    //public Image tower3ResourceSprite;

    //public Slider tower4Slider;
    //public Image tower4ResourceSprite;

    //public TextMeshProUGUI tower1Cost;
    //public TextMeshProUGUI tower2Cost;
    //public TextMeshProUGUI tower3Cost;
    //public TextMeshProUGUI tower4Cost;

    //public TextMeshPro towerCost1PM;
    //public TextMeshPro towerCost2PM;
    //public TextMeshPro towerCost3PM;
    //public TextMeshPro towerCost4PM;

    public bool towerSwap;

    public AudioSource audioSource;
    public float towerAudioVolumeIncrement = 0.05f;

    public List<Tower> towerList;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResourceBarSprite(Tower tower, Slider resourceSlider, Image resourceImage)
    {
        switch (tower.towerInfo.cost)
        {
            case TowerResourceCost.one:
                resourceSlider.maxValue = 25;
                break;

            case TowerResourceCost.two:
                resourceSlider.maxValue = 50;
                break;

            case TowerResourceCost.three:
                resourceSlider.maxValue = 75;
                break;

            case TowerResourceCost.four:
                resourceSlider.maxValue = 100;
                break;

            default:
                break;
        }
    }

    
    public void SwapTowers()
    {
        //(drumCooldown, drumCooldownBack) = (drumCooldownBack, drumCooldown);
        //(bassCooldown, bassCooldownBack) = (bassCooldownBack, bassCooldown);
        //(guitarCooldown, guitarCooldownBack) = (guitarCooldownBack, guitarCooldown);
        //(pianoCooldown, pianoCooldownBack) = (pianoCooldownBack, pianoCooldown);

        //(drumCooldownTime, drumCooldownTimeBack) = (drumCooldownTimeBack, drumCooldownTime);
        //(bassCooldownTime, bassCooldownTimeBack) = (bassCooldownTimeBack, bassCooldownTime);
        //(guitarCooldownTime, guitarCooldownTimeBack) = (guitarCooldownTimeBack, guitarCooldownTime);
        //(pianoCooldownTime, pianoCooldownTimeBack) = (pianoCooldownTimeBack, pianoCooldownTime);

        //(drumCooldownTimeRemaining, drumCooldownTimeRemainingBack) = (drumCooldownTimeRemainingBack, drumCooldownTimeRemaining);
        //(bassCooldownTimeRemaining, bassCooldownTimeRemainingBack) = (bassCooldownTimeRemainingBack, bassCooldownTimeRemaining);
        //(guitarCooldownTimeRemaining, guitarCooldownTimeRemainingBack) = (guitarCooldownTimeRemainingBack, guitarCooldownTimeRemaining);
        //(pianoCooldownTimeRemaining, pianoCooldownTimeRemainingBack) = (pianoCooldownTimeRemainingBack, pianoCooldownTimeRemaining);

        //towerSwap = !towerSwap;
    }

    
    public bool CheckIfOnCoolDown(int towerNum)
    {
        switch (towerNum)
        {
            case 0:
                return GameManager.Instance.towers[0].towerCooldownInfo.towerCooldown;

            case 1:
                return GameManager.Instance.towers[1].towerCooldownInfo.towerCooldown;

            case 2:
                return GameManager.Instance.towers[2].towerCooldownInfo.towerCooldown;

            case 3:
                return GameManager.Instance.towers[3].towerCooldownInfo.towerCooldown;

            default:
                return true;
        }
    }



    public void SetTower(GameObject tower, Vector3 tilePosition, Tile tile, int towerNum, _BeatResult result, bool isEmpowered)
    {
        GameObject _tower = Instantiate(tower, tilePosition, Quaternion.identity, CombatManager.Instance.towersParent);
        _tower.GetComponent<SpriteFollowMouse>().enabled = false;
        _tower.GetComponent<BoxCollider>().enabled = true;

        _tower.transform.position = tilePosition;

        tile.placedTower = _tower;

        Tower placingTower = _tower.GetComponent<Tower>();
        placingTower.connectedTile = tile;
        placingTower.isPoweredUp = isEmpowered;
        audioSource.Play();
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);

        //if placed on a certain beat result increase tower base damage
        switch (result)
        {
            case _BeatResult.miss:
                placingTower.currentDamage = placingTower.towerInfo.damage;


                break;
            case _BeatResult.early:
                placingTower.currentDamage = placingTower.towerInfo.damage + 1;
                break;
            case _BeatResult.perfect:
                placingTower.currentDamage = placingTower.towerInfo.damage + 2;
                break;
            default:
                break;
        }
        placingTower.tempDamageHolder = placingTower.currentDamage;

        towerList.Add(placingTower);

        //adjust volume
        DynamicMusicVolume(tower.GetComponent<Tower>().towerInfo.type);

        //set cooldown
        switch (towerNum)
        {
            case 0:

                GameManager.Instance.towers[0].towerCooldownInfo.towerCooldown = true;
                GameManager.Instance.towers[0].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                GameManager.Instance.towers[0].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 1:

                GameManager.Instance.towers[1].towerCooldownInfo.towerCooldown = true;
                GameManager.Instance.towers[1].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                GameManager.Instance.towers[1].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 2:

                GameManager.Instance.towers[2].towerCooldownInfo.towerCooldown = true;
                GameManager.Instance.towers[2].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                GameManager.Instance.towers[2].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 3:

                GameManager.Instance.towers[3].towerCooldownInfo.towerCooldown = true;
                GameManager.Instance.towers[3].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                GameManager.Instance.towers[3].towerCooldownInfo.towerCooldownTime = 0;
                break;

            default:
                break;
        }


        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerPlaceSequence)
        {
            TutorialManager.Instance.LoadNextTutorialDialogue();
            CursorTD.Instance.towerPlaceSequence = false;
            CursorTD.Instance.towerBuffSequence = true;
            Spawner.Instance.ForceEnemySpawn(-0.5f, EnemyType.Walker);
        }
    }

    public void DynamicMusicVolume(InstrumentType type)
    {
        switch (type)
        {
            case InstrumentType.Drums:
                ConductorV2.instance.drums.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.drums.volume = Mathf.Clamp(ConductorV2.instance.drums.volume, 0, 0.5f);

                break;

            case InstrumentType.Guitar:
                ConductorV2.instance.guitarH.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.guitarM.volume += towerAudioVolumeIncrement;

                ConductorV2.instance.guitarH.volume = Mathf.Clamp(ConductorV2.instance.guitarH.volume, 0, 0.5f);
                ConductorV2.instance.guitarM.volume = Mathf.Clamp(ConductorV2.instance.guitarM.volume, 0, 0.5f);

                break;

            case InstrumentType.Vocal:
                ConductorV2.instance.bass.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.bass.volume = Mathf.Clamp(ConductorV2.instance.bass.volume, 0, 0.5f);
                break;

            case InstrumentType.Piano:
                ConductorV2.instance.piano.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.piano.volume = Mathf.Clamp(ConductorV2.instance.piano.volume, 0, 0.5f);

                break;
            default:
                break;
        }
    }

    public void ResetTowerManager()
    {
        GameManager.Instance.towers[0].towerCooldownInfo.towerCooldown = false;
        GameManager.Instance.towers[1].towerCooldownInfo.towerCooldown = false;
        GameManager.Instance.towers[2].towerCooldownInfo.towerCooldown = false;
        GameManager.Instance.towers[3].towerCooldownInfo.towerCooldown = false;

        towerList.Clear();
    }

    
    public void FireTowers()
    {
        if (towerList.Count == 0) return;

        foreach (Tower tower in towerList.ToArray())
        {
            tower.BuffPlayback(ConductorV2.instance.beatTrack);

            if(tower == null)
            {
                towerList.Remove(tower);
                continue;
            }

            switch (tower.currentAttackPattern)
            {
                case TowerAttackPattern.everyBeat:
                    tower.towerAboutToFire = true;
                    tower.Fire();
                    break;

                case TowerAttackPattern.everyMeasure:
                    if (ConductorV2.instance.beatTrack == 4)
                    {
                        tower.Fire();
                        tower.towerAboutToFire = false;
                    }
                    else if (ConductorV2.instance.beatTrack == 3)
                    {
                        tower.towerAboutToFire = true;
                    }
                    break;

                case TowerAttackPattern.everyOtherBeat:

                    switch (ConductorV2.instance.beatTrack)
                    {
                        case 1:
                            tower.towerAboutToFire = true;
                            break;
                        case 2:
                            tower.Fire();
                            tower.towerAboutToFire = false;
                            break;
                        case 3:
                            tower.towerAboutToFire = true;
                            break;
                        case 4:
                            tower.Fire();
                            tower.towerAboutToFire = false;
                            break;
                    }
                    break;

                case TowerAttackPattern.everyBeatButOne:
                    tower.beat += 1;
                    if (ConductorV2.instance.beatTrack < 4)
                    {
                        tower.towerAboutToFire = true;
                        tower.Fire();

                    }
                    else if (ConductorV2.instance.beatTrack == 4)
                    {
                        tower.towerAboutToFire = false;
                        tower.beat = 1;
                    }
                    break;

                case TowerAttackPattern.snakePatternFire:
                    
                    tower.towerAboutToFire = true;
                    float yPosition = 0f;
                    
                    switch (ConductorV2.instance.beatTrack) 
                    {
                        case 1:
                            yPosition = 0;
                            break;
                        case 2:
                            yPosition = 1f;
                            break;
                        case 3:
                            yPosition = 0;
                            break;
                        case 4:
                            yPosition = -1f;
                            break;
                    }
                    tower.Fire(yPosition);
                    break; 

                default:
                    break;
            }
        }
    }

   
}

[System.Serializable]
public class TowerPlacementInfo
{
    public GameObject tower;
    public CooldownObject towerCooldownInfo;
}