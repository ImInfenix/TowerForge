using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TranslatedText : MonoBehaviour
{
    public string textTranslationTag;
    public TMP_Text associatedText { get; private set; }

    private void Start()
    {
        Translator translator = Translator.instance;
        if (translator == null) return;

        associatedText = GetComponent<TMP_Text>();
        translator.SubscribeForUpdate(this);
    }

    private void OnDestroy()
    {
        Translator translator = Translator.instance;
        if (translator == null) return;
        translator.UnsubscribeForUpdates(this);
    }
}
