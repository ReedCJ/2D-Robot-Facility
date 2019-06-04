using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform leftEnd;      // The left end of the belt. Used for moving things to the left
    [SerializeField] private Transform rightEnd;     // The right end of the belt. Used for moving things to the right
    [SerializeField] private float speed;            // How fast the conveyor belt is moving and thus the rate that it displaces things
    [SerializeField] private bool direct;            // Direction of conveyor belt. true == right
#pragma warning restore 0649
    private bool active;            // Is the conveyor belt moving?

    // Start is called before the first frame update
    void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active)
        {
            Move(collision.gameObject);
        }
        else if (collision.tag == "Enemy1" && active)
        {
            Debug.Log("Enemy is on top.");
            if (collision.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
                if (!enemy.flying)
                    Move(collision.gameObject);
            }
        }
    }

    private void Move(GameObject character)                 // Move the passed in gameObject along the belt
    {
        Transform endPoint;
        if (!direct)
            endPoint = leftEnd;
        else
            endPoint = rightEnd;

        character.transform.position = Vector2.MoveTowards(character.transform.position, endPoint.position, speed * Time.deltaTime);
    }
}
