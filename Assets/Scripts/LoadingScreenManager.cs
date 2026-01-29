using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public TextMeshProUGUI toolTips;
    public GameObject loadingScreen;
    public GameObject loadingScreenVisual;
    public Animator animator;
    public bool transitionActive = false;
    public float transitionTimer = 0.0f;
    
    public void StartLoading()
    {
        // Enable loading screen visuals
        loadingScreen.SetActive(true);
        loadingScreenVisual.SetActive(true);

        // Trigger opening transition
        animator.ResetTrigger("Load End");
        animator.SetTrigger("Load Start");

        StartCoroutine(StartTransition(1.5f));
    }

    public void EndLoading()
    {
        // Trigger closing transition
        animator.ResetTrigger("Load Start");
        animator.SetTrigger("Load End");
        
        StartCoroutine(EndTransition(1.5f));
    }

    public IEnumerator StartTransition(float timerLength)
    {
        transitionActive = true;
        
        transitionTimer = 0.0f;
        
        while (transitionTimer <= timerLength)
        {
            yield return null;
            transitionTimer += Time.deltaTime;
        }

        transitionActive = false;
    }
    
    public IEnumerator EndTransition(float timerLength)
    {
        while (transitionActive)
        {
            yield return null;
        }

        transitionTimer = 0.0f;
        
        while (transitionTimer <= timerLength)
        {
            yield return null;
            transitionTimer += Time.deltaTime;
        }

        loadingScreen.SetActive(false);
        loadingScreenVisual.SetActive(false);
    }
}
