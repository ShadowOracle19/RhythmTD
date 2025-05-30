using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSample : MonoBehaviour
{
    public ItemButton[] children;


    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _prefabListItem;

    [Space(10)]
    [Header("Scroll View Events")]
    [SerializeField] private ItemButtonEvent _eventItemOnSelect;
    [SerializeField] private ItemButtonEvent _eventItemOnSubmit;

    [Space(10)]
    [Header("Default Selected Index")]
    [SerializeField] private int _defaultSelectedIndex = 0;

    [SerializeField] private int _testButtonCount = 1;

    // Start is called before the first frame update
    void Start()
    {

        UpdateAllButtonNavigationalReferences();
        if (_testButtonCount > 0)
        {
            //TestCreateItems(_testButtonCount);
        }
    }

    public void SetupKeys()
    {
    }

    public void SelectChild(int index)
    {
        int childCount = _content.childCount;

        if (index >= childCount)
        {
            
            return; //it is out of range
        }

        GameObject childObject = _content.transform.GetChild(index).gameObject;
        ItemButton item = childObject.GetComponent<ItemButton>();

        item.ObtainSelectFocus();
    }

    public IEnumerator DelayedSelectChild(int index)
    {
        yield return new WaitForSeconds(1f);

        SelectChild(index);
    }

    private void UpdateAllButtonNavigationalReferences()
    {
        children = _content.transform.GetComponentsInChildren<ItemButton>();

        if(children.Length < 2)
        {
            return; //must have at least 2 buttons
        }

        ItemButton item;
        Navigation navigation;

        for (int i = 0; i < children.Length; i++)
        {
            item = children[i];

            navigation = item.gameObject.GetComponent<Button>().navigation;

            navigation.selectOnLeft = GetNavigationUp(i, children.Length);
            navigation.selectOnRight = GetNavigationDown(i, children.Length);

            item.gameObject.GetComponent<Button>().navigation = navigation;

            //add event listeners
            //item.OnSelectEvent.AddListener((ItemButton) => { HandleEventItemOnSelect(item); });
            //item.OnSubmitEvent.AddListener((ItemButton) => { HandleEventItemOnSubmit(item); });
        }

    }

    private Selectable GetNavigationDown(int indexCurrent, int length)
    {
        ItemButton item;

        if (indexCurrent == length - 1) //last item
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            item = _content.transform.GetChild(indexCurrent + 1).GetComponent<ItemButton>();
        }

        return item.GetComponent<Selectable>();
    }

    private Selectable GetNavigationUp(int indexCurrent, int length)
    {
        ItemButton item;

        if(indexCurrent == 0)
        {
            //looping dont set anything here
            return null;
        }
        else
        {
            item = _content.transform.GetChild(indexCurrent - 1).GetComponent<ItemButton>();
        }

        return item.GetComponent<Selectable>();
    }

    private void TestCreateItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateItems("Level_" + i);
        }
    }

    private ItemButton CreateItems(string strName)
    {
        GameObject gObj;
        ItemButton item;

        gObj = Instantiate(_prefabListItem, Vector3.zero, Quaternion.identity);
        gObj.transform.SetParent(_content.transform);
        //gObj.transform.localScale = Vector3.one;
        gObj.transform.localPosition = new Vector3();
        gObj.transform.localRotation = Quaternion.Euler(new Vector3());

        gObj.name = strName;

        //set params
        item = gObj.GetComponent<ItemButton>();
        item.ItemNameValue = strName;

        //add event listeners
        item.OnSelectEvent.AddListener((ItemButton) => { HandleEventItemOnSelect(item); });
        item.OnSubmitEvent.AddListener((ItemButton) => { HandleEventItemOnSubmit(item); });


        return item;
    }

    private void HandleEventItemOnSubmit(ItemButton item)
    {
        _eventItemOnSubmit.Invoke(item);
    }

    private void HandleEventItemOnSelect(ItemButton item)
    {
        ScrollViewAutoScroll scrollViewAutoScroll = GetComponent<ScrollViewAutoScroll>();

        scrollViewAutoScroll.HandleOnSelectChange(item.gameObject);

        _eventItemOnSelect.Invoke(item);
    }
}
