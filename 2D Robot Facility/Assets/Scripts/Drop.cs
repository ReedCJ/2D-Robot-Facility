using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private GameObject drop;

    public GameObject DropObject()
    {
        return Instantiate(drop, this.transform.position, new Quaternion());
    }
}
