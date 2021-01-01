//Script definissant un laboratoire

public class Laboratory : TowerBase
{
    protected override void Start()
    {
        base.Start();
        effectFunction = ApplyPoisonEffect; //Enregistre l'effet de poison
    }

    //Applique un effet de poison a un objet de degat
    public void ApplyPoisonEffect(WeaponDamageDealer weaponDamageDealer, Ennemy ennemy)
    {
        ennemy.StartPoison();
    }
}
