using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickup : MonoBehaviour {
    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(Transform child in inventoryPanel.transform)
        {
            if(child.gameObject.tag == collision.gameObject.tag)
            {
                string c = child.GetChild(0).GetComponent<Text>().text;
                int tcount = int.Parse(c) + 1;
                child.GetChild(0).GetComponent<Text>().text = "" + tcount;
                return;
            }

            if (collision.gameObject.tag == "Monster")
            {
                if (child.gameObject.tag == "Spear")
            {
               
                    string c = child.GetChild(0).GetComponent<Text>().text;
                    int tcount = int.Parse(c) - 1;
                    child.GetChild(0).GetComponent<Text>().text = "" + tcount;
                    return;
                }
            }
            if (collision.gameObject.tag == "Pit")
            {
                if (child.gameObject.tag == "Ladder")
                {
                    string c = child.GetChild(0).GetComponent<Text>().text;
                    int tcount = int.Parse(c) - 1;
                    child.GetChild(0).GetComponent<Text>().text = "" + tcount;
                    return;
                }
            }
        }

        GameObject i;
                
        if(collision.tag == "Ladder")
        {
            i = Instantiate(inventoryIcons[0]);
            i.transform.SetParent(inventoryPanel.transform, false);
        }
        else if (collision.tag == "Spear")
        {
            i = Instantiate(inventoryIcons[1]);
            i.transform.SetParent(inventoryPanel.transform, false);
        }

    }
}

