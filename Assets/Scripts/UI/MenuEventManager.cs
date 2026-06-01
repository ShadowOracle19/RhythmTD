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
    public AudioSource menuMusic;

    [Header("Loadout Menu")]
    public GameObject loadoutScreen;
    public List<GameObject> loadoutScreenInteractables;
    public Animator loadoutInterfaceAnimator;

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
    public Animator cameraAnimator;
    public Animator combatInterfaceAnimator;
    public GameObject showcaseCreditsScreen;
    public List<GameObject> showcaseCreditsScreenInteractables;


    private void Start()
    {
        eventSystem = EventSystem.current;
        
    }

    #region active object tracking
    public void UpdateLastSelectedObject()
    {
        lastSelectedObject = eventSystem.currentSelectedGameObject;
    }

    public void SelectLastSelectedObject()
    {
        eventSystem.SetSelectedGameObject(lastSelectedObject);
    }
    
    // Updates a reference to the most recent level the player entered
    public void UpdateLastSelectedLevel()
    {
        lastSelectedLevelObject = eventSystem.currentSelectedGameObject;
    }
    
    // Sets the currently selected menu element to the button of the most recent level the player entered
    public void SelectLastSelectedLevel()
    {
        eventSystem.SetSelectedGameObject(lastSelectedLevelObject);
    }
    #endregion
    
    #region main menu
    public void OpenMainMenu()
    {
        cameraAnimator.SetTrigger("Enter Menu");

        mainScreen.SetActive(true); //enable main menu (level select)

        if (lastSelectedLevelObject == null) 
        {
            eventSystem.SetSelectedGameObject(mainScreenInteractables[0]);
        }
        else
        {
            SelectLastSelectedLevel();
        }

        menuMusic.Play();
    }

    public void CloseMainMenu()
    {
        menuMusic.Stop();

        mainScreen.SetActive(false);
    }
    #endregion

    #region loadout menu
    public void OpenLoadoutMenu()
    {

        loadoutScreen.SetActive(true); //enable loadout menu

        cameraAnimator.SetTrigger("Enter Loadout");

        

        eventSystem.SetSelectedGameObject(loadoutScreenInteractables[0]);

        LoadingScreenManager.Instance.EndLoading(); //TEMPORARY
    }

    public void CloseLoadoutMenu()
    {
        loadoutScreen.SetActive(false);
    }

    public void ConfirmLoadout()
    {
        StartCoroutine(CombatStartSequence());
    }

    //Combat start sequence
    public IEnumerator CombatStartSequence()
    {
        TowerManager.Instance.ResetTowerManager();
        TowerManager.Instance.InstantiateTowerCooldown();
        
        bool animating = true;

        //Closing Loadout UI Animation
        //loadoutInterfaceAnimator.SetTrigger("Close Loadout");
        
        while (animating)
        {
            animating = false;

            yield return new WaitForSecondsRealtime(1.0f);
        }

        //Combat Start Sequence Animation
        cameraAnimator.SetTrigger("Combat Start");
        animating = true;

        while (animating)
        {
            animating = false;

            yield return new WaitForSecondsRealtime(8.0f);
        }

        cameraAnimator.ResetTrigger("Enter Menu");
        cameraAnimator.SetTrigger("Combat Idle");
        
        //CombatManager.Instance.combatInterface.SetActive(true);
        CombatManager.Instance.countInObject.SetActive(false);

        //Combat UI Opening Animation
        combatInterfaceAnimator.SetTrigger("Combat Start");
        animating = true;

        while (animating)
        {
            animating = false;

            yield return new WaitForSecondsRealtime(3.0f);
        }

        CombatManager.Instance.countInObject.SetActive(false);
        GameManager.Instance.StartCombat();

        StopCoroutine(CombatStartSequence());
    }
    #endregion

    #region settings menu
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
            eventSystem.SetSelectedGameObject(titleScreenInteractables[2]);
        }

        settingsScreen.SetActive(false);
    }
    #endregion

    #region exit confirmation menu
    //Open exit confirmation menu & set text
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

    //Close exit confirmation menu
    public void CloseConfirmation()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseScreenInteractables[1]);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleScreenInteractables[4]);
        }

        exitScreen.SetActive(false);
    }

    public void QuitGame()
    {
        // if in combat return to main menu
        if(GameManager.Instance.combatRoot.activeSelf || GameManager.Instance.dialogueRoot.activeSelf)
        {
            /*
            if (GameManager.Instance.currentEncounter.isShowcase)
            {
                CombatManager.Instance.EndEncounter();
                GameManager.Instance.combatRoot.SetActive(false);
                GameManager.Instance.titleRoot.SetActive(true);
                GameManager.Instance.ResumeGame();
                return;
            }
            */

            CombatManager.Instance.EndEncounter();
            GameManager.Instance.combatRoot.SetActive(false);
            OpenMainMenu();
            GameManager.Instance.ResumeGame(); //unpause
        }
        // if in main menu return to title menu
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            CloseMainMenu();
            OpenMenu(titleScreen, titleScreenInteractables[1]);
            GameManager.Instance.titleRoot.GetComponent<Animator>().SetTrigger("Return To Title");
            GameManager.Instance.ResumeGame(); //unpause
        }
        // if in title menu close application
        else
        {
            Application.Quit();
        }

    }
    #endregion

    #region dialogue menu
    public void DialogueOpen()
    {
        dialogueScreen.SetActive(true);
    }

    public void DialogueClose()
    {
        dialogueScreen.SetActive(false);
    }

    public void OpenLog()
    {
        OpenMenu(logScreen, logScreenInteractables[0]);
    }
    #endregion

    #region pause menu
    public void PauseMenuOpen()
    {
        OpenMenu(pauseScreen, pauseScreenInteractables[0]);
    }

    public void PauseMenuClose()
    {
        //set title menu active object if in title menu
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleScreenInteractables[1]);
        }
        //set main menu active object if in main menu
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            if (lastSelectedLevelObject == null) 
            {
                eventSystem.SetSelectedGameObject(mainScreenInteractables[0]);
            }
            else
            {
                SelectLastSelectedLevel();
            }
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
    #endregion
    
    #region game end menus
    // Open the fail screen 
    public void OpenFailScreen()
    {
        OpenMenu(failScreen, failScreenInteractables[0]);
    }

    public void CloseFailScreen()
    {
        GameManager.Instance.ResetCombatState();
        //failScreen.SetActive(false);
    }

    public void OpenWinScreen()
    {
        OpenMenu(winScreen, winScreenInteractables[0]);
    }

    public void CloseWinScreen()
    {
        GameManager.Instance.ResetCombatState();
        
        //winScreen.SetActive(false);
    }
    #endregion

    /*
    public void OpenShowcaseCredits()
    {
        OpenMenu(showcaseCreditsScreen, showcaseCreditsScreenInteractables[0]);
    }
    */

    public void OpenMenu(GameObject menuRoot, GameObject activeObject)
    {
        //menuMusic.Play();
        menuRoot.SetActive(true); //enable root
        eventSystem.SetSelectedGameObject(activeObject); //set active object
    }

    public void WinScreenToLevelSelect()
    {
        eventSystem.SetSelectedGameObject(GameManager.Instance.currentSelectedButton);
    }

    /*
    void OnSelect(BaseEventData eventData) 
    {

    }
    */
}
