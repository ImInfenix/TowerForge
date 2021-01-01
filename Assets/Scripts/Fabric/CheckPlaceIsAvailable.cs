//Script definissant si un emplacement est disponible pour la construction d'une nouvelle tour

using UnityEngine;

public class CheckPlaceIsAvailable : MonoBehaviour
{
    //Attributs pour l'affichage de si la place est disponible, assignés via l'inspecteur
    SpriteRenderer rangeRenderer;

    [SerializeField]
    Sprite canBePlacedSprite = null;
    [SerializeField]
    Sprite cannotBePlacedSprite = null;

    public bool CanBePlaced { get; private set; } //Etat actuel de si l'emplacement est disponible
    int numberOfColliders; //Nombre d'entites empechant actuellement le placement

    //Initialisation, par defaut la tour peut etre placee
    void Start()
    {
        rangeRenderer = GetComponent<SpriteRenderer>();
        numberOfColliders = 0;
        CanBePlaced = true;
        rangeRenderer.sprite = canBePlacedSprite;
    }

    void OnCollisionEnter2D(Collision2D collision) //Si on rencontre un objet empechant le placement
    {
        numberOfColliders++; //On l'ajoute au nombre d'entites bloquantes
        if(CanBePlaced) //Si on pouvait placer la tour
        {
            rangeRenderer.sprite = cannotBePlacedSprite; //On met à jour l'information visuelle
            CanBePlaced = false; //On en retire la possibilitee
        }
    }

    void OnCollisionExit2D(Collision2D collision) //Si un objet n'empeche plus la collision
    {
        numberOfColliders--; //On le retire des entites bloquantes
        if (numberOfColliders == 0) //Si plus aucune entite ne bloque
        {
            rangeRenderer.sprite = canBePlacedSprite; //On met à jour l'information visuelle
            CanBePlaced = true; //On rend la possibilite de placer la tour
        }
    }
}
