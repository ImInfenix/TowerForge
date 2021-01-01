//Script gerant le placement d'une tour une fois selectionnee dans la fabrique

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    public static TowerPlacement Instance { get; private set; } //Definit si une tour est en train d'etre placee - sert a bloquer la fabrique

    public  TowerFabric towerFabric;

    public GameObject TowerPrefab; //Objet de base pour la construction d'une tour
    public Sprite baseSprite; //Sprite de la base
    public Sprite weaponSprite; //Sprite de l'arme

    public static Transform towersGO; //Objet regroupant les differentes tours
    public static List<GameObject> towersList; //Liste des tours existantes

    public TowerFabric.BaseType baseType; //Type de la base de la tour
    public TowerFabric.WeaponType weaponType; //Type de l'arme de la tour

    float cost; //Cout de la tour
    CheckPlaceIsAvailable checkPlace; //Objet verifiant si un emplacement est disponible

    //Intialisation au lancement de la partie
    public static void Initialize()
    {
        Instance = null;
    }

    //Initialisation
    void Start()
    {
#if UNITY_EDITOR
        if (Instance != null) Debug.LogError("Une instance de Tower Placement existe deja !");
#endif

        towersGO = GameManager._Instance.towers.transform; //On recupere l'objet regroupant les tours
        checkPlace = GetComponentInChildren<CheckPlaceIsAvailable>(); //On recupere l'objet definissant si un emplacement est disponible
        ShowBaseRanges(); //On affiche les emplacements rendus indisponibles par les tours
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //On met a jour la position de l'objet par rapport a l'emplacement de la souris
        Vector3 position = GameManager._Instance.gameCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(position.x, position.y, 0);

        //Si le joueur clique sur une zone du jeu (pas l'UI)
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //On verifie que la place est disponible et on fait payer la tour au joueur
            if (checkPlace.CanBePlaced && GameManager._Instance.playerStats.Pay(cost))
            {
                GameObject finalTower = Instantiate(TowerPrefab, towersGO); //On cree une nouvelle tour
                //On initialise la nouvelle tour par rapport a son type de base et d'arme
                towersGO.GetComponent<TowerSetup>().LoadInfos(finalTower, baseType, weaponType);
                finalTower.transform.position = transform.position; //On definit la position ou est placee la tour
                towersList.Add(finalTower); //On ajoute cette tour a la liste des tours existantes

                PlaySoundOnEvent.Instance.PlayTowerPlaced(); //On joue le son de placement de tour
                DeleteInstance(); //On detruit l'objet actuel puisqu'il ne sert plus
            }
            else PlaySoundOnEvent.Instance.PlayTowerCannotBePlaced(); //Sinon on joue le son d'empechement de placement de tour
        }

        //Si le joueur fait un clique droit, on annule la construction
        else if(Input.GetMouseButtonDown(1))
        {
            DeleteInstance(); //On annule la creation de tour
        }
    }

    public void DeleteInstance()
    {
        HideBaseRanges(); //On cache les emplacements rendus indisponibles par les autres tours
        towerFabric.UpdateCost(); // On reaffiche le cout de la tour
        Destroy(Instance.gameObject); //On detruit l'objet
        Instance = null; //On met l'instance a null pour etre sur de ne pas tout casser, ca coute rien
    }

    //Affiche les emplacements rendus indisponibles par les autres tours
    void ShowBaseRanges()
    {
        foreach (GameObject g in towersList) //Pour chacune des tours existantes
        {
            g.GetComponent<RangeDisplay>().ShowBaseRange(); //On affiche l'emplacement occupe
        }
    }

    //Cache les emplacements rendus indisponibles par les autres tours
    void HideBaseRanges()
    {
        foreach (GameObject g in towersList) //Pour chacune des tours existantes
        {
            g.GetComponent<RangeDisplay>().HideBaseRange(); //On cache l'emplacement occupe
        }
    }

    //Met a jour le cout de la tour
    public void SetCost(float cost)
    {
        this.cost = cost;
    }
}
