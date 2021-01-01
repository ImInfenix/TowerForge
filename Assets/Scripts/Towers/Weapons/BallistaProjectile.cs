//Script definissant un carreau de balliste

using System.Collections;
using UnityEngine;

public class BallistaProjectile : WeaponDamageDealer
{
    //Attributs herites
    float BallistaProjectileSpeed { get { return GameBalance.BallistaProjectile.projectileSpeed; } }
    float ProjectileTimeout { get { return GameBalance.BallistaProjectile.projectileTimeout; } }

    bool dealtDamages; //Si le projectile a inflige des degats (pour ne pas toucher plusieurs cibles avec un seul carreau)

    //Initialisation
    protected override void Start()
    {
        base.Start();
        Damages = GameBalance.BallistaProjectile.damages;
        dealtDamages = false;
        StartCoroutine(DestroyProjectile()); //On appel la fonction de destruction du projectile apres un delai
    }

    //Initialise la force appliquee et la rotation du projectile en fonction de se destination
    public void Initialize(Vector3 targetPosition)
    {
        Vector3 towardTarget = (targetPosition - transform.position).normalized; //Vecteur direction
        GetComponent<Rigidbody2D>().AddForce(towardTarget * BallistaProjectileSpeed); //Ajout d'une force au projectile
        //Calcul de la rotation
        float zAxisRotation = Mathf.Atan((targetPosition.y - transform.position.y) / (targetPosition.x - transform.position.x));
        if (targetPosition.x < transform.position.x) zAxisRotation += Mathf.PI; //Correction de l'angle
        //Mise a jour de la rotation
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + zAxisRotation * Mathf.Rad2Deg);
    }

    //Si le projectile entre en collision avec un objet (uniquement possible avec un ennemi, Project Settings -> Physic 2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!dealtDamages) //Si les degats n'ont pas deja ete infliges
        {
            DealDamages(collision.gameObject.GetComponent<Ennemy>()); //On les applique
            dealtDamages = true;
            Destroy(gameObject); //On detruit le projectile
        }
    }

    IEnumerator DestroyProjectile() //Detruit le projectile apres un certain delai
    {
        yield return new WaitForSeconds(ProjectileTimeout);
        Destroy(gameObject);
    }
}
