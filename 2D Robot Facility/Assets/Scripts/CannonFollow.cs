using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFollow : MonoBehaviour
{
     private GameObject target;
     private Vector3 targetPoint;
     private Quaternion targetRotation;
 
     void Start () 
     {
         target = GameObject.FindWithTag("Player");
     }
 
     void Update()
     {

        Vector3 pos = transform.InverseTransformPoint(target.transform.position);
        pos.x = 0;
        float angle = Vector3.Angle(Vector3.up, pos);
        angle = pos.y < 0 ? -angle : angle;
        transform.Rotate(Vector3.right, angle, Space.Self);
        //         targetPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        //         targetRotation = Quaternion.LookRotation (-targetPoint, Vector3.up);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }
}
