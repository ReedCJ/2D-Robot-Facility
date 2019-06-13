using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy2 : MonoBehaviour
{
    public GameObject theObjectToDestroy;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
        Destroy(theObjectToDestroy);
    }
}
