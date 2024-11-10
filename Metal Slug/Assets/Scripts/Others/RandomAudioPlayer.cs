using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // Liste des clips audio
    private AudioSource audioSource; // Composant AudioSource
    public PlayerMovement playerMovement;


    private int randomIndex;
    private AudioClip clipToPlay;
    


    void Start()
    {
        // Obtenez le composant AudioSource attaché à cet objet
        audioSource = GetComponent<AudioSource>();

        // Vérifiez si des clips audio ont été assignés
        if (audioClips.Length > 0 && playerMovement.wantToFight)
        {
            PlayRandomClip();
        }
        
    }

    void Update()
    {
        // Vérifiez si la lecture est terminée et lancez un nouveau clip
        if (!audioSource.isPlaying && playerMovement.wantToFight)
        {
            PlayRandomClip();
        }
        if(audioSource.isPlaying && !playerMovement.wantToFight)
        {
            audioSource.Stop();
        }
    }
    
    void PlayRandomClip()
    {
        // Sélectionnez un clip audio aléatoire
        randomIndex = Random.Range(0, audioClips.Length);
        clipToPlay = audioClips[randomIndex];

        // Jouez le clip audio sélectionné
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}