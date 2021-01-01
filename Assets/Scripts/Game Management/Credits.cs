//Script gerant l'affichage des credits

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    private enum CreditElement { Content, Category, Person } //Type d'element de credit

    const float delayBetweenFade = 0.01f; //Espacement dans le temps entre deux update lors de l'affichage / lorsque l'on cache les credits
    float screenHeight; //Hauteur de l'ecran
    float ScrollingLimits { get { return screenHeight / 2 + 150.0f; } } //Limites dans lesquelles on se place pour realiser l'affichage
    private const string creditsFilePath = "Misc/Credits"; //Emplacement du fichier de credits

    public GameObject creditTextCategoryPrefab; //Objet a instancier pour afficher une categorie
    public GameObject creditTextNamePrefab; //Objet a instancier pour afficher un nom
    public List<Image> imagesToFade; //Images a cacher lors de l'affichage
    public List<TMP_Text> textsToFade; //Textes a cacher lors de l'affichage
    public List<Button> buttonsToDisable; //Boutons a desactiver lors de l'affichage

    float currentAlpha; //Transparence actuelle des elemnts a cacher
    float currentlyDisplayedBoxes; //Nombre d'elements de credits actuellement affiches
    List<Tuple<CreditElement, string>> credits; //Liste des elements de credits a afficher
    Tuple<CreditElement, string>[] creditsArray; //Tableau des elements de credits a afficher

    const float scrollingStep = 2.0f; //Pas entre deux deplacements des credits

    //Initialisation
    private void Start()
    {
        currentAlpha = 1.0f; //On met l'alpha a 1
        screenHeight = Screen.height; //On recupere la taille de l'ecran
        credits = null;
    }

    //Fonction appellee lorsque le joueur clique sur le bouton credits
    public void StartCredits()
    {
        gameObject.SetActive(true); //On active les credits
        currentlyDisplayedBoxes = 0; //On initialise le nombre d'elements de credit affiches
        StartCoroutine(FadeImages()); //On cache les elements a cacher
    }

    //Applique la valeur currentAlpha a tous elements a cacher
    void ApplyAlpha() 
    {
        bool activeState = currentAlpha == 1.0f; //Si l'alpha n'est pas de 1, on desactive les boutons
        foreach (Button button in buttonsToDisable)
            button.enabled = activeState;

        foreach (Image image in imagesToFade) //On applique la valeur de alpha a toutes les images
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);

        foreach (TMP_Text text in textsToFade) //On applique la valeur de alpha a tous les textes
            text.color = new Color(text.color.r, text.color.g, text.color.b, currentAlpha);
    }

    //Affiche ou cache l'UI
    IEnumerator FadeImages(bool hide = true)
    {
        yield return new WaitForSeconds(delayBetweenFade); //On attend le temps predefinit entre deux update

        if(hide) //Si on veut cacher l'UI
        {
            currentAlpha -= delayBetweenFade; //On diminue alpha
            if (currentAlpha < 0.0f) currentAlpha = 0.0f; //On verifie qu'alpha n'aille pas en dessous de 0

            ApplyAlpha(); //On applique alpha

            if (currentAlpha == 0.0f) //Si alpha a atteint 0
            {
                RunCredits(); //On affiche les credits
                yield break; //On s'arrete la
            }
        }
        else //Si on veut afficher l'UI
        {
            currentAlpha += delayBetweenFade; //On augmente alpha
            if (currentAlpha > 1.0f) currentAlpha = 1.0f; //On verifie qu'alpha n'aille pas au dessus de 1

            ApplyAlpha(); //On applique alpha

            if (currentAlpha == 1.0f)  //Si alpha a atteint 1
            {
                gameObject.SetActive(false); //On desactive les credits
                yield break; //On s'arrete la
            }
        }

        StartCoroutine(FadeImages(hide)); //On rappel la fonction pour continuer
    }

    //Lance l'affichage des credits
    void RunCredits()
    {
        XmlDocument xmlDocument = new XmlDocument(); //On cree un nouveau document XML
        TextAsset creditsTextAsset = Resources.Load<TextAsset>(creditsFilePath);
        xmlDocument.LoadXml(creditsTextAsset.text); //On lit le fichier de credits

        if (credits == null) //Si les credits n'ont pas encore ete lus
        {
            credits = new List<Tuple<CreditElement, string>>(); //On initialise la liste des credits
            BuildCreditsDisplayFromXML(xmlDocument.DocumentElement); //On remplit la liste en lisant le document
            creditsArray = credits.ToArray(); //On passe la liste sous forme de tableau
        }

        StartCoroutine(DisplayCredits()); //On affiche les credits
    }

    //Affiche l'element de credit d'indice index
    IEnumerator DisplayCredits(int index = 0)
    {
        if (index >= creditsArray.Length) //Si on atteint la fin des credits
        {
            yield break; //on s'arrete la
        }

        GameObject toInstance = null;
        float delay = index == 0 ? 0.0f : 0.5f; //On charge la duree a attendre avant d'afficher le credit (0 si on demarre)

        switch(creditsArray[index].Item1) //On recupere l'objet a creer en fonction du type de credit a afficher
        {
            case CreditElement.Category:
                toInstance = creditTextCategoryPrefab;
                delay *= 2; //Si on affiche une categorie, on double le delai
                break;
            case CreditElement.Person:
                toInstance = creditTextNamePrefab;
                break;
        }

        yield return new WaitForSeconds(delay); //On attend

        CreateBox(creditsArray[index].Item2, toInstance); //On affiche l'element de credit concerne

        StartCoroutine(DisplayCredits(index + 1)); //On demarre l'affichage de l'element suivant
    }

    //Construit la liste des credits en fonction d'un fichier XML
    void BuildCreditsDisplayFromXML(XmlNode currentNode)
    {
        if (Enum.IsDefined(typeof(CreditElement), currentNode.Name)) //Si l'element de credit est bien defini
        {
            Enum.TryParse(currentNode.Name, false, out CreditElement elementType); //On recupere le type d'element de credit lu
            if(currentNode.Attributes.GetNamedItem("name") != null) //Si l'element a un attribut name
            {
                //On ajoute un nouvel element a notre liste de credits
                credits.Add(new Tuple<CreditElement, string>(elementType, currentNode.Attributes.GetNamedItem("name").Value));
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"{currentNode.Name} si not defined !");
#endif
            return;
        }

        foreach (XmlNode node in currentNode) //On parcours les enfants du noeud etudier
            BuildCreditsDisplayFromXML(node); //Et on construit les credits a partir de ces enfants
    }

    //Affiche un element d'UI
    void CreateBox(string elementToDisplay, GameObject prefabToUse)
    {
        currentlyDisplayedBoxes++; //On incremente le nombre d'elements d'UI exstant

        TMP_Text textBox = Instantiate(prefabToUse, transform).GetComponent<TMP_Text>(); //On cree l'objet associe a l'element
        textBox.text = elementToDisplay; //On met a jour le texte dans l'affichage

        //On place l'affichage en bas de l'ecran
        textBox.rectTransform.anchoredPosition = new Vector2(textBox.rectTransform.anchoredPosition.x, -ScrollingLimits);
        //On demarre le mouvement du nouvel objet cree
        StartCoroutine(MoveBox(textBox));
    }

    //Deplace un objet de credit vers le haut
    IEnumerator MoveBox(TMP_Text textBox)
    {
        //On met a jour la nouvelle position
        textBox.rectTransform.anchoredPosition = new Vector2(textBox.rectTransform.anchoredPosition.x, textBox.rectTransform.anchoredPosition.y + scrollingStep);

        //Si on atteint les limites de la zone definit
        if(textBox.rectTransform.anchoredPosition.y > ScrollingLimits)
        {
            currentlyDisplayedBoxes--; //On decremente le nombre d'elements de credits existant
            if(currentlyDisplayedBoxes == 0) //Si plus aucun element n'est affiche
                StartCoroutine(FadeImages(false)); //On reaffiche l'UI
            Destroy(textBox.gameObject); //on detruit l'objet
            yield break; //On s'arrete la
        }

        yield return new WaitForSeconds(0.01f); //On attend la duree defini

        StartCoroutine(MoveBox(textBox)); //On rebouge l'objet
    }

}
