using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    [SerializeField] private GameObject unlockDoor;

    public void DoAllTheThings()
    {
        UnlockDoor();
    }

    public void UnlockDoor()
    {
        if(unlockDoor != null)
        {
            unlockDoor.GetComponent<DoorController>().Unlock();
            unlockDoor.GetComponent<DoorController>().LockLeft = false;
            unlockDoor.GetComponent<DoorController>().LockRight = false;
        }
    }
}
