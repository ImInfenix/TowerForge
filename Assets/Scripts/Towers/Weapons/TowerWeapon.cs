//Script definissant une arme de tour

using UnityEngine;

public abstract class TowerWeapon : MonoBehaviour
{
    public Tower associatedTower; //Tour associee a l'arme

    protected Transform target; //Cible
    Collider2D[] closeEnnemies; //Ennemis proches
    LayerMask ennemiesLayer;    //Couche sur laquelle se trouvent les ennemis

    //Attributs globaux
    public abstract float TowerRange { get; }
    protected abstract float FireDelay { get; }
    public float FireRate { get { return 1 / FireDelay; } }
    protected float reloadTime;

    protected delegate bool Comparer(GameObject g1, GameObject g2); //Definit un comparateur entre deux objets
    protected abstract Comparer UsedComparer { get; set; } //Comparateur utiliser pour choisir quel ennemi attaquer

    //Initialisation
    protected virtual void Awake()
    {
        ennemiesLayer = LayerMask.GetMask("Ennemies");
        reloadTime = FireDelay;
        closeEnnemies = new Collider2D[50];
    }

    //Initialisation
    protected virtual void Start()
    {
        InvokeRepeating(nameof(FindTarget), 0.0f, 0.5f);
    }

    protected virtual void Update()
    {
        reloadTime += Time.deltaTime; //On recharge l'arme en fonction du temps
        if (reloadTime >= FireDelay && target != null) //Si on est pret a tirer et qu'on a une cible
        {
            WeaponDamageDealer weaponDamageDealer = Fire(); //On tire
            SetDamageDealerEffect(weaponDamageDealer); //On applique l'effet de la base de la tour a l'objet de degat cree
        }
    }

    //Affiche la portee d'une tour dans l'onglet scene de l'editeur de Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TowerRange);
    }

    //Cherche une cible a viser
    void FindTarget()
    {
        //On recherche tous les ennemis dans un cercle de la taille de la portee de la tour
        int foundEnnemies = Physics2D.OverlapCircleNonAlloc(transform.position, TowerRange, closeEnnemies, ennemiesLayer);
        int index = 0;
        //On cherche l'ennemi repondant au mieu au critere de comparaison choisi
        for (int i = 1; i < foundEnnemies; i++)
        {
            if (UsedComparer(closeEnnemies[i].gameObject, closeEnnemies[index].gameObject))
            {
                index = i;
            }
        }

        //Si on a trouve un ennemi et qu'il est bien dans la portee de la tour, on le definit comme cible
        if (foundEnnemies > 0 && (closeEnnemies[index].transform.position - transform.position).magnitude <= TowerRange)
        {
            target = closeEnnemies[index].transform;
        }
        else target = null; //Sinon, on met la cible a null
    }

    //Fonction appellee lors d'un tir, renvoie l'objet de degat creer
    protected abstract WeaponDamageDealer Fire();

    //Applique l'effet de la base de la tour a un objet de degat
    protected void SetDamageDealerEffect(WeaponDamageDealer weaponDamageDealer)
    {
        associatedTower.TowerBase.SetWeaponDamageDealerEffect(weaponDamageDealer);
    }

    //Compare deux ennemies, renvoie true si g1 est plus proche que g2
    protected bool CompareClosest(GameObject g1, GameObject g2)
    {
        return Vector2.Distance(transform.position, g1.transform.position)
                    < Vector2.Distance(transform.position, g2.transform.position);
    }

    //Compare deux ennemies, renvoie true si g1 est plus loin que g2 de l'arrivee du chemin
    protected bool CompareFirst(GameObject g1, GameObject g2)
    {
        return g1.GetComponent<Ennemy>().TargetIndex > g2.GetComponent<Ennemy>().TargetIndex;
    }

    //Compare deux ennemies, renvoie true si g1 est plus proche que g2 du depart du chemin
    protected bool CompareLast(GameObject g1, GameObject g2)
    {
        return !CompareFirst(g1, g2);
    }
}
