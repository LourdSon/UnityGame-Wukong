using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSavedText : MonoBehaviour
{
    public SaveableNPC saveableNPC;
    public Text text;
    public GameObject[] npcfirst;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        
    }
    void Update()
    {
        StartCoroutine(NPCtext());
    }
    // Update is called once per frame
    private IEnumerator NPCtext()
    {
        npcfirst = GameObject.FindGameObjectsWithTag("NPCs");
        if (npcfirst.Length > 0)
        {
            
            text.text = "A citizen needs your help, Go save him !";
        }
        
        else if(npcfirst.Length == 0)
        {
            text.text = "";
        }

        yield return null;
    }
}
