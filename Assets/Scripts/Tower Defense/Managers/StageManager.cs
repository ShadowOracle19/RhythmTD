using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelSelection
{
    Default_Environment,
    Outside,
    Outside_InfoHub,
    Inside_InfoHub,
    Alert_LevelBG,
    None
}

public class StageManager : MonoBehaviour
{
    #region dont touch this
    private static StageManager _instance;
    public static StageManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("StageManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    #region Variables
    [Header("<b><size=15>Stage Environments<b><size=15>")]
    [Line(255,255,255)]
    public GameObject default_environment;
    public GameObject outside;
    public GameObject outside_InfoHub;
    public GameObject inside_InfoHub;
    public GameObject alert_LevelBG;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        HideLevel();
    }
    #endregion

    public void HideLevel()
    {
        default_environment.SetActive(false);
        outside.SetActive(false);
        outside_InfoHub.SetActive(false);
        inside_InfoHub.SetActive(false);
        alert_LevelBG.SetActive(false);
    }

    public void SetStage(LevelSelection stage)
    {
        HideLevel(); //hide all environments

        switch (stage)
        {
            case LevelSelection.Default_Environment:
                default_environment.SetActive(true);
                break;
            case LevelSelection.Outside:
                outside.SetActive(true);
                break;
            case LevelSelection.Outside_InfoHub:
                outside_InfoHub.SetActive(true);
                break;
            case LevelSelection.Inside_InfoHub:
                inside_InfoHub.SetActive(true);
                break;
            case LevelSelection.Alert_LevelBG:
                alert_LevelBG.SetActive(true);
                break;
            case LevelSelection.None:
                break;
            default:
                break;
        }

        CombatManager.Instance.SpawnStagePlatform(GameManager.Instance.currentEncounter.combatEncounter);
    }

}
