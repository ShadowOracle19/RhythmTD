using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightLoadoutManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // Level Info
    [Header("Character Info")]
    public string charaName;
    public string charaInstrument;
    public string charaCooldown;
    public string charaRhythm;
    public int costNum;
    public string upgradeDesc01;
    public string upgradeDesc02;
    public string upgradeDesc03;
    public Sprite upgradeIcon01;
    public Sprite upgradeIcon02;
    public Sprite upgradeIcon03;
    public Sprite upgradeLockedIcon;
    public Sprite rhythmPreview;
    public Sprite rangePreview;

    [Header("Character Info Panel Connections")]
    public GameObject charaInfoPanel;
    public TextMeshProUGUI charaNameText;
    public TextMeshProUGUI charaInstrumentText;
    public TextMeshProUGUI charaCooldownText;
    public TextMeshProUGUI charaRhythmText;
    public TextMeshProUGUI upgradeText01;
    public TextMeshProUGUI upgradeText02;
    public TextMeshProUGUI upgradeText03;
    public Image rangePreviewImage;
    public Image rhythmPreviewImage;
    public Image upgradeImage01;
    public Image upgradeImage02;
    public Image upgradeImage03;
    public List<Image> costImages;
    private int imageIndex = 0;

    public void OnSelect(BaseEventData eventData)
    {
        charaInfoPanel.SetActive(true);
        
        charaNameText.text = charaName;

        charaInstrumentText.text = charaInstrument;
        charaCooldownText.text = charaCooldown;
        charaRhythmText.text = charaRhythm;
        
        upgradeText01.text = upgradeDesc01;
        upgradeText02.text = upgradeDesc02;
        upgradeText03.text = upgradeDesc03;

        upgradeImage01.sprite = upgradeIcon01;
        upgradeImage02.sprite = upgradeIcon02;
        upgradeImage03.sprite = upgradeIcon03;

        imageIndex = 1;
        
        foreach(Image image in costImages)
        {
            if (imageIndex <= costNum) 
            {
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }

            imageIndex += 1;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        charaInfoPanel.SetActive(false);
    }
}
