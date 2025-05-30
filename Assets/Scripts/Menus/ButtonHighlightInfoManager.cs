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

    [Header("Info Panel Connections")]
    public GameObject infoPanel;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelNumText;
    public TextMeshProUGUI objectiveText01;
    public TextMeshProUGUI objectiveText02;
    public TextMeshProUGUI objectiveText03;
    public Image levelPreviewImage;
    public Image objectiveImage01;
    public Image objectiveImage02;
    public Image objectiveImage03;
    public List<Image> intelImages;
    private int imageIndex = 0;

    public void OnSelect(BaseEventData eventData)
    {
        levelNameText.enabled = true;
        infoPanel.SetActive(true);

        levelNameText.text = levelName;
        levelNumText.text = levelNum;

        levelPreviewImage.sprite = levelPreview;
  
        objectiveText01.text = objectiveDesc01;
        objectiveText02.text = objectiveDesc02;
        objectiveText03.text = objectiveDesc03;

        objectiveImage01.sprite = objectiveIncompleteIcon;

        if (objectiveDesc02 != "")
        {
            objectiveImage02.enabled = true;

            objectiveImage02.sprite = objectiveIncompleteIcon;
        }
        else
        {
            objectiveImage02.enabled = false;
        }

        if (objectiveDesc03 != "")
        {
            objectiveImage03.enabled = true;

            objectiveImage03.sprite = objectiveIncompleteIcon;
        }
        else 
        {
            objectiveImage03.enabled = false;
        }

        imageIndex = 0;

        foreach(Image image in intelImages)
        {
            if (imageIndex > (intelIcons.Count-1))
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = intelIcons[imageIndex];
                image.enabled = true;
            }

            imageIndex += 1;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        levelNameText.enabled = false;
        infoPanel.SetActive(false);
    }
}
