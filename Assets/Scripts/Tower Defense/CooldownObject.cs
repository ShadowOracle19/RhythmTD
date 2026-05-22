using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownObject : MonoBehaviour
{
    public Transform cooldownParent;
    [HideInInspector]
    public GameObject towerLoadoutObject;
    public RectTransform cooldownBar;
    public Animator towerCooldownAnimation;
    private float AnimationBPM;

    [HideInInspector]
    public Tower currentConnectedTower;

    [HideInInspector]
    public bool towerCooldown;

    //[HideInInspector]
    public float towerCooldownTimeRemaining = 0;

    //[HideInInspector]
    public float towerCooldownTime = 0;

    [HideInInspector]
    public int currentNumberPlaced = 0;

    // Start is called before the first frame update
    void Start()
    {
        cooldownBar.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //towerCooldownSlot.SetActive(towerCooldown);

        towerCooldownAnimation.SetBool("Cooldown", towerCooldown);
        towerCooldownAnimation.SetBool("NoPurchase", CheckIfCanPurchase());

        //AnimationBPM = (float)(2*(ConductorV2.instance.bpm*0.0125));
        towerCooldownAnimation.SetFloat("Speed", AnimationBPM);

        if(towerCooldown)
        {
            towerCooldownTime += Time.deltaTime;

            //cooldown effect
            //resourceBar.anchoredPosition = new Vector3((336.0f/100.0f) * ((towerCooldownTime / towerCooldownTimeRemaining) * 100), 0.0f, 0.0f);
            cooldownBar.anchoredPosition = new Vector3(359.0f - ((359.0f / 100.0f) * ((towerCooldownTime / towerCooldownTimeRemaining) * 100)), 0.0f, 0.0f);

            if (towerCooldownTime >= towerCooldownTimeRemaining)
            {
                towerCooldown = false;
                towerCooldownTime = 0;
            }
        }
    }

    public bool CheckIfCanPurchase()
    {
        if (currentConnectedTower == null)
        {
            return false;
        }
        if(currentConnectedTower.towerInfo.resourceCost >= CombatManager.Instance.resourceNum && !towerCooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnCooldownLoadoutObject(GameObject loadoutObjectPrefab, Tower tower)
    {
        towerLoadoutObject = Instantiate(loadoutObjectPrefab, cooldownParent);
        towerCooldownAnimation = towerLoadoutObject.GetComponentInChildren<Animator>();
        AnimationManager.instance.towerLoadoutAnimators.Add(towerCooldownAnimation);

        currentConnectedTower = tower;
    }

    public void RemoveTowerLoadoutObject()
    {
        Destroy(towerLoadoutObject);
        currentConnectedTower = null;
    }

    public void ResetCooldownObject()
    {
        towerCooldown = false;
        RemoveTowerLoadoutObject();
    }
}
