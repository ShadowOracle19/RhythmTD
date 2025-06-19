using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ItemButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text _itemName;
    public EncounterCreator heldEncounter;
    public Image fill;

    [SerializeField] private ItemButtonEvent _onSelectEvent;
    [SerializeField] private ItemButtonEvent _onSubmitEvent;

    public int viewportOffset = 0;

    public ItemButtonEvent OnSelectEvent { get => _onSelectEvent; set => _onSelectEvent = value; }

    public ItemButtonEvent OnSubmitEvent { get => _onSubmitEvent; set => _onSubmitEvent = value; }

    public string ItemNameValue { get => _itemName.text; set => _itemName.text = value; }

    private void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 800);
        _onSelectEvent.Invoke(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _onSubmitEvent.Invoke(this);
    }

    public void ObtainSelectFocus()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        _onSelectEvent.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 700);
    }
}

[System.Serializable]
public class ItemButtonEvent : UnityEvent<ItemButton>
{

}
