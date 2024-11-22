using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPwithIcons : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // Texte du tutoriel
    public TMP_SpriteAsset universalSpriteAsset; // Sprite Asset unique regroupant toutes les icônes

    // Dictionnaire de correspondances pour chaque type de contrôleur
    public Dictionary<string, Dictionary<string, int>> deviceMappings = new Dictionary<string, Dictionary<string, int>>()
    {
        {
            "xbox", new Dictionary<string, int>()
            {
                { "Joystick", 26 },   
                { "Pad", 25 }, 
                { "A", 21 },
                { "X", 33 }, 
                { "Up", 28 },
                { "Down", 27 },
                { "RT", 20 },
                { "Y", 32 },
                { "B", 22 },
                { "LB", 18 },
                { "Right Stick Button", 31 },
                { "LT", 19 },
            }
        },
        {
            "playstation", new Dictionary<string, int>()
            {
                { "Joystick", 5 },   
                { "Pad", 7 }, 
                { "A", 3 },
                { "X", 14 }, 
                { "Up", 6 },
                { "Down", 8 },
                { "RT", 11 },
                { "Y", 15 },
                { "B", 2 },
                { "LB", 0 },
                { "Right Stick Button", 13 },
                { "LT", 4 },
            }
        },
        {
            "keyboard", new Dictionary<string, int>()
            {
                { "Joystick", 5 },   
                { "Pad", 7 }, 
                { "A", 3 },
                { "X", 14 }, 
                { "Up", 6 },
                { "Down", 8 },
                { "RT", 11 },
                { "Y", 15 },
                { "B", 2 },
                { "LB", 0 },
                { "Right Stick Button", 13 },
                { "LT", 4 },
            }
        }
    };

    // Méthode pour mettre à jour le texte
    public void UpdateTutorialText(string deviceType)
    {
        // Vérifie si le type de contrôleur est valide
        if (!deviceMappings.ContainsKey(deviceType))
        {
            Debug.LogWarning($"Device type '{deviceType}' non reconnu !");
            return;
        }

        // Obtenez le texte brut
        string rawText = tutorialText.text;

        // Obtenez le mapping correspondant
        var currentMapping = deviceMappings[deviceType];

        // Parcourir le texte et remplacer les tags
        string updatedText = ReplaceTagsWithSprites(rawText, currentMapping);

        // Appliquer le texte et le Sprite Asset
        tutorialText.text = updatedText;
        tutorialText.spriteAsset = universalSpriteAsset;
    }

    // Méthode pour remplacer les tags [x], [pad], etc., par des balises <sprite>
    public string ReplaceTagsWithSprites(string rawText, Dictionary<string, int> mapping)
    {
        foreach (var entry in mapping)
        {
            string tag = $"[{entry.Key}]"; // Exemple : [x]
            string spriteTag = $"<sprite={entry.Value}>"; // Exemple : <sprite=0>
            rawText = rawText.Replace(tag, spriteTag);
        }
        return rawText;
    }
}
