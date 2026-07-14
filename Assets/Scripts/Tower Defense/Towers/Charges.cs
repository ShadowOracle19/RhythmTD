using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charges : MonoBehaviour
{
    public int resourceGain;
    public Vector3 placementLocation;
    public bool chargeActive = false;
    private bool damageCharge = false;

    public float timeAtPlacement = 0.0f;
    public int expiryTimeInBeats = 0;
    public int expiryTelegraphInBeats = 0;

    [Header ("Lerp Variables (DO NOT TOUCH)")]
    public SpriteRenderer spriteRenderer;
    public Color currentColor;
    public float alphaTargetTime = 0.0f;
    public float alphaProgress = 0.0f;
    public float defaultAlpha = 1.0f;
    public float expiryAlpha = 0.5f;

    private void Update()
    {
        if (!chargeActive)
        {
            transform.position = Vector3.Lerp(transform.position, placementLocation, Time.deltaTime * 5);

            if (Vector3.Distance(transform.position, placementLocation) < 0.01f)
            {
                transform.position = placementLocation;
                chargeActive = true;
            }
        }
    }

    public void initalizeCharge(int _resourceGain, Vector3 _placementLocation, Tower connectedTower, bool fromTower)
    {
        timeAtPlacement = ConductorV2.instance.songPosition;
        resourceGain = _resourceGain;
        placementLocation = _placementLocation;
        if(fromTower)
        {
            if (connectedTower.upgradeIndex == 1)
            {
                damageCharge = true;
            }
            else
            {
                damageCharge = false;
            }
        }

        StartCoroutine(StartChargeExpiry(defaultAlpha, expiryAlpha));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !damageCharge && chargeActive)
        {
            CombatManager.Instance.resourceNum += resourceGain;
            RemoveCharge();
        }
        else if(other.gameObject.CompareTag("Enemy") && damageCharge)
        {
            other.gameObject.GetComponent<Enemy>().Damage(resourceGain * 2);
            RemoveCharge();
        }
    }

    public void RemoveCharge()
    {
        Destroy(gameObject);
    }

    public IEnumerator StartChargeExpiry(float startAlpha, float endAlpha)
    {
        bool fading = true;
        
        while (ConductorV2.instance.songPosition < (timeAtPlacement + (ConductorV2.instance.crotchet * expiryTimeInBeats))) //(expiryTimeInBeats - expiryTelegraphInBeats)
        {
            Debug.Log(timeAtPlacement + (ConductorV2.instance.crotchet * (expiryTimeInBeats - expiryTelegraphInBeats)));
            yield return null;
        }

        //alphaTargetTime = (timeAtPlacement + (ConductorV2.instance.crotchet * (expiryTimeInBeats - expiryTelegraphInBeats))) + (ConductorV2.instance.crotchet / 4);
        //alphaProgress = 0.0f;
        //Debug.Log("Current Song Time:" + ConductorV2.instance.songPosition);
        //Debug.Log("Alpha Target Time:" + alphaTargetTime);
        //Debug.Log("Expiry End Time:" + (timeAtPlacement + (ConductorV2.instance.crotchet * expiryTimeInBeats)));

        /*
        while (ConductorV2.instance.songPosition < (timeAtPlacement + (ConductorV2.instance.crotchet * expiryTimeInBeats)))
        {
            alphaProgress = (ConductorV2.instance.songPosition - timeAtPlacement) / (alphaTargetTime - timeAtPlacement);

            currentColor = new Color(1f, 1f, 1f, Mathf.Lerp(startAlpha, endAlpha, alphaProgress));
            spriteRenderer.color = currentColor;
            
            if (fading && alphaProgress >= 1.0f)
            {
                startAlpha = expiryAlpha;
                endAlpha = defaultAlpha;
                alphaTargetTime += ConductorV2.instance.crotchet / 4;
                fading = false;
            }
            else if (alphaProgress >= 1.0f)
            {
                startAlpha = defaultAlpha;
                endAlpha = expiryAlpha;
                alphaTargetTime += ConductorV2.instance.crotchet / 4;
                fading = true;
            }

            yield return null;
        }
        */

        RemoveCharge();
    }
}
