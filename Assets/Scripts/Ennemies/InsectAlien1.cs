//Script definissant un l'ennemi InsectAlien1

using UnityEngine;

public class InsectAlien1 : Ennemy
{
    //Attributs herites
    protected override float Speed { get { return GameBalance.InsectAlien1.speed; } }
    protected override float MaxHealth { get { return GameBalance.InsectAlien1.health; } }
    protected override float Damages { get { return GameBalance.InsectAlien1.damages; } }

    Animator animator;

    //Initialisation
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        moneyEarnedOnDeath = GameBalance.InsectAlien1.moneyEarnedOnDeath;
    }

    //Gestion de la mort
    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Die"); //On declenche l'animation de mort
    }

    //Method appelee a la fin de l'animation de mort, retire l'ennemi du jeu
    void RealDeath()
    {
        Destroy(gameObject);
    }
}
