using UnityEngine;
using UnityEngine.UI;

public class anjian : MonoBehaviour
{
    public AudioClip buttonSound; 
    private AudioSource audioSource;

    void Start()
    {
          
        audioSource = gameObject.AddComponent<AudioSource>();
        
        
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (buttonSound != null)
        {
            audioSource.PlayOneShot(buttonSound); 
        }
    }
}