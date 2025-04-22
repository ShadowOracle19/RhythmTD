using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region dont touch this
    private static TutorialManager _instance;
    public static TutorialManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("TutorialManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public GameObject tutorialPopup;
    public TextMeshProUGUI tutorialText;
    public bool pressKeyToNextDialogue = false;

    public GameObject magicBarArrow;
    public GameObject metronomeArrows;
    public GameObject cursorArrows;
    public GameObject cursorMovementGlyphs;
    public GameObject menuOpenGlyph;

    public List<TutorialDialogue> tutorialDialogue = new List<TutorialDialogue>();

    [SerializeField]private GameObject pressKeyToNextPopup;
    public int index = 0;

    [Header("Showcase")]
    public bool spawnEnemies = false;

    public void LoadTutorial()
    {
        tutorialPopup.SetActive(true);
        index = 0;
        tutorialText.text = tutorialDialogue[index].text;
        pressKeyToNextDialogue = tutorialDialogue[index].isSkippableText;
    }


    // Update is called once per frame
    void Update()
    {
        pressKeyToNextPopup.SetActive(pressKeyToNextDialogue);

        if(pressKeyToNextDialogue && Input.GetKeyDown(KeyCode.Return) && !ConductorV2.instance.countingIn)
        {
            LoadNextTutorialDialogue();
        }

        if (index == 1)
        {
            cursorMovementGlyphs.SetActive(true);
        }
        else
        {
            cursorMovementGlyphs.SetActive(false);
        }

        if (index == 2)
        {
            metronomeArrows.SetActive(true);
            cursorArrows.SetActive(true);
        }
        else
        {
            metronomeArrows.SetActive(false);
            cursorArrows.SetActive(false);
        }

        if (index == 4)
        {
            magicBarArrow.SetActive(true);
        }
        else
        {
            
            magicBarArrow.SetActive(false);
        }

        if (index == 5)
        {
            menuOpenGlyph.SetActive(true);
        }
        else
        {
            
            menuOpenGlyph.SetActive(false);
        }
      
    }


    public void LoadNextTutorialDialogue()
    {
        index += 1; 
        if (index > tutorialDialogue.Count - 1)
        {
            tutorialPopup.SetActive(false);
            pressKeyToNextDialogue = false;
            CombatManager.Instance.LoadEncounter(GameManager.Instance.currentEncounter.combatEncounter);
            //GameManager.Instance.tutorialRunning = false;
            CombatManager.Instance.enemyTimerObject.SetActive(true);
            CombatManager.Instance.healthBar.SetActive(true);
            CombatManager.Instance.controls.SetActive(true);
            CombatManager.Instance.resources.SetActive(true);
            CombatManager.Instance.towerDisplay.SetActive(true);
            CombatManager.Instance.feverBar.SetActive(true);
            CombatManager.Instance.metronome.SetActive(true);
            CombatManager.Instance.waveCounter.SetActive(true);
            CombatManager.Instance.combo.SetActive(true);
            return;
            
        }
        tutorialText.text = tutorialDialogue[index].text;
        pressKeyToNextDialogue = tutorialDialogue[index].isSkippableText;

        
    }

    public void LoadPrevTutorialDialogue()
    {
        index -= 1;
        tutorialText.text = tutorialDialogue[index].text;
        pressKeyToNextDialogue = tutorialDialogue[index].isSkippableText;
    }
}

[System.Serializable]
public class TutorialDialogue
{
    [TextArea(5, 20)]
    public string text;
    public bool isSkippableText;
}
