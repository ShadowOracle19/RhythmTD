using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextColorChange : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Color selectedColor;

    private void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = selectedColor;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

}
