using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightInfoManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // Level Info
    [Header("Encounter Info")]
    public string levelName;
    public string levelNum;
    public string objectiveDesc01;
    public string objectiveDesc02;
    public string objectiveDesc03;
    public Sprite levelPreview;
    public Sprite objectiveIncompleteIcon;
    public Sprite objectiveCompleteIcon;
    public List<Sprite> intelIcons;

    private void Start()
    {
        levelName = GetComponent<ItemButton>().heldEncounter.encounterName;
        levelNum = GetComponent<ItemButton>().heldEncounter.LevelLabel;
        objectiveDesc01 = GetComponent<ItemButton>().heldEncounter.data.objectiveDesc01;
        objectiveDesc02 = GetComponent<ItemButton>().heldEncounter.data.objectiveDesc02;
        objectiveDesc03 = GetComponent<ItemButton>().heldEncounter.data.objectiveDesc03;
        levelPreview = GetComponent<ItemButton>().heldEncounter.data.levelPreview;
        objectiveIncompleteIcon = GetComponent<ItemButton>().heldEncounter.data.objectiveIncompleteIcon;
        objectiveCompleteIcon = GetComponent<ItemButton>().heldEncounter.data.objectiveCompleteIcon;

        intelIcons = GetComponent<ItemButton>().heldEncounter.data.intelIcons;
    }

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

    public void OnDeselect(BaseEventData eventData)
    {
        GameManager.Instance.levelNameText.enabled = false;
        GameManager.Instance.infoPanel.SetActive(false);
    }
}
