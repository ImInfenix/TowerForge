//Script gerant le menu principal

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Fonction appelle en cas de clique sur le bouton jouer
    public void Play()
    {
        SceneManager.LoadScene("Scenes/Level"); //On charge le jeu
    }

    //Fonction appelle en cas de clique sur le bouton credits
    public void Credits(Credits credits)
    {
        credits.StartCredits(); //On affiche les credits
    }

    //Fonction appelle en cas de clique sur le bouton quitter
    public void Quit()
    {
        Debug.Log("Exit game !");
        Application.Quit(0); //On ferme l'application
    }
}
