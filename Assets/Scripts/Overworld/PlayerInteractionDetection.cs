using UnityEngine;

public class PlayerInteractionDetection : MonoBehaviour
{
    public GameObject InteractableObject = null;
    public int maxInteractionDistance = 1;
    [SerializeField] private float distance;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
            InteractableObject = other.gameObject;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Interactable"))
    //    {
    //        InteractableObject = null;
    //    }
    //}

    private void Update()
    {
        if (InteractableObject != null)
        {
            distance = Vector3.Distance(gameObject.transform.position, InteractableObject.transform.position);
            InteractableObject = (distance >= maxInteractionDistance) ? null : InteractableObject;

            
        }
    }
}
