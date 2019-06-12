using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardController : MonoBehaviour
{
    public List<string> keycards = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        keycards.Add("none");

        GameMaster GM = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        if (GM.cards != null)
            for (int i = 0; i < GM.cards.Length; i++)
                keycards.Add(GM.cards[i]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collided with " + other.gameObject);
        if(other.gameObject.tag == "Keycard")
        {
            //Debug.Log("it's a keycard");
            if(other.gameObject.GetComponent<KeyCard>() != null)
            {
                //Debug.Log("it has a script");
                if(other.gameObject.GetComponent<KeyCard>().KeyCardcode == "Default")
                {
                    Debug.Log("Set Keycard Code");
                }
                else
                {
                    keycards.Add(other.gameObject.GetComponent<KeyCard>().KeyCardcode);
                    //Debug.Log("Code added");
                }
                Destroy(other.gameObject);
            }
        }
    }
}
