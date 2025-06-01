using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadoutIcons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TowerTypeCreator tower;
    public LoadoutInfoPanel infoPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        infoPanel.gameObject.SetActive(true);
        infoPanel.WriteInfoPanel(tower);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        infoPanel.gameObject.SetActive(false);
    }
}
