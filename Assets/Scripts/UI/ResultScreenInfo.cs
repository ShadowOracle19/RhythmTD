using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScreenInfo : MonoBehaviour
{
    [SerializeField] private Image characterSprite;
    [SerializeField] private Sprite characterwin;
    [SerializeField] private Sprite characterlose;
    [SerializeField] private TMP_Text missionResult;
    [SerializeField] private TMP_Text encounterName;
    [SerializeField] private TMP_Text difficulty;
    [SerializeField] private TMP_Text totalScore;
    [SerializeField] private TMP_Text highScore;
    [SerializeField] private TMP_Text grade;

    [Header("Objectives")]
    [SerializeField] private Sprite complete;
    [SerializeField] private Sprite incomplete;

    [SerializeField] private Image object1Highlight;
    [SerializeField] private GameObject object1TextHighlight;
    [SerializeField] private TMP_Text object1Text;

    [Space(5)]
    [SerializeField] private Image object2Highlight;
    [SerializeField] private GameObject object2TextHighlight;
    [SerializeField] private TMP_Text object2Text;

    [Space(5)]
    [SerializeField] private Image object3Highlight;
    [SerializeField] private GameObject object3TextHighlight;
    [SerializeField] private TMP_Text object3Text;

    public void WriteToResultScreen(bool isWin, string _encounterName,int score, int _highScore, bool completeObject2, bool completeObject3)
    {
        difficulty.text = "normal";
        if (isWin)//win state
        {
            missionResult.text = "Mission Complete";
            encounterName.text = _encounterName;
            totalScore.text = score.ToString();
            highScore.text = _highScore.ToString();
            grade.text = "S";
            grade.color = Color.green;

            object1Highlight.sprite = complete;
            object1TextHighlight.gameObject.SetActive(true);
            object1Text.color = Color.white;

            CheckObjective2(completeObject2);

            CheckObjective3 (completeObject3);

            characterSprite.sprite = characterwin;

        }
        else //lose state
        {
            missionResult.text = "Mission Failed";
            encounterName.text = _encounterName;
            totalScore.text = score.ToString();
            highScore.text = _highScore.ToString();
            grade.text = "F";
            grade.color = Color.grey;

            object1Highlight.sprite = incomplete;
            object1TextHighlight.gameObject.SetActive(false);
            object1Text.color = Color.grey;

            CheckObjective2(completeObject2);

            CheckObjective3(completeObject3);

            characterSprite.sprite = characterlose;
        }
    }

    public void CheckObjective2(bool objective2)
    {
        if (objective2)
        {
            object2Highlight.sprite = complete;
            object2TextHighlight.gameObject.SetActive(true);
            object2Text.color = Color.white;
        }
        else
        {
            object2Highlight.sprite = incomplete;
            object2TextHighlight.gameObject.SetActive(false);
            object2Text.color = Color.grey;
        }
    }

    public void CheckObjective3(bool objective3)
    {
        if (objective3)
        {
            object3Highlight.sprite = complete;
            object3TextHighlight.gameObject.SetActive(true);
            object3Text.color = Color.white;
        }
        else
        {
            object3Highlight.sprite = incomplete;
            object3TextHighlight.gameObject.SetActive(false);
            object3Text.color = Color.grey;
        }
    }
}
