//Script definissant un temple

using UnityEngine;

public class Temple : TowerBase
{
    public Animator criticDamagesDisplay;

    protected override void Start()
    {
        base.Start();
        effectFunction = ApplyCriticalEffect; //Enregistre l'effet de degats critique
    }

    //Applique un effet de degats critique a un objet de degat
    public void ApplyCriticalEffect(WeaponDamageDealer weaponDamageDealer, Ennemy ennemy)
    {
        float probability = Random.Range(0.0f, 1.0f);
        if(probability < GameBalance.Temple.critProbability)
        {
            weaponDamageDealer.Damages *= 2;
            criticDamagesDisplay.Play("BaseTempleCriticalDamageDisplay");
        }
    }
}
