using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveNumber : MonoBehaviour
{
    public Text saveNumberText;
    public int saveNumber;

    private SaveableNPC[] saveableNPCs;
    private List<SaveableNPC> savedNPCs;  // Pour éviter de compter plusieurs fois le même PNJ

    // Start is called before the first frame update
    void Start()
    {
        saveNumber = 0;
        saveableNPCs = FindObjectsOfType<SaveableNPC>();  // Trouver tous les NPC au début
        savedNPCs = new List<SaveableNPC>();  // Liste pour suivre les NPC déjà sauvés
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SaveableNPC saveableNPC in saveableNPCs)
        {
            // Si le PNJ est prêt à être sauvé et n'a pas encore été comptabilisé
            if (saveableNPC.currentSaveTime >= saveableNPC.saveTimeRequired - 1 && 
                saveableNPC.currentLimitTime > 0 &&
                !savedNPCs.Contains(saveableNPC))
            {
                saveNumber += 1;  // Incrémente le nombre de citoyens sauvés
                savedNPCs.Add(saveableNPC);  // Ajoute le PNJ à la liste des sauvés
            }
        }
        
        // Mettre à jour l'UI
        saveNumberText.text = "Citizens saved: " + saveNumber;
    }
}
