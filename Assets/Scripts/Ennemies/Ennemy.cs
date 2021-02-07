//Script definissant un ennemi

using System.Collections;
using UnityEngine;

public abstract class Ennemy : MonoBehaviour
{
    //Attributs pour la gestion de vague
    protected Track followedTrack = null;
    [SerializeField]
    protected Wave wave;

    //Attributs globaux
    protected abstract float Speed { get; } //Vitesse de deplacement
    protected abstract float MaxHealth { get; } //Vie maximale de l'ennemi
    protected float health; //Vie actuelle de l'ennemi
    protected Transform target; //Position de destination actuelle
    public int TargetIndex { get; set; } //Point suivi actuellement dans la vague
    protected abstract float Damages { get; } //Degats infliges au joueur si l'ennemi atteint la fin du chemin
    public float moneyEarnedOnDeath; //Argent donnee a la mort

    protected float distanceEpsilon = 0.1f; //Precision necessaire pour considerer avoir atteint une destination
    bool isGoingRight; //Defines if the ennemy is currently going left of right
    SpriteRenderer spriteRenderer; //Atatched sprite render, use to flip x axis

    //Objets d'affichage de la vie, assignés via l'inspecteur
    [SerializeField]
    private GameObject healthBar = null;
    [SerializeField]
    private GameObject healthBarRemainingHealth = null;

    //Gestion des effets sur la duree
    [HideInInspector]
    public bool isPoisoned;
    [HideInInspector]
    public uint remainingBurningTicks;
    [HideInInspector]
    public float increasedRewardLimit;

    //Gestion visuelle des effets surla duree
    public Animator poisonAnimator;
    public Animator fireAnimator;
    public Animator goldAnimator;

    [HideInInspector]
    public bool isSpawnedByEnnemy = false; //Defini si l'ennemi est apparu normalement ou s'il a ete cree par un autre ennemi

