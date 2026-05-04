using System;
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
   

    public bool towerSwap;

    public AudioSource audioSource;
    public float towerAudioVolumeIncrement = 0.05f;

    public List<Tower> towerList;


    public static event Action FireTower;

    public void InstantiateTowerCooldown()
    {
        for (int i = 0; i < GameManager.Instance.towers.Count; i++)
        {
            GameManager.Instance.towers[i].towerCooldownInfo.SpawnCooldownLoadoutObject(GameManager.Instance.towers[i].tower.GetComponent<Tower>().towerInfo.towerLoadoutUIPrefab, GameManager.Instance.towers[i].tower.GetComponent<Tower>());
            GameManager.Instance.towers[i].towerCooldownInfo.currentNumberPlaced = 0;
        }
    }

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
        return GameManager.Instance.towers[towerNum].towerCooldownInfo.towerCooldown;
        
    }

    public bool CheckIfTowerAtLimit(int towerNum)
    {
        return GameManager.Instance.towers[towerNum].towerCooldownInfo.currentNumberPlaced >= CombatManager.Instance.currentEncounter.towerLimit;
        
    }

    public void PlacedTower(int towerNum)
    {
        GameManager.Instance.towers[towerNum].towerCooldownInfo.currentNumberPlaced += 1;
        
    }

    public void RemovedTower(int towerNum)
    {
        GameManager.Instance.towers[towerNum].towerCooldownInfo.currentNumberPlaced -= 1;
    }

    public void SetCooldown(int towerNum, Tower placingTower)
    {
        GameManager.Instance.towers[towerNum].towerCooldownInfo.towerCooldown = true;
        GameManager.Instance.towers[towerNum].towerCooldownInfo.towerCooldownTimeRemaining = placingTower.towerInfo.cooldownTime;
        GameManager.Instance.towers[towerNum].towerCooldownInfo.towerCooldownTime = 0;
        
        
    }

    public void SetTower(GameObject tower, Vector3 tilePosition, Tile tile, int towerNum, _BeatResult result, bool isEmpowered)
    {
        GameObject _tower = Instantiate(tower, tilePosition, Quaternion.identity, CombatManager.Instance.towersParent);
        _tower.GetComponent<BoxCollider>().enabled = true;

        _tower.GetComponent<Tower>().towerNum = towerNum;

        _tower.transform.position = tilePosition;

        tile.placedTower = _tower;

        Tower placingTower = _tower.GetComponent<Tower>();
        placingTower.connectedTile = tile;
        //placingTower.isPoweredUp = isEmpowered;
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
        SetCooldown(towerNum, placingTower);


        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerPlaceSequence)
        {
            Debug.Log("Placed tower in tutorial");
            TutorialManager.Instance.LoadNextTutorialDialogue();
            CursorTD.Instance.towerPlaceSequence = false;
            CursorTD.Instance.towerBuffSequence = true;
            Spawner.Instance.ForceEnemySpawn(-0.5f, EnemyType.Walker);
        }

        if(CombatManager.Instance.currentEncounter.enableTowerLimit)
        {
            PlacedTower(towerNum);
        }
    }

    public void DynamicMusicVolume(InstrumentType type)
    {
        if (!GameManager.Instance.tutorialRunning)
        {
            if (CombatManager.Instance.currentEncounter.enableTowerLimit)
            {
                towerAudioVolumeIncrement = 1 / CombatManager.Instance.currentEncounter.towerLimit;

            }
            else
            {
                towerAudioVolumeIncrement = 0.05f;
            }
        }

        

        switch (type)
        {
            case InstrumentType.Flats:
                ConductorV2.instance.flats.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.flats.volume = Mathf.Clamp(ConductorV2.instance.flats.volume, 0, 0.5f);

                break;

            case InstrumentType.Trill:
                ConductorV2.instance.trill.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.trill.volume = Mathf.Clamp(ConductorV2.instance.trill.volume, 0, 0.5f);

                break;

            case InstrumentType.Major:
                ConductorV2.instance.major.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.major.volume = Mathf.Clamp(ConductorV2.instance.major.volume, 0, 0.5f);
                break;

            case InstrumentType.Chromatic:
                ConductorV2.instance.chromatic.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.chromatic.volume = Mathf.Clamp(ConductorV2.instance.chromatic.volume, 0, 0.5f);
                break;

            case InstrumentType.Allegro:
                ConductorV2.instance.allegro.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.allegro.volume = Mathf.Clamp(ConductorV2.instance.allegro.volume, 0, 0.5f);

                break;

            case InstrumentType.Poco:
                ConductorV2.instance.poco.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.poco.volume = Mathf.Clamp(ConductorV2.instance.poco.volume, 0, 0.5f);

                break;

            case InstrumentType.Forte:
                ConductorV2.instance.forte.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.forte.volume = Mathf.Clamp(ConductorV2.instance.forte.volume, 0, 0.5f);

                break;

            case InstrumentType.Legato:
                ConductorV2.instance.legato.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.legato.volume = Mathf.Clamp(ConductorV2.instance.legato.volume, 0, 0.5f);

                break;

            case InstrumentType.Tower9:
                ConductorV2.instance.Tower9.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.Tower9.volume = Mathf.Clamp(ConductorV2.instance.Tower9.volume, 0, 0.5f);

                break;

            case InstrumentType.Tower10:
                ConductorV2.instance.Tower10.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.Tower10.volume = Mathf.Clamp(ConductorV2.instance.Tower10.volume, 0, 0.5f);

                break;

            case InstrumentType.Tower11:
                ConductorV2.instance.Tower11.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.Tower11.volume = Mathf.Clamp(ConductorV2.instance.Tower11.volume, 0, 0.5f);

                break;

            case InstrumentType.Tower12:
                ConductorV2.instance.Tower12.volume += towerAudioVolumeIncrement;
                ConductorV2.instance.Tower12.volume = Mathf.Clamp(ConductorV2.instance.Tower12.volume, 0, 0.5f);

                break;

            default:
                break;
        }
    }

    public void ResetTowerManager()
    {
    
        for (int i = 0; i < GameManager.Instance.towers.Count; i++)
        {
            GameManager.Instance.towers[i].towerCooldownInfo.ResetCooldownObject();
        }


        towerList.Clear();
    }

    
    public void FireTowers()
    {
        gameObject.BroadcastMessage("FireTower");

        //if (towerList.Count == 0) return;

        //foreach (Tower tower in towerList.ToArray())
        //{
        //    tower.BuffPlayback(ConductorV2.instance.beatTrack);

        //    if(tower == null)
        //    {
        //        towerList.Remove(tower);
        //        continue;
        //    }

        //    switch (tower.currentAttackPattern)
        //    {
        //        case TowerAttackPattern.everyBeat:
        //            tower.towerAboutToFire = true;
        //            tower.Fire();
        //            break;

        //        case TowerAttackPattern.everyMeasure:
        //            if (ConductorV2.instance.beatTrack == 4)
        //            {
        //                tower.Fire();
        //                tower.towerAboutToFire = false;
        //            }
        //            else if (ConductorV2.instance.beatTrack == 3)
        //            {
        //                tower.towerAboutToFire = true;
        //            }
        //            break;

        //        case TowerAttackPattern.everyOtherBeat:

        //            switch (ConductorV2.instance.beatTrack)
        //            {
        //                case 1:
        //                    tower.towerAboutToFire = true;
        //                    break;
        //                case 2:
        //                    tower.Fire();
        //                    tower.towerAboutToFire = false;
        //                    break;
        //                case 3:
        //                    tower.towerAboutToFire = true;
        //                    break;
        //                case 4:
        //                    tower.Fire();
        //                    tower.towerAboutToFire = false;
        //                    break;
        //            }
        //            break;

        //        case TowerAttackPattern.everyBeatButOne:
        //            tower.beat += 1;
        //            if (ConductorV2.instance.beatTrack < 4)
        //            {
        //                tower.towerAboutToFire = true;
        //                tower.Fire();

        //            }
        //            else if (ConductorV2.instance.beatTrack == 4)
        //            {
        //                tower.towerAboutToFire = false;
        //                tower.beat = 1;
        //            }
        //            break;

        //        case TowerAttackPattern.snakePatternFire:
                    
        //            tower.towerAboutToFire = true;
        //            float yPosition = 0f;
                    
        //            switch (ConductorV2.instance.beatTrack) 
        //            {
        //                case 1:
        //                    yPosition = 0;
        //                    break;
        //                case 2:
        //                    yPosition = 1f;
        //                    break;
        //                case 3:
        //                    yPosition = 0;
        //                    break;
        //                case 4:
        //                    yPosition = -1f;
        //                    break;
        //            }
        //            tower.Fire(yPosition);
        //            break; 

        //        default:
        //            break;
        //    }
        //}
    }

   
}

[System.Serializable]
public class TowerPlacementInfo
{
    public GameObject tower;
    public CooldownObject towerCooldownInfo;

}