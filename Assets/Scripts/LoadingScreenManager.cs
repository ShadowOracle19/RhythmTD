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

    public TextMeshProUGUI toolTips;
    public List<string> toolTipsList = new List<string>();

    public Image artwork;
    public List<Sprite> artworkList = new List<Sprite>();

    public GameObject loadingScreen;
    public GameObject loadingScreenVisual;

    public GameObject prevScreen;
    public GameObject nextScreen;

    public Animator animator;
    public bool transitionActive = false;
    public float transitionTimer = 0.0f;
    
    public void StartLoading()
    {
        // Enable loading screen visuals
        loadingScreen.SetActive(true);
        loadingScreenVisual.SetActive(true);

        // Set new text & visuals
        SetArtwork();
        SetToolTips();

        // Trigger opening transition
        animator.ResetTrigger("Load End");
        animator.SetTrigger("Load Start");
        
        StartCoroutine(SetTimer(1.5f));
    }

    public void EndLoading()
    {
        // Trigger closing transition
        animator.ResetTrigger("Load Start");
        animator.SetTrigger("Load End");
        
        StartCoroutine(SetTimer(1.5f));
    }

    public IEnumerator SetTimer(float timerLength)
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

    public void SetArtwork()
    {
        artwork.sprite = artworkList[Random.Range(0, artworkList.Count-1)];
    }

    public void SetToolTips()
    {
        toolTips.text = toolTipsList[Random.Range(0, toolTipsList.Count-1)];
    }
}
