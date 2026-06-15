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

    [Header ("Score Pulse")]
    public GameObject cursorSprite;
    public Vector3 textDefaultSize = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 textPulseSize = new Vector3(1.0f, 1.25f, 1.0f);

    [Header ("Score Points")]

    [Header ("UI Elements")]
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI scoreText;
    public Transform comboTextParent;
    public Transform multiplierTextParent;
    public Transform scoreTextParent;

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

        comboText.text = currentCombo.ToString();
        multiplierText.text = currentMultiplier.ToString();
        scoreText.text = score.ToString();

    }

    public void IncreaseCombo()
    {
        comboTextParent.localScale = textPulseSize;
        currentCombo += 1; //increase combo
        StartCoroutine(TextPulse(comboTextParent));

        if(currentCombo % streakIncrement == 0)
        {
            multiplierTextParent.localScale = textPulseSize;
            currentMultiplier += 1;
            StartCoroutine(TextPulse(multiplierTextParent));
        }
    }

    public void IncreaseScore(int points)
    {
        scoreTextParent.localScale = textPulseSize;
        score += points * currentMultiplier;

        StartCoroutine(TextPulse(scoreTextParent));
    }

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
