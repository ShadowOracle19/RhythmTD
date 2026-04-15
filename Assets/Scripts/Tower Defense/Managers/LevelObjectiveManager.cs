using UnityEngine;

public class LevelObjectiveManager : MonoBehaviour
{
    #region dont touch this
    private static LevelObjectiveManager _instance;
    public static LevelObjectiveManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("LevelObjectiveManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfObjectiveWasCompleted(MoreLevelObjectives currentObjective)
    {
        switch (currentObjective)
        {
            case MoreLevelObjectives.ClearWithFourTowers:
                return CombatManager.Instance.towersParent.childCount <= 4;
                
            default:
                break;
        }

        return false;
    }
}
