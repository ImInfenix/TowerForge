//Script gerant les effets sonores du jeu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEvent : MonoBehaviour
{
    public static PlaySoundOnEvent Instance { get; private set; } //Instance

    public AudioClip onClickAudioClip; //Son de clique

    public AudioClip ballistaFire; //Son de tire de balliste
    public AudioClip mortarFire; //Son de tire de mortier
    public AudioClip canonballExplosion; //Son d'explosion du boulet de canon

    public AudioClip towerForged; //Son de forge de tour
    public AudioClip towerCannotPlaced; //Son de refus de placement de tour
    public AudioClip towerPlaced; //Son de placement de tour

    public AudioClip newWave; //Son d'annonce de vague

    AudioSource audioSource; //Emetteur sonore

    bool playEffects; //Si les effets doivent etre joues

    //Initialisation
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        Instance = this;
    }

    //Initialisation
    private void Start()
    {
        //On verifie que les parametres existent
        if (!PlayerPrefs.HasKey("playEffects")) PlayerPrefs.SetInt("playEffects", 1);
        if (!PlayerPrefs.HasKey("effectsVolume")) PlayerPrefs.SetFloat("effectsVolume", 1);

        //On met a jour le volume de la musique et des effets
        UpdateEffectsState();
        UpdateEffectsVolume();
    }

    //Met a jour l'etat des effets sonores
    public void UpdateEffectsState()
    {
        playEffects = PlayerPrefs.GetInt("playEffects") != 0;
    }

    //Met a jour le volume des effets sonores
    public void UpdateEffectsVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("effectsVolume");
    }

    //Joue l'effet de clique
    public void PlayOnClick()
    {
        if(playEffects)
        {
            audioSource.PlayOneShot(onClickAudioClip);
        }
    }

    //Joue l'effet de tire de balliste
    public void PlayBallistaFire()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(ballistaFire);
        }
    }

    //Joue l'effet de mortier
    public void PlayMortarFire()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(mortarFire);
        }
    }

    //Joue l'effet d'explosion de boulet de canon
    public void PlayCanonballExplosion()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(canonballExplosion);
        }
    }

    //Joue l'effet de forge de tour
    public void PlayTowerForged()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(towerForged);
        }
    }

    //Joue l'effet de refus de placement de tour
    public void PlayTowerCannotBePlaced()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(towerCannotPlaced);
        }
    }

    //Joue l'effet de placement de tour
    public void PlayTowerPlaced()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(towerPlaced);
        }
    }

    //Joue l'effet d'annonce de nouvelle vague
    public void PlayNewWave()
    {
        if (playEffects)
        {
            audioSource.PlayOneShot(newWave);
        }
    }
}
