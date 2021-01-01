//Script definissant une taverne

public class Tavern : TowerBase
{
    //Initialisation
    protected override void Start()
    {
        base.Start();
        effectFunction = ApplyMoneyEffect; //Enregistre l'effet de feu
    }

    //Applique un effet de feu a un objet de degat
    public void ApplyMoneyEffect(WeaponDamageDealer weaponDamageDealer, Ennemy ennemy)
    {
        ennemy.ApplyIncreasedValue();
    }
}
