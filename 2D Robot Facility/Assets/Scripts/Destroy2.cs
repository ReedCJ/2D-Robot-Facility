using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy2 : MonoBehaviour
{
    public GameObject[] theObjectToDestroy;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {
        for (int i =0; i <= theObjectToDestroy.Length; i++)
        {
            if(theObjectToDestroy[i])
            Destroy(theObjectToDestroy[i], 2f);
        }
        
    }
}
