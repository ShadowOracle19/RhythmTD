using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadoutIcons : MonoBehaviour, ISelectHandler
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
        infoPanel.WriteInfoPanel(tower);
    }
}
