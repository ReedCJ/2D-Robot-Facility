using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] theObjectToDestroy;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnDisable()
    {
        for (int i = 0; i <= theObjectToDestroy.Length; i++)
        {
            if (theObjectToDestroy[i])
                Destroy(theObjectToDestroy[i], 2f);
        }
    }
}
