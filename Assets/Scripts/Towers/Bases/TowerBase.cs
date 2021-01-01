using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    public Tower associatedTower; //Tour associee a cette base

    public delegate void BaseEffect(WeaponDamageDealer weaponDamageDealer, Ennemy ennemy); //Signature d'un effet de base
    protected BaseEffect effectFunction; //Effet a appliquer aux objets de degats crees par cette tour

    protected virtual void Start()
    {

    }

    //Associe l'effet de degat de la tour a un objet de degat
    public void SetWeaponDamageDealerEffect(WeaponDamageDealer weaponDamageDealer)
    {
        weaponDamageDealer.damageDealerEffect = effectFunction;
    }

}
