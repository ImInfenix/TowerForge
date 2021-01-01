//Script gerant la musique du jeu

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } //Instance

    public AudioClip menuMusic; //Musique du menu
    public AudioClip gameMusic; //Musique du jeu

    AudioSource audioSource; //Emetteur de la musique

    //Initialisation
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //On recupere la musique liee a la scene
        audioSource.clip = SceneManager.GetActiveScene().buildIndex == 0 ? menuMusic : gameMusic;

        if (Instance == null) //Si pas d'instance, on definit celle-ci comme le singleton
        {
            Instance = this;
        }
        else if (Instance.audioSource.clip != audioSource.clip) //Sinon, on verifie si le clip audio est le meme
        {
            Destroy(Instance.gameObject); //Si non, on detruit le singleton
            Instance = this;              //et on le remplace
        }
        else
        {
            Destroy(gameObject); //Si oui, on continue de jouer la musique et on detruit cette instance
        }

        //On dit a l'instance de ne pas etre detruite au chargement d'une nouvelle scene pour ne pas redemarrer la musique
        //lorsque l'on recharge la scene (boutton recommencer)
        DontDestroyOnLoad(this);
    }

    //Initialisation
    private void Start()
    {
        //On verifie que les parametres existent
        if (!PlayerPrefs.HasKey("playMusic")) PlayerPrefs.SetInt("playMusic", 1);
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 1);

        //On met a jour le volume de la musique et des effets
        UpdateMusicState();
        UpdateMusicVolume();
    }

    //Met a jour l'etat de la musique
    public void UpdateMusicState()
    {
        if(PlayerPrefs.GetInt("playMusic") != 0)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    //Met a jour le volume de la musique
    public void UpdateMusicVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}
