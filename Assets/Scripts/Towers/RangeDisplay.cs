//Script gerant l'affichage des emplacements disponibles pour creer une tour

using UnityEngine;

public class RangeDisplay : MonoBehaviour
{
    public GameObject baseRangeObject; //Objet d'affichage d'emplacement associe

    //Affiche l'objet d'emplacement
    public void ShowBaseRange()
    {
        baseRangeObject.SetActive(true);
    }

    //Cache l'objet d'emplacement
    public void HideBaseRange()
    {
        baseRangeObject.SetActive(false);
    }
}
