//Script definissant un l'ennemi InsectAlien3

using UnityEngine;

public class Queen : Ennemy
{
    //Attributs herites
    protected override float Speed { get { return GameBalance.Queen.speed; } }
    protected override float MaxHealth { get { return GameBalance.Queen.health; } }
    protected override float Damages { get { return GameBalance.Queen.damages; } }

    [HideInInspector]
    public GameObject spawnedEnnemyAtHatch;
    float IncubationDuration { get { return GameBalance.Queen.incubationDuration; } }
    float timeAtSpawn;

    Animator animator;

    //Initialisation
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        moneyEarnedOnDeath = GameBalance.Queen.moneyEarnedOnDeath;
    }

    protected override void Start()
    {
        base.Start();
        timeAtSpawn = Time.time; //On met a jour l'heure a laquelle l'incubation commence
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - timeAtSpawn >= IncubationDuration) //On verifie si l'incubation est terminee
        {
            wave.SpawnFromEnnemy(this, spawnedEnnemyAtHatch, false); //Si c'est le cas, on lance l'eclosion
            timeAtSpawn = Time.time; //On prepare un nouvel ennemi
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
