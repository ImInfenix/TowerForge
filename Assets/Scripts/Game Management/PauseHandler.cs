//Script destine a gerer le menu pause

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseHandler : MonoBehaviour
{
    public TowerFabric towerFabric; //Fabrique du jeu

    public GameObject menu; //Objet representant le menu pause
    public TMP_Text pauseTitle; //Objet contenant le message de pause
    public GameObject resumeButton; //Bouton reprendre
    public GameObject pauseButton; //Bouton pause

    public GameObject tipsMenu; //Menu d'aide
    public GameObject helpButton; //Bouton aide

    public GameObject nextWaveButton; //Bouton de lancement de vague
    public GameObject openFabricMenu; //Bouton d'ouverture de la fabrique

    //Initialisation
    private void Start()
    {
        menu.SetActive(false); //Aide au developpement, on masque le menu pause
    }

    //Fonction appellee lorsque l'on met le jeu en pause
    public void PauseGame()
    {
        towerFabric.CloseMenu(); //On ferme la fabrique
        if (TowerPlacement.Instance != null) TowerPlacement.Instance.DeleteInstance(); //On detruit la tour en cours de placement

        Time.timeScale = 0.0f; //On arrete le temps

        pauseButton.SetActive(false); //On masque le bouton pause
        helpButton.SetActive(false); //On masque le bouton d'aide
        nextWaveButton.SetActive(false); //On masque le bouton de lancement de vague
        openFabricMenu.SetActive(false); // On masque le bouton d'ouverture de la fabrique
        menu.SetActive(true); //On affiche le menu pause
    }

    //Fonction appellee lorsque l'on reprend le jeu
    public void ResumeGame()
    {
        menu.SetActive(false); //On desactive le menu pause
        openFabricMenu.SetActive(true); // On reactive le bouton d'ouverture de la fabrique
        nextWaveButton.SetActive(true); //On reactive le bouton de lancement de vague
        helpButton.SetActive(true); //On reactive le bouton d'aide
        pauseButton.SetActive(true); //On reactive le bouton pause

        Time.timeScale = 1.0f; //On redemarre le temps
    }

    //Fonction appellee lorsque l'on redemarre la partie
    public void RestartGame()
    {
        Time.timeScale = 1.0f; //On reactive le temps
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //On recharge la scene
    }

    //Fonction appellee lorsque l'on retourne au menu principal
    public void GoBackToMenu()
    {
        Time.timeScale = 1.0f; //On reactive le temps
        SceneManager.LoadScene("Scenes/MainMenu"); //On charge la scene du menu
    }

    //Stop le jeu apres une victoire / defaite
    public void PauseGameForEnd()
    {
        towerFabric.CloseMenu(); //On ferme la fabrique
        if (TowerPlacement.Instance != null) TowerPlacement.Instance.DeleteInstance(); //On detruit la tour en cours de placement

        pauseButton.SetActive(false); //On desactive le boutton pause
        helpButton.SetActive(false); // On masque le bouton d'aide
        nextWaveButton.SetActive(false); //On masque le bouton de lancement de vague
        openFabricMenu.SetActive(false); // On masque le bouton d'ouverture de la fabrique
        menu.SetActive(true); //On active le menu pause
    }

    //Affiche l'aide au joueur
    public void ShowTips()
    {
        towerFabric.OpenMenu(); //On ouvre la fabrique

        Time.timeScale = 0.0f; //On arrete le temps

        pauseButton.SetActive(false); //On masque le bouton pause
        helpButton.SetActive(false); // On masque le bouton d'aide
        tipsMenu.SetActive(true); //On affiche le menu d'aide
    }

    //Cache l'aide au joueur
    public void HideTips()
    {
        tipsMenu.SetActive(false); //On desactive le menu d'aide
        helpButton.SetActive(true); //On reactive le bouton d'aide
        pauseButton.SetActive(true); //On reactive le bouton pause

        Time.timeScale = 1.0f; //On redemarre le temps
    }
}
