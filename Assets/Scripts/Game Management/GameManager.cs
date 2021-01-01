//Script destine a gerer les evenements lies directement au joeur

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance; //Instance

    public PlayerStats playerStats; //Statistiques du joueur

    public TMP_Text healthText; //Affichage de la vie du joueur
    public Animator healthAnimator; //Animation de la vie du joueur en cas de perte de vie
    public TMP_Text moneyText; //Affichage de l'argent du joueur

    public Camera gameCamera; //Camera utilisee pour visualiser le jeu
    public GameObject towers; //Objet regroupant les tours existantes

    public PauseHandler pauseHandler; //Gestionnaire de pause

    //Initialisation
    void Awake()
    {
        _Instance = this; //On met a jour l'instance
        TowerPlacement.towersList = new List<GameObject>(); //On cree une nouvelle liste de tours
        playerStats = new PlayerStats(this); //On cree une nouvelle instance du joueur
        TowerPlacement.Initialize(); //On retire l'instance actuelle de tour en placement, s'il en existait une
    }

    //Initialisation
    void Start()
    {
        TowerFabric fabric = FindObjectOfType<TowerFabric>(); //On cherche la fabrique dans la scene et on la reference
        if (fabric != null) fabric.CloseMenu(); //Pour aider au developpement, on ferme automatiquement la fabrique
    }

    //Gestion de la defaite du joueur
    public void LoseGame()
    {
        pauseHandler.PauseGameForEnd(); //On affiche le menu de pause

        pauseHandler.resumeButton.SetActive(false); //On desactive le bouton reprendre
        pauseHandler.pauseTitle.text = "Les aliens ont atteint la forteresse..."; //On met a jour le message affiche
    }

    //Gestion de la victoire du joueur
    public void WinGame()
    {
        pauseHandler.PauseGameForEnd(); //On affiche le menu pause

        pauseHandler.resumeButton.SetActive(false); //On desactive le bouton reprendre
        pauseHandler.pauseTitle.text = "Victoire !"; //On met a jour le message affiche
    }
}
