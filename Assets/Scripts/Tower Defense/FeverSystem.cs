using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverSystem : MonoBehaviour
{
    #region dont touch this
    private static FeverSystem _instance;
    public static FeverSystem Instance
    {
        get
        {
            if (_instance is null)
            {
                //Debug.LogError("FeverSystemManager is NULL");
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
    [Header("<b><size=15>Fever<b><size=15>")]
    [Line(255,255,255)]
    public bool feverModeActive = false;
    public int feverBarNum = 0;

    [Space(20)][Header("<b><size=15>UI<b><size=15>")]
    [Line(255,255,255)]
    public Slider feverBar;
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        feverBarNum = Mathf.Clamp(feverBarNum, 0, 100);
        feverBar.value = feverBarNum;
        if(feverBarNum == 0)
        {
            feverModeActive = false;
        }
    }
    #endregion

    public void ActivateFeverMode()
    {
        if(feverBarNum == 100)
        {
            feverModeActive = true;

            if(GameManager.Instance.tutorialRunning && CursorTD.Instance.feverModeSequence)
            {
                CursorTD.Instance.feverModeSequence = false;   
            }
        }
    }

    public void FeverBarBeat()
    {
        if (feverModeActive)
        {
            feverBarNum -= 5;
        }
        else
        {
            feverBarNum += 1 * ScoreManager.Instance.multiplier;
        }
    }
}
