using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour
{
    TextMeshProUGUI textField;

    public LocalisedString localisedString;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = localisedString.value;
    }
}