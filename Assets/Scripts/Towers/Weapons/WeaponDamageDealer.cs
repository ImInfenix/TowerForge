//Script definissant un objet de degats

using UnityEngine;

public abstract class WeaponDamageDealer : MonoBehaviour
{
    public TowerBase.BaseEffect damageDealerEffect; //Effet appliquer lors des degats

    public float Damages { get; set; } //Degats infliges

    protected virtual void Start()
    {

    }

    //Applique les degats a un ennemi
    protected virtual void DealDamages(Ennemy ennemy)
    {
        damageDealerEffect(this, ennemy); //Application de l'effet a l'objet de degats
        ennemy.TakeDamages(Damages); //Application des degats
    }
}
