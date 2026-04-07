using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    #region dont touch this
    private static ComboManager _instance;
    public static ComboManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("ComboManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public int currentCombo;
    public int highestCombo;
    public int currentMultiplier;
    public int streakIncrement;
    public int score;

    [Header ("Score Points")]

    [Header ("UI Elements")]
    public TextMeshProUGUI currentComboText;
    public TextMeshProUGUI currentMultiplierText;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentCombo = 0;
        highestCombo = 0;
        currentMultiplier = 1;
        streakIncrement = 50;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentMultiplier = Mathf.Clamp(currentMultiplier, 1, 5);

        currentComboText.text = "COMBO " + currentCombo;
        currentMultiplierText.text = "MULTIPLIER X" + currentMultiplier;
        scoreText.text = "SCORE " + score;
    }

    public void IncreaseCombo()
    {
        currentCombo += 1; //increase combo

        if(currentCombo % streakIncrement == 0)
        {
            currentMultiplier += 1;
        }
    }

    public void IncreaseScore(int points)
    {
        score += points * currentMultiplier;
    }

    public void ResetCombo()
    {
        if(currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }

        currentCombo = 0; //reset combo
        currentMultiplier = 1; //reset multiplier
    }
}
