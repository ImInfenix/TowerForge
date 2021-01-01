//Script definissant un mortier

using UnityEngine;

public class Mortar : TowerWeapon
{
    Animator animator; //Animation du mortier
    public GameObject projectilePrefab; //Projectile du mortier

    //Attributs herites
    public override float TowerRange { get { return GameBalance.Mortar.range; } }
    protected override float FireDelay { get { return GameBalance.Mortar.fireDelay; } }

    protected override Comparer UsedComparer { get; set; } //Comparateur utiliser pour choisir quel ennemi attaquer

    //Initialisation
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        UsedComparer = CompareClosest;
    }

    //Fonction appellee lors d'un tir, renvoie l'objet de degat creer
    protected override WeaponDamageDealer Fire()
    {
        reloadTime = 0.0f; //On commence a recharger
        animator.SetTrigger("Fire"); //On declenche l'aniamtion
        GameObject projectile = Instantiate(projectilePrefab, transform); //On cree le projectile
        projectile.transform.position += Vector3.up; //On place le boulet au dessus du canon
        Canonball newCanonBall = projectile.GetComponent<Canonball>();
        newCanonBall.Initialize(target.position, projectile.transform.position); //On initialise le boulet de canon

        PlaySoundOnEvent.Instance.PlayMortarFire(); //On joue le son de tir de mortier
 
        return newCanonBall; //On renvoie l'objet de degat cree
    }    
}
