using UnityEngine;

public class Billboarding : MonoBehaviour
{
    [SerializeField] bool freezeXZAxis = true;
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        //if (freezeXZAxis)
        //{
        //    transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        //}
        //else
        //{
            
        //}
    }
}

