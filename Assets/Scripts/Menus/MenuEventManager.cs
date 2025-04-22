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

    public GameObject pauseMenuFirstObject;
    public GameObject pauseMenuSecondObject;
    public GameObject titleMenuFirstObject;
    public GameObject titleMenuSecondObject;
    public GameObject DialogueMenuFirstObject;
    public GameObject mainMenuFirstObject;
    public GameObject winScreenFirstObject;
    public GameObject winScreenSecondObject;
    public GameObject loseScreenObject;
    public GameObject exitConfirmationObject;
    public GameObject logFirstObject;
    public GameObject showcaseCreditsObject;

    public TextMeshProUGUI exitConfirmationText;

    [Header("Last Selected Object")]
    public GameObject lastSelectedObject;

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
            eventSystem.SetSelectedGameObject(titleMenuFirstObject);
        }
    }

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
            exitConfirmationText.text = "Exit to level select?";

            if (GameManager.Instance.currentEncounter.isShowcase)
            {
                exitConfirmationText.text = "Exit to title screen?";
            }
        }

        eventSystem.SetSelectedGameObject(exitConfirmationObject);
    }

    public void CloseConfirmation()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseMenuSecondObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuSecondObject);
        }
        else if(GameManager.Instance.winScreen.activeSelf)
        {
            eventSystem.SetSelectedGameObject(winScreenSecondObject);
        }
    }

    public void DialogueOpen()
    {
        eventSystem.SetSelectedGameObject(DialogueMenuFirstObject);
    }

    public void PauseMenuOpen()
    {
        eventSystem.SetSelectedGameObject(pauseMenuFirstObject);
    }

    public void PauseMenuClose()
    {
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuFirstObject);
        }
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(mainMenuFirstObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuFirstObject);
        }
        else if(GameManager.Instance.dialogueRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(DialogueMenuFirstObject);
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
