//Script destine a la construction de vagues via l'inspecteur de Unity

using System.Collections.Generic;
using UnityEngine;

//Element de vague
[System.Serializable]
public class WaveElement
{
    public GameObject gameObject; //Ennemi associe
    public int quantity; //Nombre d'ennemi a creer
    public float timeSpacing; //Duree d'attente avant l'ennemi suivant
}

//Descripteur d'une vague
[System.Serializable] //Descripteur d'une vague
public class WaveDescriptor
{
    public Track track; //Chemin a suivre
    public List<WaveElement> waveElements; //Elements de la vague
}

//Descripteur de toutes les vague
public class WavesDescriptor
{
    public List<WaveDescriptor> waveDescriptors; //Vagues successives
}