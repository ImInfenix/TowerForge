//Script definissant un boulet de canon

using UnityEngine;

public class Canonball : WeaponDamageDealer
{
    Animator animator; //Animation du boulet

    //Attributs pour la gestion de la trajectoire (fonction quadratique)
    private float a;
    private float b;
    private float timeAdvancement;
    private float TransitTime { get { return GameBalance.Canonball.transitTime; } }
    [Range(0,1)]
    private float advancement;
    float distX; //Distance de depart au point d'arrivee selon l'axe X
    Vector3 originPosition; //Point de depart

    //Mode deplacement
    delegate void AdvanceDelegate();
    AdvanceDelegate Advance;

    //Portee d'explosion du boulet de canon
    float ExplosionRadius { get { return GameBalance.Canonball.impactRange; } }

    bool dealtDamages; //Si les degats ont deja ete appliques

    //Initialisation
    private void Awake()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();

        advancement = 0.0f;
        Damages = GameBalance.Canonball.damages;
        dealtDamages = false;
    }

    void Update()
    {
        if(!dealtDamages) //Si les degats n'ont pas ete appliques
        {
            timeAdvancement += Time.deltaTime; //On compte le temps ecoule
            advancement = timeAdvancement / TransitTime; //On modifie le rapport de distance parcourue
            if (advancement > 1.0f) advancement = 1.0f; //Si on a depasse le temps de distance, on se remet au maximum
            Advance(); //On avance
            if (advancement == 1.0f) Explode(); //Si on a depasse le temps de distance, on fait exploser le boulet
        }
    }

    //Initialise la trajectoire du boulet
    public void Initialize(Vector3 targetPosition, Vector3 originPosition)
    {
        this.originPosition = originPosition; //Initialisation du point d'origine

        distX = targetPosition.x - transform.position.x; //Initialisation de la distance a parcourir selon l'axe X
        float distY = targetPosition.y - transform.position.y; //Initialisation de la distance a parcourir selon l'axe Y

        //Placement des points vises dans le repere
        float x2 = distX, y2 = distY;
        float x1 = x2 / 2, y1;

        // ==== Calcul de la trajectoire ===== //

        if (distX == 0) //Cas particulier : on tire seulement sur l'axe Y (pratiquement jamais utilise donc)
        {
            Advance = QuadraticWithoutXMove; //Choix de la fonction de trajectoire a utiliser
            x1 = 0.5f;
            x2 = 1;

            if(distY == 0)
            {
#if UNITY_EDITOR
                Debug.LogError("Weapon shot at its own position, should not be possible.");
#endif
                Destroy(gameObject);
            }

            if (distY > 0) y1 = y2;
            else y1 = 0;
        }
        else
        {
            if(distY > 0)
            {
                y1 = y2;
            }
            else if(distY < 0)
            {
                y1 = 0;
            }
            else
            {
                y1 = x1; //i.e. = x2 / 2;
            }

            Advance = QuadraticMove; //Choix de la fonction de trajectoire a utiliser
        }

        //Calcul des parametres de la fonction quadratique
        a = (y2 / x2 / x2 - y1 / x1 / x2) * (1 / (1 - x1 / x2));
        b = y1 / x1 - a * x1;

        gameObject.SetActive(true); //On active le boulet
    }

    //Fonction de deplacement de base
    void QuadraticMove()
    {
        //Calcul de la distance parcourue
        float relativeX = advancement * distX;
        Vector3 relativePosition = new Vector3(relativeX, a * relativeX * relativeX + b * relativeX, 0);
        //Mise a jour de la position
        transform.position = originPosition + relativePosition;
    }

    //Fonction de deplacement uniquement sur l'axe Y
    void QuadraticWithoutXMove()
    {
        //Calcul de la distance parcourue
        Vector3 relativePosition = new Vector3(0, a * advancement * advancement + b * advancement, 0);
        //Mise a jour de la position
        transform.position =  originPosition + relativePosition;
    }

    //Fait exploser le boulet de cannon
    private void Explode()
    {
        //Pour chaque ennemi a portee
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(transform.position, ExplosionRadius))
        {
            Ennemy ennemy = collider.gameObject.GetComponent<Ennemy>();
            if(ennemy != null) //Si l'ennemi existe bien
            {
                DealDamages(ennemy); //On lui inflige des degats
            }
        }

        PlaySoundOnEvent.Instance.PlayCanonballExplosion(); //On joue le bruit d'explosion du boulet de canon

        animator.SetTrigger("Explode"); //On joue l'animation d'explosion
        dealtDamages = true; //Mise a jour du satut de degats
    }


    //Method appelee a la fin de l'animation d'explosion, detruit le boulet
    void DeleteObject()
    {
        Destroy(gameObject);
    }
}
