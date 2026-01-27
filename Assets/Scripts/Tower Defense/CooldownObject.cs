using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownObject : MonoBehaviour
{
    public Transform cooldownParent;
    public GameObject towerLoadoutObject;
    public GameObject towerCooldownSlot;
    public Animator towerCooldownAnimation;
    public bool towerCooldown;
    public float towerCooldownTimeRemaining = 0;
    public float towerCooldownTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        towerCooldownSlot.SetActive(towerCooldown);

        towerCooldownAnimation.SetBool("Cooldown", towerCooldown);

        if(towerCooldown)
        {
            towerCooldownTime += Time.deltaTime;

            //cooldown effect
            towerCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(towerCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((towerCooldownTime / towerCooldownTimeRemaining) * 100));
            
            if (towerCooldownTime >= towerCooldownTimeRemaining)
            {
                towerCooldown = false;
                towerCooldownTime = 0;
            }
        }
    }

    public void SpawnCooldownLoadoutObject(GameObject loadoutObjectPrefab)
    {
        towerLoadoutObject = Instantiate(loadoutObjectPrefab, cooldownParent);
    }

    public void RemoveTowerLoadoutObject()
    {
        Destroy(towerLoadoutObject);
    }

    public void ResetCooldownObject()
    {
        towerCooldown = false;
        RemoveTowerLoadoutObject();
    }
}
