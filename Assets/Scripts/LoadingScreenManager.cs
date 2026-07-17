using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    #region dont touch this
    private static LoadingScreenManager _instance;
    public static LoadingScreenManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("LoadingScreenManager is NULL");
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
    public TextMeshProUGUI loadingText;
    public string[] loadingTextArray = new string[3];
    public int loadingTextIndex = 0;
    public bool loading = false;
    
    public TextMeshProUGUI toolTips;
    public List<string> toolTipsList = new List<string>();

    public Image artwork;
    public List<Sprite> artworkList = new List<Sprite>();

    public GameObject loadingScreen;
    public GameObject loadingScreenVisual;
    
    //public GameObject prevScreen;
    //public GameObject nextScreen;

    public Animator animator;
    public bool transitionActive = false;
    public float transitionTimer = 0.0f;
    #endregion
    
    public IEnumerator StartLoading()
    {
        // Enable loading screen visuals
        loadingScreen.SetActive(true);
        loadingScreenVisual.SetActive(true);

        // Set new text & visuals
        SetArtwork();
        SetToolTips();

        loading = true;
        StartCoroutine(UpdateLoadingText());

        // Trigger opening transition
        animator.ResetTrigger("Load End");
        animator.SetTrigger("Load Start");

        transitionActive = true;
        transitionTimer = 0.0f;
        
        while (transitionTimer <= 1.5f)
        {
            yield return null;
            transitionTimer += Time.deltaTime;
        }

        transitionActive = false;

        MenuEventManager.Instance.CloseMainMenu();
        GameManager.Instance.LoadCombatScene();
        GameManager.Instance.ResetCombatState(); 

        // Start loading process
        if (GameManager.Instance.currentEncounter.introDialogue != null)
        {
            MenuEventManager.Instance.DialogueOpen();
            DialogueManager.Instance.LoadDialogue(GameManager.Instance.currentEncounter.introDialogue);
        }
        else
        {
            MenuEventManager.Instance.OpenLoadoutMenu();
        } 

        StopCoroutine(StartLoading());
    }

    public void EndLoading()
    {
        loading = false;
        StopCoroutine(UpdateLoadingText());
        loadingText.text = "READY TO ROCK!";
        
        // Trigger closing transition
        animator.ResetTrigger("Load Start");
        animator.SetTrigger("Load End");
        
        StartCoroutine(EndTransition(1.5f));
    }
    
    public IEnumerator EndTransition(float timerLength)
    {
        //prevents end transition from running while start transition is still active
        while (transitionActive)
        {
            yield return null;
        }

        transitionTimer = 0.0f;
        
        while (transitionTimer <= timerLength)
        {
            transitionTimer += Time.deltaTime;
            yield return null;
        }

        loadingScreen.SetActive(false);
        loadingScreenVisual.SetActive(false);

        StopCoroutine(EndTransition(1.5f));
    }

    public void SetArtwork()
    {
        artwork.sprite = artworkList[Random.Range(0, artworkList.Count-1)];
    }

    public void SetToolTips()
    {
        toolTips.text = toolTipsList[Random.Range(0, toolTipsList.Count-1)];
    }

    public IEnumerator UpdateLoadingText()
    {
        while (loading)
        {
            loadingText.text = loadingTextArray[loadingTextIndex % 3];

            loadingTextIndex += 1;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}
