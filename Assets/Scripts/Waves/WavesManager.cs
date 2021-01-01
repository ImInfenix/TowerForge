//Script gerant les vagues

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavesManager : MonoBehaviour
{
    public TMP_Text waveCount; //Affichage de la vague a laquelle on est rendu

    public static WavesManager _Instance = null; //Instance
    public GameObject aliveEnnemies; //Objet regroupant les ennemis en vie

    public List<WaveDescriptor> waves; //Description de chaque vague
    bool[] finishedWaves; //Tableau regroupant l'etat (fini ou non) de chacune des vagues
    int currentWave; //Vague actuellement jouee

    //Initialisation
    void Awake()
    {
        _Instance = this;
        finishedWaves = new bool[waves.Count];
        for (int i = 0; i < finishedWaves.Length; i++) finishedWaves[i] = false;

        currentWave = 0;
        waveCount.text = "Préparez-vous !";
    }

    //Lance la vague suivante
    public void LaunchNextWave()
    {
        if(currentWave < waves.Count) //S'il reste une vague a envoyer
        {
            //On cree une nouvelle vague et on l'initialise
            Wave newWave = new GameObject().AddComponent<Wave>();
            newWave.name = "Wave " + (currentWave + 1);
            newWave.track = waves[currentWave].track; //Chemin a suivre
            newWave.waveComposition = waves[currentWave].waveElements; //Composition de la vague
            newWave.waveIndex = currentWave; //On definit la vague actuelle comme celle-ci
            newWave.Launch(); //On demarre la vague
            currentWave++; //On incremente l'indice de la vague actuelle
            waveCount.text = "Vague " + currentWave; //On met a jour l'affichage
        }
    }

    //Declare une vague terminee via son indice
    public void EndWave(int index)
    {
        finishedWaves[index] = true; //On declare la vague terminee

        //On verifie si toutes les vagues sont finies
        bool gameEnded = true;
        for(int i = 0; i < finishedWaves.Length; i++)
        {
            if(!finishedWaves[i]) //Si une vague ne l'est pas, on s'arrete
            {
                gameEnded = false;
                break;
            }
        }

        //Si toutes les vagues sont terminees, on fait gagner le joueur
        if(gameEnded) GameManager._Instance.WinGame();
    }
}
