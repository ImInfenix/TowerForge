//Script gerant l'affichage de la portee d'une tour

using UnityEngine;
using UnityEngine.EventSystems;

public class RangeIndicator : MonoBehaviour
{
    SpriteRenderer spriteRenderer = null; //Affichage de la portee
    public bool IsSelected { get; private set; } //Si l'objet est actuellement selectionne, pas utilise actuellement mais le sera plus tard
    public bool isBeingPlaced = false; //Si l'objet est en train d'etre place ou s'il l'est deja

    private void Update()
    {
        //Si l'utilisateur clique sur l'objet, affiche la portee de celui-ci
        //S'il clique a cote, cache la portee
        if(!isBeingPlaced && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(GameManager._Instance.gameCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Range"));

            if (rayHit) SetSelected(rayHit.transform.gameObject == gameObject);
            else SetSelected(false);
        }
    }

    //Mais a jour la portee a afficher
    public void SetRange(float range)
    {
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(range * 0.64f, range * 0.9f, 1.0f);
        SetSelected(false);
    }

    //Defini si l'objet est selectionne
    public void SetSelected(bool selected)
    {
        IsSelected = selected; //Not used for now
        spriteRenderer.enabled = selected;
    }
}
