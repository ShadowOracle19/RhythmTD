using UnityEngine;

public class TitleMenuRoot : MonoBehaviour
{
    // VARIABLE
    public AudioSource titleMenuMusic;
    
    private void Awake()
    {
        titleMenuMusic.Play();
    }
}
