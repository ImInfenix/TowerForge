//Script definissant un l'ennemi InsectAlienEgg

using UnityEngine;

public class Egg : Ennemy
{
    //Attributs herites
    protected override float Speed { get { return GameBalance.Egg.speed; } }
    protected override float MaxHealth { get { return GameBalance.Egg.health; } }
    protected override float Damages { get { return GameBalance.Egg.damages; } }

    //Attributs de l'oeuf
    [HideInInspector]
    public GameObject spawnedEnnemyAtHatch; //Ennemi apparaissant a l'eclosion
    float IncubationDuration { get { return GameBalance.Egg.incubationDuration; } } //Duree d'incubation
    float timeAtSpawn; //Heure a laquelle l'oeuf apparait (par rapport au lancement du programme)

    Animator animator;

    //Initialisation
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        moneyEarnedOnDeath = GameBalance.Egg.moneyEarnedOnDeath;
    }

    protected override void Start()
    {
        base.Start();
        timeAtSpawn = Time.time; //On met a jour l'heure a laquelle l'incubation commence
    }

    protected override void Update()
    {
        base.Update();

        if(Time.time - timeAtSpawn >= IncubationDuration) //On verifie si l'incubation est terminee
        {
            wave.SpawnFromEnnemy(this, spawnedEnnemyAtHatch, true); //Si c'est le cas, on lance l'eclosion
        }
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