    protected virtual void Awake()
    {
        healthBar.SetActive(false); //On cache la barre de vie si l'ennemi a toute sa vie
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Initialisation
    protected virtual void Start()
    {
        if(!isSpawnedByEnnemy) TargetIndex = 0; //On place l'ennemi en debut de vague seulement s'il est apparu normalement
        target = followedTrack.waypoints[TargetIndex];

        isGoingRight = transform.position.x < target.position.x;
        spriteRenderer.flipX = isGoingRight;

        isPoisoned = false;
        remainingBurningTicks = 0;
        increasedRewardLimit = -1;

        health = MaxHealth;
    }

    protected virtual void Update()
    {
        //Gestion du deplacement
        if (Vector3.Distance(target.position, transform.position) < distanceEpsilon)
        {
            TargetIndex++;

            if(TargetIndex < followedTrack.waypoints.Length)
            {
                target = followedTrack.waypoints[TargetIndex]; //Une fois un point atteint, on passe au suivant

                bool isNowGoingRight = transform.position.x < target.position.x;
                if (isGoingRight != isNowGoingRight)
                {
                    isGoingRight = isNowGoingRight;
                    spriteRenderer.flipX = isGoingRight;
                }

            }
            else
            {
                EndTravel(); //Si on a atteint le dernier point, on applique les effets de fin trajet
            }
        }

        //On update la position
        transform.position += (target.position - transform.position).normalized * Time.deltaTime * Speed;

        //Code non utiliser pour le moment, gestion de la vitesse (suppose ralentissement)
        //transform.position += (target.position - transform.position).normalized * Time.deltaTime * Speed * (Time.time < increasedRewardLimit ? GameBalance.Tavern.bountyIncreaseAmount : 1);
    }

    //Application des effets de fin trajet
    void EndTravel()
    {
        GameManager._Instance.playerStats.TakeDamages(Damages); //On inflige les degats au joueur
        wave.RemoveEnnemy(this); //On retire l'ennemi de la vague
        Destroy(gameObject); //On retire l'ennemi du jeu
    }

    //Associe un chemin a un ennemi
    public void SetFollowedTrack(Track track, Wave wave)
    {
        if(followedTrack == null)
        {
            followedTrack = track;
        }
        else
        {
            Debug.LogError("Multiple Track Path detected added to the ennemy " + name + " /!\\");
        }

        this.wave = wave;
    }

    //Applique des degats a l'ennemi
    public void TakeDamages(float damages)
    {
        if (health <= 0) return; //Si l'ennemi a une vie negative, la mort est deja en train d'etre geree

        health -= damages; //On inflige les degats
        if(health < MaxHealth) //Si l'ennemi n'a plus toute sa vie, on ajuste la barre de vie en fonction
        {
            healthBar.SetActive(true);
            healthBarRemainingHealth.transform.localScale = new Vector3(health / MaxHealth, 1, 1);
        }
        if (health <= 0) Die();
    }

    //Gestion de la mort d'un ennemi
    protected virtual void Die()
    {
        //On verifie si l'ennemi a sa valeur augmentee et on calcul combien gagne le joueur
        int reward = Mathf.CeilToInt(moneyEarnedOnDeath * (increasedRewardLimit > Time.time ? GameBalance.Tavern.bountyIncreaseAmount : 1));
        GameManager._Instance.playerStats.AddMoney(reward); //On donne l'argent au joueur
        wave.RemoveEnnemy(this); //On retire l'ennemi de la vague

        GetComponent<Collider2D>().enabled = false; //On desactive le collider pour que l'ennemi ne soit plus visible du reste du jeu
        healthBar.SetActive(false); //on cache la barre de vie

        CancelInvoke(); //On arrete les appels reguliers (type degats sur la duree)
    }

    //Applique l'effet de ralentissement sur un ennemi
    public void ApplyIncreasedValue()
    {
        increasedRewardLimit = Time.time + GameBalance.Tavern.rewardDuration;
        goldAnimator.SetBool("IsRewardBigger", true);
        Invoke(nameof(CheckGoldAnimation), GameBalance.Tavern.rewardDuration);
    }

    void CheckGoldAnimation()
    {
        if (increasedRewardLimit > Time.time) return;
        goldAnimator.SetBool("IsRewardBigger", false);
    }

    //Applique l'effet de feu sur un ennemi
    public void ApplyFire()
    {
        if (remainingBurningTicks == 0) //Si l'ennemi n'est pas enflamm
        {
            StartCoroutine(TakeBurningDamages()); //Il commence a prendre des degats de feu
            fireAnimator.SetBool("IsBurning", true); //On declenche l'animation
        }

        remainingBurningTicks = GameBalance.Forge.burningTicks; //On met a jour le nombre de ticks de degats restants dans tous les cas
    }

    //Applique des degats de feu a l'ennemi
    public IEnumerator TakeBurningDamages()
    {
        yield return new WaitForSeconds(GameBalance.Forge.burningTick); //On attend avant d'appliquer le tick suivant

        remainingBurningTicks--; //On retire un tick restant
        TakeDamages(GameBalance.Forge.burningDamage); //On applique les degats

        if (remainingBurningTicks > 0 && health > 0)    //Si l'ennemi brule et vie encore, on rappel la fonction
            StartCoroutine(TakeBurningDamages());
        else                                            //Sinon on arrete l'animation
            fireAnimator.SetBool("IsBurning", false);
    }

    //Applique l'effet de poison sur un ennemi
    public void StartPoison()
    {
        if (isPoisoned) return; //Si l'ennemi est deja empoisonne, pas besoin de continuer

        isPoisoned = true; //On declare l'ennemi comme empoisonne
        poisonAnimator.SetBool("IsPoisoned", true); //On declench l'animation
        InvokeRepeating(nameof(TakePoisonDamage), GameBalance.Laboratory.poisonTick, GameBalance.Laboratory.poisonTick); //On applique les degats regulierement
    }

    //Applique des degats de poison a l'ennemi
    private void TakePoisonDamage()
    {
        TakeDamages(GameBalance.Laboratory.poisonDamage); //Applique les degats
    }
}
