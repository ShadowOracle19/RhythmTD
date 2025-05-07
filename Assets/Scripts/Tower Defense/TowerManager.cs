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
    
    //list of current towers the player has
    public List<GameObject> towers = new List<GameObject>();

    //Tower background shader management
    public Material feverModeShader;

    public GameObject stageBackground;
    
    //Tower menu shader management
    public Material greyscaleShader;
    public Material overchargeShader;

    public GameObject menuTower1;
    public GameObject menuTower2;
    public GameObject menuTower3;
    public GameObject menuTower4;

    public GameObject sidebarTower1;
    public GameObject sidebarTower2;
    public GameObject sidebarTower3;
    public GameObject sidebarTower4;
    
    //Note: After this sprint replace to be more dynamic
    public GameObject drumCooldownSlot;
    public bool drumCooldown;
    private float drumCooldownTimeRemaining = 0;
    private float drumCooldownTime = 0;

    public GameObject bassCooldownSlot;
    public bool bassCooldown;
    private float bassCooldownTimeRemaining = 0;
    private float bassCooldownTime = 0;

    public GameObject guitarCooldownSlot;
    public bool guitarCooldown;
    private float guitarCooldownTimeRemaining = 0;
    private float guitarCooldownTime = 0;

    public GameObject pianoCooldownSlot;
    public bool pianoCooldown;
    private float pianoCooldownTimeRemaining = 0;
    private float pianoCooldownTime = 0;



    public GameObject drumCooldownSlotPM;
    public GameObject bassCooldownSlotPM;
    public GameObject pianoCooldownSlotPM;
    public GameObject guitarCooldownSlotPM;


    [Header("Tower Cost Labels")]
    public Sprite oneBar;
    public Sprite twoBar;
    public Sprite threeBar;
    public Sprite fourBar;

    public Slider tower1Slider;
    public Image tower1ResourceSprite;

    public Slider tower2Slider;
    public Image tower2ResourceSprite;

    public Slider tower3Slider;
    public Image tower3ResourceSprite;

    public Slider tower4Slider;
    public Image tower4ResourceSprite;

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

    [Header("Tower")]
    [SerializeField] private List<Tower> towerList;
    public bool everyOtherBeat;

    // Update is called once per frame
    void Update()
    {
        Cooldown();
        UpdateCooldownShader();

        if (FeverSystem.Instance.feverModeActive == true)
        {
           stageBackground.GetComponent<Image>().material = feverModeShader; 
        }
        else
        {
            stageBackground.GetComponent<Image>().material = null; 
        }
    }

    public void SetupResourceBars()
    {
        SetResourceBarSprite(towers[0].GetComponent<Tower>(), tower1Slider, tower1ResourceSprite);
        SetResourceBarSprite(towers[1].GetComponent<Tower>(), tower2Slider, tower2ResourceSprite);
        SetResourceBarSprite(towers[2].GetComponent<Tower>(), tower3Slider, tower3ResourceSprite);
        SetResourceBarSprite(towers[3].GetComponent<Tower>(), tower4Slider, tower4ResourceSprite);
    }

    public void SetResourceBarSprite(Tower tower, Slider resourceSlider, Image resourceImage)
    {
        switch (tower.towerInfo.cost)
        {
            case TowerResourceCost.one:
                resourceSlider.maxValue = 25;
                resourceImage.sprite = oneBar;
                break;

            case TowerResourceCost.two:
                resourceSlider.maxValue = 50;
                resourceImage.sprite = twoBar;
                break;

            case TowerResourceCost.three:
                resourceSlider.maxValue = 75;
                resourceImage.sprite = threeBar;
                break;

            case TowerResourceCost.four:
                resourceSlider.maxValue = 100;
                resourceImage.sprite = fourBar;
                break;

            default:
                break;
        }
    }

    public void TowerCost()
    {
        tower1Slider.value = CombatManager.Instance.resourceNum;
        tower2Slider.value = CombatManager.Instance.resourceNum;
        tower3Slider.value = CombatManager.Instance.resourceNum;
        tower4Slider.value = CombatManager.Instance.resourceNum;

        //if (!towerSwap)
        //{
        //    tower1Cost.text = towers[0].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower2Cost.text = towers[1].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower3Cost.text = towers[2].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower4Cost.text = towers[3].GetComponent<Tower>().towerInfo.resourceCost.ToString();

        //    towerCost1PM.text = towers[0].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost2PM.text = towers[1].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost3PM.text = towers[2].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost4PM.text = towers[3].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //}
        //else
        //{
        //    tower1Cost.text = towers[4].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower2Cost.text = towers[5].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower3Cost.text = towers[6].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    tower4Cost.text = towers[7].GetComponent<Tower>().towerInfo.resourceCost.ToString();

        //    towerCost1PM.text = towers[4].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost2PM.text = towers[5].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost3PM.text = towers[6].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //    towerCost4PM.text = towers[7].GetComponent<Tower>().towerInfo.resourceCost.ToString();
        //}
        
    }

    public void Cooldown()
    {
        drumCooldownSlot.SetActive(drumCooldown);
        bassCooldownSlot.SetActive(bassCooldown);
        pianoCooldownSlot.SetActive(pianoCooldown);
        guitarCooldownSlot.SetActive(guitarCooldown);

        drumCooldownSlotPM.SetActive(drumCooldown);
        bassCooldownSlotPM.SetActive(bassCooldown);
        pianoCooldownSlotPM.SetActive(pianoCooldown);
        guitarCooldownSlotPM.SetActive(guitarCooldown);
        //front slots
        if (drumCooldown)
        {
            drumCooldownTime += Time.deltaTime;

            //cooldown effect
            drumCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(drumCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((drumCooldownTime / drumCooldownTimeRemaining) * 100));
            drumCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-2, -(((drumCooldownTime / drumCooldownTimeRemaining) * 100))-120);
            //drumCooldownSlotPM.transform.localScale = new Vector3(1, drumCooldownTime / drumCooldownTimeRemaining, 1);

            if(drumCooldownTime >= drumCooldownTimeRemaining)
            {
                drumCooldown = false;
                drumCooldownTime = 0;
            }
            
        }
        if(bassCooldown)
        {
            bassCooldownTime += Time.deltaTime;

            //cooldown effect
            bassCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(bassCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((bassCooldownTime / bassCooldownTimeRemaining) * 100));
            bassCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-120, -(((bassCooldownTime / bassCooldownTimeRemaining) * 100))-232);
            //bassCooldownSlotPM.transform.localScale = new Vector3(1, bassCooldownTime / bassCooldownTimeRemaining, 1);

            if (bassCooldownTime >= bassCooldownTimeRemaining)
            {
                bassCooldown = false;
                bassCooldownTime = 0;
            }
        }
        if(guitarCooldown)
        {
            guitarCooldownTime += Time.deltaTime;

            //cooldown effect
            guitarCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(guitarCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((guitarCooldownTime / guitarCooldownTimeRemaining) * 100));
            guitarCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-120, -(((guitarCooldownTime / guitarCooldownTimeRemaining) * 100))-9);
            //guitarCooldownSlotPM.transform.localScale = new Vector3(1, guitarCooldownTime / guitarCooldownTimeRemaining, 1);

            if (guitarCooldownTime >= guitarCooldownTimeRemaining)
            {
                guitarCooldown = false;
                guitarCooldownTime = 0;
            }
        }
        if(pianoCooldown)
        {
            pianoCooldownTime += Time.deltaTime;

            //cooldown effect
            pianoCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(pianoCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((pianoCooldownTime / pianoCooldownTimeRemaining) * 100));
            pianoCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-223, -(((pianoCooldownTime / pianoCooldownTimeRemaining) * 100))-120);
            //pianoCooldownSlotPM.transform.localScale = new Vector3(1, pianoCooldownTime / pianoCooldownTimeRemaining, 1);


            if (pianoCooldownTime >= pianoCooldownTimeRemaining)
            { 
                pianoCooldown = false;
                pianoCooldownTime = 0;
            }
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

    public bool CheckIfOnCoolDown(InstrumentType type)
    {
        switch (type)
        {
            case InstrumentType.Drums:
                return drumCooldown;

            case InstrumentType.Guitar:
                return guitarCooldown;

            case InstrumentType.Bass:
                return bassCooldown;

            case InstrumentType.Piano:
                return pianoCooldown;

            default:
                return true;
        }
    }

    public void SetTower(GameObject tower, Vector3 tilePosition, Tile tile, InstrumentType type, _BeatResult result, bool isEmpowered)
    {
        GameObject _tower = Instantiate(tower, tilePosition, Quaternion.identity, CombatManager.Instance.towersParent);
        _tower.GetComponent<SpriteFollowMouse>().enabled = false;
        _tower.GetComponent<BoxCollider>().enabled = true;

        _tower.transform.position = tilePosition;

        tile.placedTower = _tower;

        Tower placingTower = _tower.GetComponent<Tower>();
        placingTower.rotateStarted = true;
        placingTower.connectedTile = tile;
        placingTower.isPoweredUp = isEmpowered;
        audioSource.Play();
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);

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

        switch (type)
        {
            case InstrumentType.Drums:

                ConductorV2.instance.drums.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.drums.volume = Mathf.Clamp(ConductorV2.instance.drums.volume, 0, 0.5f);

                drumCooldown = true;
                drumCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                drumCooldownTime = 0;
                break;

            case InstrumentType.Guitar:

                ConductorV2.instance.guitarH.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.guitarM.volume += towerAudioVolumeIncrement;

                ConductorV2.instance.guitarH.volume = Mathf.Clamp(ConductorV2.instance.guitarH.volume, 0, 0.5f);
                ConductorV2.instance.guitarM.volume = Mathf.Clamp(ConductorV2.instance.guitarM.volume, 0, 0.5f);

                guitarCooldown = true;
                guitarCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                guitarCooldownTime = 0;
                break;

            case InstrumentType.Bass:

                ConductorV2.instance.bass.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.bass.volume = Mathf.Clamp(ConductorV2.instance.bass.volume, 0, 0.5f);

                bassCooldown = true;
                bassCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                bassCooldownTime = 0;
                break;

            case InstrumentType.Piano:

                ConductorV2.instance.piano.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.piano.volume = Mathf.Clamp(ConductorV2.instance.piano.volume, 0, 0.5f);

                pianoCooldown = true;
                pianoCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                pianoCooldownTime = 0;
                break;

            default:
                break;
        }


        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerPlaceSequence)
        {
            TutorialManager.Instance.LoadNextTutorialDialogue();
            CursorTD.Instance.towerPlaceSequence = false;
            CursorTD.Instance.towerBuffSequence = true;
            EnemySpawner.Instance.ForceEnemySpawn(gameObject.transform.position.y, EnemyType.Walker);
        }
    }

    public void ResetTowerManager()
    {
        drumCooldown = false;
        bassCooldown = false;
        pianoCooldown = false;
        guitarCooldown = false;

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

            if(tower.towerInfo.type == InstrumentType.Piano && tower.isPoweredUp)
            {
                CombatManager.Instance.resourceNum += 1;
            }

            switch (tower.towerInfo.attackPattern)
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

    void UpdateCooldownShader() 
    { 
        //GameObject menuTower, GameObject sidebarTower, bool shaderToggle
        /*
        if (shaderToggle)
        {
            menuTower.GetComponent<Image>().material = greyscaleShader;
            sidebarTower.GetComponent<Image>().material = greyscaleShader;
        }
        else
        {
            menuTower.GetComponent<Image>().material = null;
            sidebarTower.GetComponent<Image>().material = null;
        }
        */
        
        if (CombatManager.Instance.resourceNum == CombatManager.Instance.maxResource)
        {
            menuTower1.GetComponent<Image>().material = overchargeShader;
            sidebarTower1.GetComponent<Image>().material = overchargeShader;

            menuTower2.GetComponent<Image>().material = overchargeShader;
            sidebarTower2.GetComponent<Image>().material = overchargeShader;

            menuTower3.GetComponent<Image>().material = overchargeShader;
            sidebarTower3.GetComponent<Image>().material = overchargeShader;

            menuTower4.GetComponent<Image>().material = overchargeShader;
            sidebarTower4.GetComponent<Image>().material = overchargeShader;
        }
        else
        {
            if (guitarCooldown || CombatManager.Instance.resourceNum < towers[0].GetComponent<Tower>().towerInfo.resourceCost) 
            {
                menuTower1.GetComponent<Image>().material = greyscaleShader;
                sidebarTower1.GetComponent<Image>().material = greyscaleShader;
            }
            else if (!guitarCooldown && CombatManager.Instance.resourceNum >= towers[0].GetComponent<Tower>().towerInfo.resourceCost)
            {
                menuTower1.GetComponent<Image>().material = null;
                sidebarTower1.GetComponent<Image>().material = null;
            }
            
            if (drumCooldown || CombatManager.Instance.resourceNum < towers[1].GetComponent<Tower>().towerInfo.resourceCost) 
            {
                menuTower2.GetComponent<Image>().material = greyscaleShader;
                sidebarTower2.GetComponent<Image>().material = greyscaleShader;
            }
            else if (!drumCooldown && CombatManager.Instance.resourceNum >= towers[1].GetComponent<Tower>().towerInfo.resourceCost)
            {
                menuTower2.GetComponent<Image>().material = null;
                sidebarTower2.GetComponent<Image>().material = null;
            }

            if (bassCooldown || CombatManager.Instance.resourceNum < towers[2].GetComponent<Tower>().towerInfo.resourceCost) 
            {
                menuTower3.GetComponent<Image>().material = greyscaleShader;
                sidebarTower3.GetComponent<Image>().material = greyscaleShader;
            }
            else if (!bassCooldown && CombatManager.Instance.resourceNum >= towers[2].GetComponent<Tower>().towerInfo.resourceCost)
            {
                menuTower3.GetComponent<Image>().material = null;
                sidebarTower3.GetComponent<Image>().material = null;
            }

            if (pianoCooldown || CombatManager.Instance.resourceNum < towers[3].GetComponent<Tower>().towerInfo.resourceCost) 
            {
                menuTower4.GetComponent<Image>().material = greyscaleShader;
                sidebarTower4.GetComponent<Image>().material = greyscaleShader;
            }
            else if (!pianoCooldown && CombatManager.Instance.resourceNum >= towers[3].GetComponent<Tower>().towerInfo.resourceCost)
            {
                menuTower4.GetComponent<Image>().material = null;
                sidebarTower4.GetComponent<Image>().material = null;
            }
        }
        
    }
}
