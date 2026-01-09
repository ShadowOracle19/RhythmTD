using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class MenuEventManager : MonoBehaviour
{
    #region dont touch this
    private static MenuEventManager _instance;
    public static MenuEventManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("MenuEventManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion
    EventSystem eventSystem;

    [Header("Last Selected Object")]
    public GameObject lastSelectedObject;



    [Header("Title Menu")]
    public GameObject titleMenuObject01;
    public GameObject titleMenuObject02;
    public GameObject titleMenuObject03;
    public GameObject titleMenuObject04;
    public GameObject titleMenuObject05;
    public GameObject titleMenuObject06;

    [Header("Pause Menu")]
    public GameObject pauseMenuFirstObject;
    public GameObject pauseMenuSecondObject;

    [Header("Dialogue Menu")]
    public GameObject dialogueMenuFirstObject;
    public GameObject logFirstObject;

    [Header("Main Menu")]
    public GameObject mainMenuFirstObject;

    [Header("Win Menu")]
    public GameObject winScreenFirstObject;
    //public GameObject winScreenSecondObject;

    [Header("Lose Menu")]
    public GameObject loseScreenObject;

    [Header("Exit Confirmation Menu")]
    public GameObject exitConfirmationObject;
    public TextMeshProUGUI exitConfirmationText;
    
    [Header("Other")]
    public GameObject showcaseCreditsObject;



    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void RecordLastSelectedObject()
    {
        lastSelectedObject = eventSystem.currentSelectedGameObject;
    }

    public void SelectLastSelectedObject()
    {
        eventSystem.SetSelectedGameObject(lastSelectedObject);
    }

    public void OpenLog()
    {
        eventSystem.SetSelectedGameObject(logFirstObject);
    }

    public void CloseSettings()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseMenuFirstObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuObject02);
        }
    }

    // open exit confirmation menu and set text
    public void OpenConfirmation()
    {
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            exitConfirmationText.text = "Quit to desktop?";
        }
        else if (GameManager.Instance.menuRoot.activeSelf)
        {
            exitConfirmationText.text = "Exit to title screen?";
        }
        else if (GameManager.Instance.combatRoot.activeSelf)
        {
            exitConfirmationText.text = "Exit to stage select?";

            if (GameManager.Instance.currentEncounter.isShowcase)
            {
                exitConfirmationText.text = "Exit to title screen?";
            }
        }

        eventSystem.SetSelectedGameObject(exitConfirmationObject);
    }

    // close exit confirmation menu & set text
    public void CloseConfirmation()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseMenuSecondObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuObject04);
        }
        /*
        else if(GameManager.Instance.winScreen.activeSelf)
        {
            eventSystem.SetSelectedGameObject(winScreenSecondObject);
        }
        */
    }

    public void DialogueOpen()
    {
        eventSystem.SetSelectedGameObject(dialogueMenuFirstObject);
    }

    public void PauseMenuOpen()
    {
        eventSystem.SetSelectedGameObject(pauseMenuFirstObject);
    }

    public void PauseMenuClose()
    {
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuObject01);
        }
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(mainMenuFirstObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuObject01);
        }
        else if(GameManager.Instance.dialogueRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(dialogueMenuFirstObject);
        }
        else if(GameManager.Instance.combatRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }

    public void LoseScreenOpen()
    {
        eventSystem.SetSelectedGameObject(loseScreenObject);
    }

    public void WinScreenOpen()
    {
        eventSystem.SetSelectedGameObject(winScreenFirstObject);
    }

    public void OpenShowcaseCredits()
    {
        eventSystem.SetSelectedGameObject(showcaseCreditsObject);
    }

    /*
    void OnSelect(BaseEventData eventData) 
    {

    }
    */
}
