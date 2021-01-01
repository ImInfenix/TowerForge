//Script permettant d'initialiser une tour en fonction de son type de base et d'arme

using System.Collections.Generic;
using UnityEngine;

public class TowerSetup : MonoBehaviour
{
    public List<BaseInfos> basesInfos; //Informations a charger pour chaque type de base
    public List<WeaponInfos> weaponsInfos; //Informations a charger pour chaque type d'arme

    Dictionary<TowerFabric.BaseType, GameObject> basesDictionnary; //Dictionnaire liant un type de base a ses informations
    Dictionary<TowerFabric.WeaponType, GameObject> weaponsDictionnary; //Dictionnaire liant un type d'arme a ses informations

    //Initialisation
    private void Start()
    {
        //On charge les informations des bases
        basesDictionnary = new Dictionary<TowerFabric.BaseType, GameObject>();
        foreach (BaseInfos info in basesInfos) basesDictionnary.Add(info.baseType, info.associatedGO);

        //On charge les informations des armes
        weaponsDictionnary = new Dictionary<TowerFabric.WeaponType, GameObject>();
        foreach (WeaponInfos info in weaponsInfos) weaponsDictionnary.Add(info.weaponType, info.associatedGO);

    }

    //Initialise une tour en fonction de son type de base et d'arme
    public void LoadInfos(GameObject tower, TowerFabric.BaseType baseType, TowerFabric.WeaponType weaponType)
    {
        //On recupere les informations de la base et on cree une nouvelle base en fonction
        GameObject towerBase = Instantiate(basesDictionnary[baseType], tower.transform);
        towerBase.name = "Base";

        //On recupere les informations de l'arme et on cree une nouvelle arme en fonction
        GameObject towerWeapon = Instantiate(weaponsDictionnary[weaponType], tower.transform);
        towerWeapon.name = "Weapon";

        //On ajoute un composant Tower a notre objet qui lie la base et l'arme
        tower.AddComponent<Tower>().Initialize(towerBase.GetComponent<TowerBase>(), towerWeapon.GetComponent<TowerWeapon>());
    }
}

//Objet definissant les informations d'un element de tour
[System.Serializable]
public class TowerElementInfos
{
    public GameObject associatedGO; //Objet a instancier pour un element particulier
}

//Objet definissant les informations d'une base
[System.Serializable]
public class BaseInfos : TowerElementInfos
{
    public TowerFabric.BaseType baseType; //Type de base concerne
}

//Objet definissant les informations d'une arme
[System.Serializable]
public class WeaponInfos : TowerElementInfos
{
    public TowerFabric.WeaponType weaponType; //Type d'arme concerne
}