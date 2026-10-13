using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region dont touch this
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("ScoreManager is NULL");
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
    [Header("<b><size=15>Score<b><size=15>")]
    [Line(255,255,255)]
    public int score;
    public int combo;
    public int highestCombo;
    public int multiplier;

    public int multiplierMin;
    public int multiplierMax;
    public int streakIncrement;
    
    [Space(20)][Header("<b><size=15>Score Pulse<b><size=15>")]
    [Line(255,255,255)]
    public Vector3 textDefaultSize = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 textPulseSize = new Vector3(1.0f, 1.25f, 1.0f);

    /*
    [Space(20)][Header("<b><size=15>Score Points<b><size=15>")]
    [Line(255,255,255)]

    */

    [Space(20)][Header("<b><size=15>UI<b><size=15>")]
    [Line(255,255,255)]
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI scoreText;
    public Transform comboTextParent;
    public Transform multiplierTextParent;
    public Transform scoreTextParent;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        combo = 0;
        highestCombo = 0;
        multiplier = 1;
    }
    #endregion

    #region  Update
    // Update is called once per frame
    void Update()
    {
        multiplier = Mathf.Clamp(multiplier, multiplierMin, multiplierMax);

        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        multiplierText.text = multiplier.ToString(); 
    }
    #endregion

    #region Scoring
    public void IncreaseCombo()
    {
        combo += 1; //increase combo

        comboTextParent.localScale = textPulseSize; //animation feedback
        StartCoroutine(TextPulse(comboTextParent));

        if(combo % streakIncrement == 0) //increase multiplier 
        {
            multiplierTextParent.localScale = textPulseSize;
            multiplier += 1;
            StartCoroutine(TextPulse(multiplierTextParent));
        }
    }

    public void IncreaseScore(int points)
    {
        score += points * multiplier; //increase score
        
        scoreTextParent.localScale = textPulseSize; //animation feedback
        StartCoroutine(TextPulse(scoreTextParent));
    }

    public void ResetStageScoreData()
    {
        score = 0;
        combo = 0;
        highestCombo = 0;
        multiplier = multiplierMin;
    }

    public void ResetCombo()
    {
        if(combo > highestCombo)
        {
            highestCombo = combo;
        }

        combo = 0; //reset combo
        multiplier = multiplierMin; //reset multiplier
    }
    #endregion

    #region Animation
    public IEnumerator TextPulse(Transform textParent)
    {
        float progress = 0.0f;
        
        while (textParent.localScale.x > textDefaultSize.x)
        {
            textParent.localScale = Vector3.Slerp(textParent.localScale, textDefaultSize, progress); //return score text to origin size
            progress += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
