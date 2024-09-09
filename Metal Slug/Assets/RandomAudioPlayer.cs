using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // Liste des clips audio
    private AudioSource audioSource; // Composant AudioSource

    void Start()
    {
        // Obtenez le composant AudioSource attaché à cet objet
        audioSource = GetComponent<AudioSource>();

        // Vérifiez si des clips audio ont été assignés
        if (audioClips.Length > 0)
        {
            PlayRandomClip();
        }
    }

    void Update()
    {
        // Vérifiez si la lecture est terminée et lancez un nouveau clip
        if (!audioSource.isPlaying)
        {
            PlayRandomClip();
        }
    }
    
    void PlayRandomClip()
    {
        // Sélectionnez un clip audio aléatoire
        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip clipToPlay = audioClips[randomIndex];

        // Jouez le clip audio sélectionné
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}