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
    
    public void StartLoading()
    {
        loadingScreen.SetActive(true);
    }

    public void EndLoading()
    {
        loadingScreen.SetActive(false);
    }
}
