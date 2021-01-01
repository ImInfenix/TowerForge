//Script gerant la construction d'une tour via l'UI

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerFabric : MonoBehaviour
{
    public enum BaseType { Forge, Temple, Laboratoire, Tavern } //Types de bases disponibles
    public enum WeaponType { Balliste, Mortier } //Types d'arme disponibles

    public GameObject OpenMenuButton; //Bouton pour ouvrir la fabrique

    public List<TowerDescriptor> basesSprites; //Images des bases disponibles
    public List<TowerDescriptor> weaponsSprites; //Images des armes disponibles

    public Image baseImage; //Affichage de la base selectionnee
    public Image weaponImage; //Affichage de l'arme selectionnee

    BaseType currentBasePick; //Type de base selectionnee
    WeaponType currentWeaponPick; //Type d'arme selectionnee

    public TMP_Text currentBasePickText; //Nom de la base actuellement affichee
    public TMP_Text currentWeaponPickText; //Nom de l'arme actuellement affichee

    public TMP_Text currentBaseDescriptor; //Description de la base actuellement affichee
    public TMP_Text currentWeaponDescriptor; //Description de l'arme actuellement affichee

    float cost; //Cout total de la tour selectionnee
    public TMP_Text priceText; //Affichage du cout total

    public GameObject towerPrefab; //Objet de depart pour la construction d'une tour
    public GameObject towerRangeIndicator; //Objet d'affichage de la portee d'une tour

    //Initialisation
    public void Awake()
    {
#if UNITY_EDITOR //On verifie qu'au moins une tour soit disponible
        if (basesSprites.Count == 0) Debug.LogError("At least one image must be set in the image list of TowerFabric for : bases /!\\");
        if (weaponsSprites.Count == 0) Debug.LogError("At least one image must be set in the image list of TowerFabric for : weapons /!\\");
#endif
        currentBasePick = 0; //On initialise la base actuelle en prenant la premiere
        currentWeaponPick = 0; //On initialise l'arme actuelle en prenant la premiere
        UpdateCost(); //On met a jour le cout apres avoir choisi la tour de depart

        baseImage.sprite = basesSprites[(int) currentBasePick].sprite; //On initialise la base actuellement affichee
        weaponImage.sprite = weaponsSprites[(int) currentWeaponPick].sprite; //On initialise l'arme actuellement affichee

        currentBasePickText.text = basesSprites[(int)currentBasePick].name; //On initialise le nom la base actuellement affichee
        currentWeaponPickText.text = weaponsSprites[(int)currentWeaponPick].name; //On initialise le nom l'arme actuellement affichee

        UpdateBaseDescriptor(); //On met a jour la description de la base selectionnee
        UpdateWeaponDescriptor(); //On met a jour la description de l'arme selectionnee
    }

    //Permet d'ouvrir le menu de la fabrique
    public void OpenMenu()
    {
        OpenMenuButton.SetActive(false); //On cache le bouton d'ouverture
        gameObject.SetActive(true); //On affiche la fabrique
    }

    //Permet de fermer le menu de la fabrique
    public void CloseMenu()
    {
        gameObject.SetActive(false); //On cache la fabrique
        OpenMenuButton.SetActive(true); //On reaffiche le bouton pour l'ouvrir
    }

    //Fonction appellee lorsque le joueur confirme
    public void ConfirmChoice()
    {
        if (TowerPlacement.Instance != null)
        {
            TowerPlacement.Instance.DeleteInstance();
            return;
        }
        else
        {
            DisplayCancelText();
        }

        GameObject tower = Instantiate(towerPrefab); //On cree une nouvelle tour
        TowerPlacement towerPlacement = tower.GetComponent<TowerPlacement>(); //On recupere l'objet gerant le placement d'une tour
        SpriteRenderer[] renderers = tower.GetComponentsInChildren<SpriteRenderer>(); //On recupere les differents affichages

        towerPlacement.towerFabric = this; // On rappel la fabrique a la nouvelle instance

        foreach(SpriteRenderer renderer in renderers)
        {
            switch(renderer.name)
            {
                case "Base": //On met l'affichage de la base a jour
                    renderer.sprite = baseImage.sprite;
                    towerPlacement.baseSprite = baseImage.sprite;
                    break;
                case "Weapon": //On met l'affichage de l'arme a jour
                    renderer.sprite = weaponImage.sprite;
                    towerPlacement.weaponSprite = weaponImage.sprite;
                    break;
            }
        }
        towerPlacement.baseType = currentBasePick; //On communique a l'objet le type de base choisi
        towerPlacement.weaponType = currentWeaponPick; //On communique a l'objet le type d'arme choisi
        towerPlacement.SetCost(cost); //On definit le cout de la tour a placer

        //On ajoute un affichage de la portee de la tour
        RangeIndicator rangeIndicator = Instantiate(towerRangeIndicator, towerPlacement.transform).GetComponent<RangeIndicator>();
        float range;
        switch(currentWeaponPick) //On recupere la portee en fonction du type d'arme
        {
            case WeaponType.Balliste:
                range = GameBalance.Ballista.range;
                break;
            case WeaponType.Mortier:
                range = GameBalance.Mortar.range;
                break;
            default:
                Debug.LogError("Arme inexistante !");
                return;
        }
        rangeIndicator.SetRange(range); //On met a jour la portee a afficher
        rangeIndicator.SetSelected(true); //On active l'affichage de la portee

        PlaySoundOnEvent.Instance.PlayTowerForged(); //On joue le son de forge de tour
    }

    //Gere le changement de base via l'UI
    public void SwapBase(bool GoOnRight)
    {
        //On change le choix en fonction de l'action du joueur
        if(GoOnRight) //A droite
        {
            currentBasePick++;
            if ((int) currentBasePick >= basesSprites.Count) currentBasePick = 0;
        }
        else //A gauche
        {
            currentBasePick--;
            if (currentBasePick < 0) currentBasePick = (BaseType) basesSprites.Count - 1;
        }

        currentBasePickText.text = basesSprites[(int)currentBasePick].name; //On met a jour la base selectionnee
        baseImage.sprite = basesSprites[(int) currentBasePick].sprite; //On met a jour l'affichage
        UpdateBaseDescriptor(); //On met a jour la description
        if(TowerPlacement.Instance == null) UpdateCost(); //On met a jour le cout
    }

    //Gere le changement d'arme via l'UI
    public void SwapWeapon(bool GoOnRight)
    {
        //On change le choix en fonction de l'action du joueur
        if (GoOnRight) //A droite
        {
            currentWeaponPick++;
            if ((int) currentWeaponPick >= weaponsSprites.Count) currentWeaponPick = 0;
        }
        else //A gauche
        {
            currentWeaponPick--;
            if (currentWeaponPick < 0) currentWeaponPick = (WeaponType) weaponsSprites.Count - 1;
        }

        currentWeaponPickText.text = weaponsSprites[(int)currentWeaponPick].name; //On met a jour l'arme selectionnee
        weaponImage.sprite = weaponsSprites[(int) currentWeaponPick].sprite; //On met a jour l'affichage
        UpdateWeaponDescriptor(); //On met a jour la description
        if (TowerPlacement.Instance == null) UpdateCost(); //On met a jour le cout
    }

    //Met a jour le cout de la tour selectionnee
    public void UpdateCost()
    {
        cost = 0; //On initialise le cout a 0
        switch(currentBasePick) //On ajoute le cout la base
        {
            case BaseType.Forge:
                cost += GameBalance.Forge.cost;
                break;

            case BaseType.Laboratoire:
                cost += GameBalance.Laboratory.cost;
                break;

            case BaseType.Temple:
                cost += GameBalance.Temple.cost;
                break;

            case BaseType.Tavern:
                cost += GameBalance.Tavern.cost;
                break;

            default:
                Debug.LogError($"Pas de cout defini pour la base {currentBasePick}");
                break;
        }
        switch(currentWeaponPick) //On ajoute le cout de l'arme
        {
            case WeaponType.Balliste:
                cost += GameBalance.Ballista.cost;
                break;

            case WeaponType.Mortier:
                cost += GameBalance.Mortar.cost;
                break;

            default:
                Debug.LogError($"Pas de cout defini pour l'arme {currentWeaponPick}");
                break;
        }

        priceText.text = $"Forger\n- {cost} pts -"; //On met a jour l'affichage du cout
    }

    // Change le text de confirmation pour le remplacer par une annulation
    void DisplayCancelText()
    {
        priceText.text = "- Annuler -"; //On change l'affichage pour proposer l'annulation d'achat
    }

    //Met a jour la description de la base
    void UpdateBaseDescriptor()
    {
        string str = "";
        foreach (string s in basesSprites[(int)currentBasePick].description)
            str += s + "\n";
        currentBaseDescriptor.text = str;
    }

    //Met a jour la description de l'arme
    void UpdateWeaponDescriptor()
    {
        string str = "";
        foreach (string s in weaponsSprites[(int)currentWeaponPick].description)
            str += s + "\n";
        currentWeaponDescriptor.text = str;
    }
}

//Objet decrivant une tour par un nom, un sprite et une description servants a la mise en place des tours via l'inspecteur
[System.Serializable]
public class TowerDescriptor
{
    public string name;
    public Sprite sprite;
    public List<string> description;
}