using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Translator : MonoBehaviour
{
    public static Translator instance { get; private set; }

    public enum LanguageTag { FR, EN, None, New}

    public LanguageTag defaultLanguage = LanguageTag.EN;
    private LanguageTag _currentLanguage;
    public LanguageTag currentLanguage
    {
        get { return _currentLanguage; }
        private set
        {
            _currentLanguage = value;
            InitiateTranslation();
            UpdateAllTexts();
        }
    }
    public TextTranslation[] files;
    Dictionary<string, string> loadedWords;
    List<TranslatedText> textToUpdate;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        textToUpdate = new List<TranslatedText>();

        currentLanguage = defaultLanguage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) currentLanguage = LanguageTag.FR;
        if (Input.GetKeyDown(KeyCode.Z)) currentLanguage = LanguageTag.EN;
        if (Input.GetKeyDown(KeyCode.E)) currentLanguage = LanguageTag.None;
    }

    void InitiateTranslation()
    {
        DontDestroyOnLoad(gameObject);

        XmlDocument doc = new XmlDocument();
        foreach(TextTranslation tt in files)
        {
            if(tt.language == _currentLanguage)
            {
                doc.LoadXml(tt.file.text);
                break;
            }
        }


        loadedWords = new Dictionary<string, string>();

        if(doc.DocumentElement != null) LoadDocument(doc);

        //foreach (string s in loadedWords.Keys)
        //    Debug.Log($"{s} - {loadedWords[s]}");
    }

    void LoadDocument(XmlDocument doc)
    {
        if (doc.DocumentElement.Name != "Content") return;

        foreach (XmlNode node in doc.DocumentElement)
        {
            if (node.Name != "Category") continue;

            LoadNode(node);
        }
    }

    void LoadNode(XmlNode node)
    {
        switch (node.Name)
        {
            case "Category":
                foreach (XmlNode nodeC in node.ChildNodes)
                {
                    if (nodeC.Name != "Category" && nodeC.Name != "Text") continue;
                    LoadNode(nodeC);
                }
                break;

            case "Text":
                string completeTag = "";
                XmlNode parentNode = node.ParentNode;
                while(parentNode.Name == "Category")
                {
                    completeTag = $"{parentNode.Attributes.GetNamedItem("name").Value}.{completeTag}";
                    parentNode = parentNode.ParentNode;
                }
                completeTag = completeTag + node.Attributes.GetNamedItem("tag").Value;
                string value = node.Attributes.GetNamedItem("text").Value;
                if (loadedWords.ContainsKey(completeTag))
                {
                    Debug.LogError($"The key \"{completeTag}\" already exist with value \"{loadedWords[completeTag]}\". Tried to create new one with value \"{value}\".");
                    break;
                }
                loadedWords.Add(completeTag, value);
                break;
        }
    }

    public void SubscribeForUpdate(TranslatedText translatedText)
    {
        UpdateText(translatedText);
        textToUpdate.Add(translatedText);
    }

    public void UnsubscribeForUpdates(TranslatedText translatedText)
    {
        textToUpdate.Remove(translatedText);
    }

    private void UpdateAllTexts()
    {
        foreach(TranslatedText tt in textToUpdate)
        {
            UpdateText(tt);
        }
    }

    private void UpdateText(TranslatedText translatedText)
    {
        if (!loadedWords.ContainsKey(translatedText.textTranslationTag))
        {
            translatedText.associatedText.text = translatedText.textTranslationTag;
            return;
        }

        translatedText.associatedText.text = loadedWords[translatedText.textTranslationTag];
    }
}

[System.Serializable]
public struct TextTranslation
{
    public TextAsset file;
    public Translator.LanguageTag language;
}