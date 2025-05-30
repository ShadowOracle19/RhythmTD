using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutInfoPanel : MonoBehaviour
{
    public TMP_Text characterName;
    public TMP_Text instrumentName;
    public TowerResourceCost cost;
    public TMP_Text cooldown;

    public Image cost1, cost2, cost3, cost4;
    public Color c1, c2, c3, c4;

    public void WriteInfoPanel(TowerTypeCreator towerToWrite)
    {
        characterName.text = towerToWrite.towerName;
        instrumentName.text = $"Instrument: {towerToWrite.type.ToString()}";
        cooldown.text = towerToWrite.cooldown.ToString();
        cost1.color = c1;

        switch (towerToWrite.cost)
        {
            case TowerResourceCost.one:
                cost2.color = Color.white;
                cost3.color = Color.white;
                cost4.color = Color.white;
                break;
            case TowerResourceCost.two:
                cost2.color = c2;
                cost3.color = Color.white;
                cost4.color = Color.white;
                break;
            case TowerResourceCost.three:
                cost2.color = c2;
                cost3.color = c3;
                cost4.color = Color.white;
                break;
            case TowerResourceCost.four:
                cost2.color = c2;
                cost3.color = c3;
                cost4.color = c4;
                break;
            default:
                break;
        }
    }
}
