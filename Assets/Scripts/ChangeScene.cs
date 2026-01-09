using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MainScenePlay()
    {
        SceneManager.LoadScene("Game Scene");
    }
    
    public void MainMenuLoad()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        // if in combat return to main menu
        if(GameManager.Instance.combatRoot.activeSelf || GameManager.Instance.dialogueRoot.activeSelf)
        {
            if (GameManager.Instance.currentEncounter.isShowcase)
            {
                CombatManager.Instance.EndEncounter();
                GameManager.Instance.combatRoot.SetActive(false);
                GameManager.Instance.titleRoot.SetActive(true);
                GameManager.Instance.ResumeGame();
                return;
            }

            CombatManager.Instance.EndEncounter();
            GameManager.Instance.combatRoot.SetActive(false);
            GameManager.Instance.menuRoot.SetActive(true);
            GameManager.Instance.ResumeGame();
        }
        // if in main menu return to title menu
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            GameManager.Instance.menuRoot.SetActive(false);
            GameManager.Instance.titleRoot.SetActive(true);
            GameManager.Instance.titleRoot.GetComponent<Animator>().SetTrigger("Return To Title");
            GameManager.Instance.ResumeGame();
        }
        // if in title menu close application
        else
        {
            Application.Quit();
        }

    }

    

    //[MenuItem("SonorantStudios/Scenes/Main Menu")]
    //static void LoadMainMenuScene()
    //{
    //    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //    {
    //        EditorSceneManager.OpenScene($"Assets/Scenes/Main Menu.unity", OpenSceneMode.Single);
    //    }
    //}

    //[MenuItem("SonorantStudios/Scenes/Game Scene")]
    //static void LoadBuildScene()
    //{
    //    if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    //    {
    //        EditorSceneManager.OpenScene($"Assets/Scenes/Game Scene.unity", OpenSceneMode.Single);
    //    }
    //}




}
