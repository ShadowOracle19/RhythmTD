using UnityEngine;

public class ShielderEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile_Tower"))
        {
            Destroy(other.gameObject);
        }
    }
}
