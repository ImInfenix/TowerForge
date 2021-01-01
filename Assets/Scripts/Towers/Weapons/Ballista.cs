//Script definissant une balliste

using System.Collections.Generic;
using UnityEngine;

public class Ballista : TowerWeapon
{
    public GameObject projectilePrefab; //Projectile cree par une balliste

    bool isLoaded; //Si la balliste est chargee
    public List<Sprite> ballistaLoadedSprites; //Sprites de la balliste chargee
    public List<Sprite> ballistaEmptySprites; //Sprites de la balliste dechargee
    SpriteRenderer spriteRenderer; //Affichage de la balliste

    int nbSpritesLoaded; //Nombre d'images dans ballistaLoadedSprites
    int nbSpritesNotEmpty; //Nombre d'images dans ballistaEmptySprites
    int spriteIndex; //Sprite actuellement utilise

    //Attributs herites
    public override float TowerRange { get { return GameBalance.Ballista.range; } }
    protected override float FireDelay { get { return GameBalance.Ballista.fireDelay; } }

    protected override Comparer UsedComparer { get; set; } //Comparateur utiliser pour choisir quel ennemi attaquer

    //Initialisation
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UsedComparer = CompareFirst;

        isLoaded = true;
        nbSpritesLoaded = ballistaLoadedSprites.Count;
        nbSpritesNotEmpty = ballistaEmptySprites.Count;
        spriteIndex = 0;
    }

    protected override void Update()
    {
        base.Update();
        if(target != null) LookTarget(); //Si une cible est actuellement selectionnee, on la regarde
        if (!isLoaded && reloadTime >= FireDelay / 2) //Si la tour est prete a recharger
        {
            isLoaded = true; //On la recharge
            spriteRenderer.sprite = ballistaLoadedSprites[spriteIndex]; //On met a jour l'affichage
        }
    }

    //Fonction appellee lors d'un tir, renvoie l'objet de degat creer
    protected override WeaponDamageDealer Fire()
    {
        //On cree et initialise le projectile
        BallistaProjectile firedProjectile = Instantiate(projectilePrefab, transform).GetComponent<BallistaProjectile>();
        firedProjectile.Initialize(target.position); 
        isLoaded = false; //On decharge la balliste

        PlaySoundOnEvent.Instance.PlayBallistaFire(); //On joue le son de tire de balliste

        reloadTime = 0.0f; //On commence a recharger

        return firedProjectile;
    }

    //Oriente la balliste vers sa cible
    void LookTarget()
    {
        //Calcul de l'angle
        float rotationValue = Mathf.Atan((target.transform.position.y - transform.position.y) / (target.transform.position.x - transform.position.x));
        if (target.transform.position.x - transform.position.x < 0) rotationValue += Mathf.PI; //Rectification de l'angle
        float lookingAngle = rotationValue * Mathf.Rad2Deg; //Conversion de l'angle de radian vers degres

        //On met a jour l'affichage en fonction de si la balliste est chargee ou non et de l'angle trouve
        if (isLoaded)
        {
            nbSpritesLoaded = ballistaLoadedSprites.Count;
            spriteIndex = (int)((lookingAngle - 360.0f / nbSpritesLoaded / 2) / 360.0f * nbSpritesLoaded + nbSpritesLoaded) % nbSpritesLoaded;
            spriteRenderer.sprite = ballistaLoadedSprites[spriteIndex];
        }
        else
        {
            nbSpritesNotEmpty = ballistaEmptySprites.Count;
            spriteIndex = (int)((lookingAngle - 360.0f / nbSpritesNotEmpty / 2) / 360.0f * nbSpritesNotEmpty + nbSpritesNotEmpty) % nbSpritesNotEmpty;
            spriteRenderer.sprite = ballistaEmptySprites[spriteIndex];
        }
    }
}
