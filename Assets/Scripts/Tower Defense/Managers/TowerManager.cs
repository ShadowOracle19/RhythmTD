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
    public List<TowerPlacementInfo> towers = new List<TowerPlacementInfo>();

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
        //Cooldown();
        //UpdateCooldownShader();

        /*
        if (FeverSystem.Instance.feverModeActive == true)
        {
           stageBackground.GetComponent<Image>().material = feverModeShader; 
        }
        else
        {
            stageBackground.GetComponent<Image>().material = null; 
        }
        */
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

    public void TowerCost()
    {
        //tower1Slider.value = CombatManager.Instance.resourceNum;
        //tower2Slider.value = CombatManager.Instance.resourceNum;
        //tower3Slider.value = CombatManager.Instance.resourceNum;
        //tower4Slider.value = CombatManager.Instance.resourceNum;

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
        //drumCooldownSlot.SetActive(drumCooldown);
        //bassCooldownSlot.SetActive(bassCooldown);
        //pianoCooldownSlot.SetActive(pianoCooldown);
        //guitarCooldownSlot.SetActive(guitarCooldown);

        ////front slots
        //if (drumCooldown)
        //{
        //    drumCooldownTime += Time.deltaTime;

        //    //cooldown effect
        //    drumCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(drumCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((drumCooldownTime / drumCooldownTimeRemaining) * 100));
        //    drumCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-2, -(((drumCooldownTime / drumCooldownTimeRemaining) * 100))-120);
        //    //drumCooldownSlotPM.transform.localScale = new Vector3(1, drumCooldownTime / drumCooldownTimeRemaining, 1);

        //    if(drumCooldownTime >= drumCooldownTimeRemaining)
        //    {
        //        drumCooldown = false;
        //        drumCooldownTime = 0;
        //    }
            
        //}
        //if(bassCooldown)
        //{
        //    bassCooldownTime += Time.deltaTime;

        //    //cooldown effect
        //    bassCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(bassCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((bassCooldownTime / bassCooldownTimeRemaining) * 100));
        //    bassCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-120, -(((bassCooldownTime / bassCooldownTimeRemaining) * 100))-232);
        //    //bassCooldownSlotPM.transform.localScale = new Vector3(1, bassCooldownTime / bassCooldownTimeRemaining, 1);

        //    if (bassCooldownTime >= bassCooldownTimeRemaining)
        //    {
        //        bassCooldown = false;
        //        bassCooldownTime = 0;
        //    }
        //}
        //if(guitarCooldown)
        //{
        //    guitarCooldownTime += Time.deltaTime;

        //    //cooldown effect
        //    guitarCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(guitarCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((guitarCooldownTime / guitarCooldownTimeRemaining) * 100));
        //    guitarCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-120, -(((guitarCooldownTime / guitarCooldownTimeRemaining) * 100))-9);
        //    //guitarCooldownSlotPM.transform.localScale = new Vector3(1, guitarCooldownTime / guitarCooldownTimeRemaining, 1);

        //    if (guitarCooldownTime >= guitarCooldownTimeRemaining)
        //    {
        //        guitarCooldown = false;
        //        guitarCooldownTime = 0;
        //    }
        //}
        //if(pianoCooldown)
        //{
        //    pianoCooldownTime += Time.deltaTime;

        //    //cooldown effect
        //    pianoCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(pianoCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((pianoCooldownTime / pianoCooldownTimeRemaining) * 100));
        //    pianoCooldownSlotPM.GetComponent<RectTransform>().offsetMax = new Vector2(-223, -(((pianoCooldownTime / pianoCooldownTimeRemaining) * 100))-120);
        //    //pianoCooldownSlotPM.transform.localScale = new Vector3(1, pianoCooldownTime / pianoCooldownTimeRemaining, 1);


        //    if (pianoCooldownTime >= pianoCooldownTimeRemaining)
        //    { 
        //        pianoCooldown = false;
        //        pianoCooldownTime = 0;
        //    }
        //}
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

    //public bool CheckIfOnCoolDown(InstrumentType type)
    //{
    //    switch (type)
    //    {
    //        case InstrumentType.Drums:
    //            return drumCooldown;

    //        case InstrumentType.Guitar:
    //            return guitarCooldown;

    //        case InstrumentType.Vocal:
    //            return bassCooldown;

    //        case InstrumentType.Piano:
    //            return pianoCooldown;

    //        default:
    //            return true;
    //    }
    //}

    public bool CheckIfOnCoolDown(int towerNum)
    {
        switch (towerNum)
        {
            case 0:
                return towers[0].towerCooldownInfo.towerCooldown;

            case 1:
                return towers[1].towerCooldownInfo.towerCooldown;

            case 2:
                return towers[2].towerCooldownInfo.towerCooldown;

            case 3:
                return towers[3].towerCooldownInfo.towerCooldown;

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

                towers[0].towerCooldownInfo.towerCooldown = true;
                towers[0].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                towers[0].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 1:

                towers[1].towerCooldownInfo.towerCooldown = true;
                towers[1].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                towers[1].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 2:

                towers[2].towerCooldownInfo.towerCooldown = true;
                towers[2].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                towers[2].towerCooldownInfo.towerCooldownTime = 0;
                break;

            case 3:

                towers[3].towerCooldownInfo.towerCooldown = true;
                towers[3].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
                towers[3].towerCooldownInfo.towerCooldownTime = 0;
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
        towers[0].towerCooldownInfo.towerCooldown = false;
        towers[1].towerCooldownInfo.towerCooldown = false;
        towers[2].towerCooldownInfo.towerCooldown = false;
        towers[3].towerCooldownInfo.towerCooldown = false;

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

    /*
    void UpdateCooldownShader() 
    { 
        
        //GameObject menuTower, GameObject sidebarTower, bool shaderToggle
        if (shaderToggle)
        {
            menuTower.GetComponent<Image>().material = greyscaleShader;
            //sidebarTower.GetComponent<Image>().material = greyscaleShader;
        }
        else
        {
            menuTower.GetComponent<Image>().material = null;
            //sidebarTower.GetComponent<Image>().material = null;
        }
        

        // Check if guitar tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (guitarCooldown || CombatManager.Instance.resourceNum < towers[0].GetComponent<Tower>().towerInfo.resourceCost) 
        {
            menuTower1.GetComponent<Image>().material = greyscaleShader;
        }
        else if (!guitarCooldown && CombatManager.Instance.resourceNum >= towers[0].GetComponent<Tower>().towerInfo.resourceCost)
        {
            menuTower1.GetComponent<Image>().material = null;
        }
        
        // Check if drum tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (drumCooldown || CombatManager.Instance.resourceNum < towers[1].GetComponent<Tower>().towerInfo.resourceCost) 
        {
            menuTower2.GetComponent<Image>().material = greyscaleShader;
        }
        else if (!drumCooldown && CombatManager.Instance.resourceNum >= towers[1].GetComponent<Tower>().towerInfo.resourceCost)
        {
            menuTower2.GetComponent<Image>().material = null;
        }

        // Check if bass tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (bassCooldown || CombatManager.Instance.resourceNum < towers[2].GetComponent<Tower>().towerInfo.resourceCost) 
        {
            menuTower3.GetComponent<Image>().material = greyscaleShader;
        }
        else if (!bassCooldown && CombatManager.Instance.resourceNum >= towers[2].GetComponent<Tower>().towerInfo.resourceCost)
        {
            menuTower3.GetComponent<Image>().material = null;
        }

        // Check if piano tower is on cooldown or player does not have enough resources to purchase them. If so, apply greyscale shader material. Otherwise, remove.
        if (pianoCooldown || CombatManager.Instance.resourceNum < towers[3].GetComponent<Tower>().towerInfo.resourceCost) 
        {
            menuTower4.GetComponent<Image>().material = greyscaleShader;
        }
        else if (!pianoCooldown && CombatManager.Instance.resourceNum >= towers[3].GetComponent<Tower>().towerInfo.resourceCost)
        {
            menuTower4.GetComponent<Image>().material = null;
        }
    */

}

[System.Serializable]
public class TowerPlacementInfo
{
    public GameObject tower;
    public CooldownObject towerCooldownInfo;
}