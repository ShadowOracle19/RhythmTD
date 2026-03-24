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

    [Header("Active Object Tracking")]
    public GameObject lastSelectedObject;
    public GameObject lastSelectedLevelObject;

    [Header("Title Menu")]
    public GameObject titleScreen;
    public List<GameObject> titleScreenInteractables;

    [Header("Pause Menu")]
    public GameObject pauseScreen;
    public List<GameObject> pauseScreenInteractables;

    [Header("Settings Menu")]
    public GameObject settingsScreen;
    public List<GameObject> settingsScreenInteractables;

    [Header("Dialogue Menu")]
    public GameObject dialogueScreen;
    public List<GameObject> dialogueScreenInteractables;

    public GameObject logScreen;
    public List<GameObject> logScreenInteractables;

    [Header("Main Menu")]
    public GameObject mainScreen;
    public List<GameObject> mainScreenInteractables;

    [Header("Win Screen")]
    public GameObject winScreen;
    public List<GameObject> winScreenInteractables;

    [Header("Fail Screen")]
    public GameObject failScreen;
    public List<GameObject> failScreenInteractables;

    [Header("Exit Confirmation Menu")]
    public GameObject exitScreen;
    public List<GameObject> exitScreenInteractables;
    public TextMeshProUGUI exitMenuText;
    
    [Header("Other")]
    public GameObject showcaseCreditsScreen;
    public List<GameObject> showcaseCreditsScreenInteractables;


    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void UpdateLastSelectedObject()
    {
        lastSelectedObject = eventSystem.currentSelectedGameObject;
    }

    // Updates a reference to the most recent level the player entered
    public void UpdateLastSelectedLevel()
    {
        lastSelectedLevelObject = eventSystem.currentSelectedGameObject;
    }

    public void SelectLastSelectedObject()
    {
        eventSystem.SetSelectedGameObject(lastSelectedObject);
    }

    // Sets the currently selected menu element to the button of the most recent level the player entered
    public void SelectLastSelectedLevel()
    {
        eventSystem.SetSelectedGameObject(lastSelectedLevelObject);
    }

    public void OpenMainMenu()
    {
        mainScreen.SetActive(true); //enable main menu (level select)

        if (lastSelectedLevelObject == null) 
        {
            eventSystem.SetSelectedGameObject(winScreenInteractables[0]);
        }
        else
        {
            SelectLastSelectedLevel();
        }

        //SET UP NEW AUDIO MANAGER FOR THIS AND CALL METHODS FROM IT
        //stop music 
        //start new music
    }

    public void CloseMainMenu()
    {
        //SET UP NEW AUDIO MANAGER FOR THIS AND CALL METHODS FROM IT
        //stop music
        
        UpdateLastSelectedLevel();

        mainScreen.SetActive(false);
    }

    public void CloseSettings()
    {
        //
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseScreenInteractables[0]);
        }
        //
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleScreenInteractables[1]);
        }

        settingsScreen.SetActive(false);
    }

    // open exit confirmation menu & set text
    public void OpenConfirmation()
    {
        exitScreen.SetActive(true);
        
        //set exit confirmation text
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            exitMenuText.text = "Quit to desktop?";
        }
        else if (GameManager.Instance.menuRoot.activeSelf)
        {
            exitMenuText.text = "Exit to title screen?";
        }
        else if (GameManager.Instance.combatRoot.activeSelf)
        {
            exitMenuText.text = "Exit to stage select?";

            /*
            if (GameManager.Instance.currentEncounter.isShowcase)
            {
                exitMenuText.text = "Exit to title screen?";
            }
            */
        }

        eventSystem.SetSelectedGameObject(exitScreenInteractables[0]);
    }

    // close exit confirmation menu
    public void CloseConfirmation()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseScreenInteractables[1]);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleScreenInteractables[3]);
        }

        exitScreen.SetActive(false);
    }

    public void DialogueOpen()
    {
        OpenMenu(dialogueScreen, dialogueScreenInteractables[0]);
    }

    public void OpenLog()
    {
        OpenMenu(logScreen, logScreenInteractables[0]);
    }

    public void PauseMenuOpen()
    {
        OpenMenu(pauseScreen, pauseScreenInteractables[0]);
    }

    public void PauseMenuClose()
    {
        //set title menu active object if in title menu
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleScreenInteractables[0]);
        }
        //set main menu active object if in main menu
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(mainScreenInteractables[0]);
        }
        //set dialogue active object if in dialogue scene
        else if(GameManager.Instance.dialogueRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(dialogueScreenInteractables[0]);
        }
        //set no active object if in combat
        else if(GameManager.Instance.combatRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(null);
        }

        pauseScreen.SetActive(false);
    }

    // Open the fail screen 
    public void OpenFailScreen()
    {
        OpenMenu(failScreen, failScreenInteractables[0]);
    }

    public void OpenWinScreen()
    {
        OpenMenu(winScreen, winScreenInteractables[0]);
    }

    /*
    public void OpenShowcaseCredits()
    {
        OpenMenu(showcaseCreditsScreen, showcaseCreditsScreenInteractables[0]);
    }
    */

    public void OpenMenu(GameObject menuRoot, GameObject activeObject)
    {
        menuRoot.SetActive(true); //enable root
        eventSystem.SetSelectedGameObject(activeObject); //set active object
    }

    /*
    void OnSelect(BaseEventData eventData) 
    {

    }
    */
}
