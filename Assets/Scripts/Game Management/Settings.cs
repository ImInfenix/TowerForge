//Script gerant les parametres du joueur

using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject attachedMenu; //Menu depuis lequel le menu de configuration est appele
    bool isInitialized = false; //Si le menu de configuration a ete initialise

    //Objets de menu de configuration
    public Toggle playMusic;
    public Slider musicVolume;
    public Toggle playEffects;
    public Slider effectsVolume;

    //initialisation
    private void Start()
    {
        //On verifie que chacun des parametres existent. Si ce n'est pas le cas, on les initialise a 1 (on / volume max)
        if (!PlayerPrefs.HasKey("playMusic")) PlayerPrefs.SetInt("playMusic", 1);
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 1);
        if (!PlayerPrefs.HasKey("playEffects")) PlayerPrefs.SetInt("playEffects", 1);
        if (!PlayerPrefs.HasKey("effectsVolume")) PlayerPrefs.SetFloat("effectsVolume", 1);

        //On initialise le menu de configuration en fonction des parametres du joueur
        playMusic.isOn = PlayerPrefs.GetInt("playMusic") != 0;
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume");
        playEffects.isOn = PlayerPrefs.GetInt("playEffects") != 0;
        effectsVolume.value = PlayerPrefs.GetFloat("effectsVolume");

        isInitialized = true;
    }

    //Affiche le menu de configuration
    public void ShowSettingsConfiguration()
    {
        attachedMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    //Cache le menu de configuration
    public void HideSettingsConfiguration()
    {
        PlaySoundOnEvent.Instance.PlayOnClick();

        gameObject.SetActive(false);
        attachedMenu.SetActive(true);
    }

    //Fonction appelee en cas de changement de valeur du parametre playMusic
    public void PlayMusic(bool play)
    {
        if(isInitialized)
        {
            PlayerPrefs.SetInt("playMusic", play ? 1 : 0); //On modifie avec la nouvelle valeur
            MusicManager.Instance.UpdateMusicState(); //On met a jour la musique
        }
    }

    //Fonction appelee en cas de changement de valeur du parametre musicVolume
    public void SetMusicVolume(float value)
    {
        if (isInitialized)
        {
            PlayerPrefs.SetFloat("musicVolume", value); //On modifie avec la nouvelle valeur
            MusicManager.Instance.UpdateMusicVolume(); //On met a jour le volume de la musique
        }
    }

    //Fonction appelee en cas de changement de valeur du parametre playEffects
    public void PlayEffects(bool play)
    {
        if(isInitialized)
        {
            PlayerPrefs.SetInt("playEffects", play ? 1 : 0); //On modifie avec la nouvelle valeur
            PlaySoundOnEvent.Instance.UpdateEffectsState(); //On met a jour les effets
        }
    }

    //Fonction appelee en cas de changement de valeur du parametre effectsVolume
    public void SetEffectsVolume(float value)
    {

        if (isInitialized)
        {
            PlayerPrefs.SetFloat("effectsVolume", value); //On modifie avec la nouvelle valeur
            PlaySoundOnEvent.Instance.UpdateEffectsVolume(); //On met a jour le volume des effets
        }
    }
}
