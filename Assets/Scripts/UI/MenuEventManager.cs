using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;


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

    #region Variables
    EventSystem eventSystem;

    [Space(20)][Header("<b><size=15>Active Object Tracking<b><size=15>")]
    [Line(255,255,255)]
    public GameObject lastSelectedObject;
    public GameObject lastSelectedLevelObject;

    [Space(20)][Header("<b><size=15>Title Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject titleScreen;
    public List<GameObject> titleScreenInteractables;
    public AudioSource titleMusic;

    [Space(20)][Header("<b><size=15>Pause Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject pauseScreen;
    public List<GameObject> pauseScreenInteractables;

    [Space(20)][Header("<b><size=15>Settings Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject settingsScreen;
    public List<GameObject> settingsScreenInteractables;

    [Space(20)][Header("<b><size=15>Dialogue Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject dialogueScreen;
    public List<GameObject> dialogueScreenInteractables;
    [Space(10)]
    public GameObject logScreen;
    public List<GameObject> logScreenInteractables;

    [Space(20)][Header("<b><size=15>Main Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject mainScreen;
    public List<GameObject> mainScreenInteractables;
    [Space(10)]
    public bool isModMenuOpen = false;
    public GameObject modifierScreenObject;
    public Animator modifierScreenAnimator;
    [Space(10)]
    public AudioSource menuMusic;

    [Space(20)][Header("<b><size=15>Loadout Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject loadoutScreen;
    public List<GameObject> loadoutScreenInteractables;
    public Animator loadoutInterfaceAnimator;

    [Space(20)][Header("<b><size=15>Win Screen<b><size=15>")]
    [Line(255,255,255)]
    public GameObject winScreen;
    public List<GameObject> winScreenInteractables;

    [Space(20)][Header("<b><size=15>Fail Screen<b><size=15>")]
    [Line(255,255,255)]
    public GameObject failScreen;
    public List<GameObject> failScreenInteractables;

    [Space(20)][Header("<b><size=15>Exit Confirmation Menu<b><size=15>")]
    [Line(255,255,255)]
    public GameObject exitScreen;
    public List<GameObject> exitScreenInteractables;
    public TextMeshProUGUI exitMenuText;
    
    [Space(20)][Header("<b><size=15>Other<b><size=15>")]
    [Line(255,255,255)]
    public Animator cameraAnimator;
    public Animator combatInterfaceAnimator;
    #endregion

    #region Start
    private void Start()
    {
        eventSystem = EventSystem.current;
    }
    #endregion

    #region Object selection
    // Gets a reference to the most recently selected menu element
    public void UpdateLastSelectedObject()
    {
        lastSelectedObject = eventSystem.currentSelectedGameObject;
    }

    // Sets the active menu element to the last selected menu element
    public void SelectLastSelectedObject()
    {
        eventSystem.SetSelectedGameObject(lastSelectedObject);
    }
    
    // Gets a reference to the button of the last selected level
    public void UpdateLastSelectedLevel()
    {
        lastSelectedLevelObject = eventSystem.currentSelectedGameObject;
    }
    
    // Sets the active menu element to the button of the most recent level the player entered / Highlighted
    public void SelectLastSelectedLevel()
    {
        eventSystem.SetSelectedGameObject(lastSelectedLevelObject);
    }
    #endregion
    
    #region Level select
    // Opens the level select menu
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

    // Closes the level select menu
    public void CloseMainMenu()
    {
        menuMusic.Stop();

        mainScreen.SetActive(false);
    }

    // Opens and closes the modifier menu in the level select menu
    public void HandleModifierMenuInput()
    {
        if (!isModMenuOpen) //if modifier menu is closed, open it
        {
            modifierScreenAnimator.SetTrigger("Open");
            eventSystem.SetSelectedGameObject(modifierScreenObject);
            isModMenuOpen = true;
        }
        else //if modifier menu is open, close it
        {
            modifierScreenAnimator.SetTrigger("Close");
            SelectLastSelectedLevel();
            isModMenuOpen = false;
        }
    }
    #endregion

    #region Loadout
    // Opens the loadout menu
    public void OpenLoadoutMenu()
    {
        loadoutScreen.SetActive(true); //enable loadout menu

        cameraAnimator.SetTrigger("Enter Loadout");

        eventSystem.SetSelectedGameObject(loadoutScreenInteractables[0]);

        LoadingScreenManager.Instance.EndLoading(); //TEMPORARY
    }

    // Closes the loadout menu
    public void CloseLoadoutMenu()
    {
        loadoutScreen.SetActive(false);
    }
    #endregion

    #region Combat sequence
    // Starts the combat start sequence
    public void StartCombatStartSequence()
    {
        StartCoroutine(CombatStartSequence());
    }

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

    #region Settings
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

    #region Exit confirmation
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
            titleMusic.Play();
        }
        // if in title menu close application
        else
        {
            Application.Quit();
        }

    }
    #endregion

    #region Dialogue
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

    #region Pause
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
    
    #region Game end
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

    public void WinScreenToLevelSelect()
    {
        eventSystem.SetSelectedGameObject(GameManager.Instance.currentSelectedButton);
    }
    #endregion

    #region General
    public void OpenMenu(GameObject menuRoot, GameObject activeObject)
    {
        //menuMusic.Play();
        menuRoot.SetActive(true); //enable root
        eventSystem.SetSelectedGameObject(activeObject); //set active object
    }
    #endregion
}
