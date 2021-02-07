//Script definissant une tour

using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerBase TowerBase { get; private set;} //Base associee a la tour

    protected TowerWeapon towerWeapon; //Arme associee a la tour
    RangeIndicator rangeIndicator; //Objet d'affichage de la portee

    //Initialise une tour a l'aide de sa base et de son arme
    public void Initialize(TowerBase towerBase, TowerWeapon towerWeapon)
    {
        TowerBase = towerBase; //Reference la base
        this.towerWeapon = towerWeapon; //Reference l'arme
        rangeIndicator = GetComponentInChildren<RangeIndicator>(); //Reference l'objet de portee
        rangeIndicator.SetRange(towerWeapon.TowerRange); //Met a jour la portee

        towerBase.associatedTower = this; //Se definit comme tour de la base en entree
        towerWeapon.associatedTower = this; //Se definit comme tour de l'arme en entree
    }
}
