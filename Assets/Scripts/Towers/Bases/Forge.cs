//Script definissant une forge

public class Forge : TowerBase
{
    //Initialisation
    protected override void Start()
    {
        base.Start();
        effectFunction = ApplyBurningEffect; //Enregistre l'effet de feu
    }

    //Applique un effet de feu a un objet de degat
    public void ApplyBurningEffect(WeaponDamageDealer weaponDamageDealer, Ennemy ennemy)
    {
        ennemy.ApplyFire();
    }
}
