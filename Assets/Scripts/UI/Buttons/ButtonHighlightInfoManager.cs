using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightInfoManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // Level Info
    [Header("<b><size=15>Encounter Info<b><size=15>")]
    [Line(255,255,255)]
    public ItemButton itemButton;

    [Space(20)][Header("<b><size=15>Encounter Info<b><size=15>")]
    [Line(255,255,255)]
    public string levelName;
    public string levelNum;
    public string objectiveDesc01;
    public string objectiveDesc02;
    public string objectiveDesc03;
    public Sprite levelPreview;
    public Sprite objectiveIncompleteIcon;
    public Sprite objectiveCompleteIcon;
    public List<Sprite> intelIcons;

    #region Start
    private void Start()
    {
        itemButton = GetComponent<ItemButton>();
        
        levelName = itemButton.heldEncounter.encounterName;
        levelNum = itemButton.heldEncounter.LevelLabel;
        objectiveDesc01 = itemButton.heldEncounter.data.objectiveDesc01;
        objectiveDesc02 = itemButton.heldEncounter.data.objectiveDesc02;
        objectiveDesc03 = itemButton.heldEncounter.data.objectiveDesc03;
        levelPreview = itemButton.heldEncounter.data.levelPreview;
        objectiveIncompleteIcon = itemButton.heldEncounter.data.objectiveIncompleteIcon;
        objectiveCompleteIcon = itemButton.heldEncounter.data.objectiveCompleteIcon;

        intelIcons = itemButton.heldEncounter.data.intelIcons;
    }
    #endregion

    #region Select
    public void OnSelect(BaseEventData eventData)
    {
        GameManager.Instance.levelNameText.enabled = true;
        GameManager.Instance.infoPanel.SetActive(true);

        GameManager.Instance.levelNameText.text = levelName;
        GameManager.Instance.levelNumText.text = levelNum;

        GameManager.Instance.levelPreviewImage.sprite = levelPreview;

        GameManager.Instance.objectiveText01.text = objectiveDesc01;
        GameManager.Instance.objectiveText02.text = objectiveDesc02;
        GameManager.Instance.objectiveText03.text = objectiveDesc03;

        GameManager.Instance.objectiveImage01.sprite = objectiveIncompleteIcon;

        if (objectiveDesc02 != "")
        {
            GameManager.Instance.objectiveImage02.enabled = true;

            GameManager.Instance.objectiveImage02.sprite = objectiveIncompleteIcon;
        }
        else
        {
            GameManager.Instance.objectiveImage02.enabled = false;
        }

        if (objectiveDesc03 != "")
        {
            GameManager.Instance.objectiveImage03.enabled = true;

            GameManager.Instance.objectiveImage03.sprite = objectiveIncompleteIcon;
        }
        else 
        {
            GameManager.Instance.objectiveImage03.enabled = false;
        }

        GameManager.Instance.imageIndex = 0;

        foreach(Image image in GameManager.Instance.intelImages)
        {
            if (GameManager.Instance.imageIndex > (intelIcons.Count-1))
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = intelIcons[GameManager.Instance.imageIndex];
                image.enabled = true;
            }

            GameManager.Instance.imageIndex += 1;
        }
    }
    #endregion

    #region Deselect
    public void OnDeselect(BaseEventData eventData)
    {
        GameManager.Instance.levelNameText.enabled = false;
        GameManager.Instance.infoPanel.SetActive(false);
    }
    #endregion
}
