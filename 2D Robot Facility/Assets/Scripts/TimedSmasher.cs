using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSmasher : MonoBehaviour
{
    private InstantDeath hazard;
    private Animator animate;
    private bool raising;           // Is it currently retract back up?
    private bool lowering;          // Is it currently lowering down to the ground?
    private bool raised;            // Is the compactor currently fully raised?
    private bool lowered;           // Is the compactor currently fully lowered?
    private float timer;            // Keeps track of how long to hold in a raised or lowered position
#pragma warning disable 0649
    [SerializeField] private float timeUp;          // Time it spends staying still after retracting back to the top
    [SerializeField] private float timeDown;        // Time it spends staying still after extending to the max range
    [SerializeField] private float slamTime;        // How long it takes to finish the slam animation
    [SerializeField] private float retractTime;     // How long it takes to finish the retract animation
    [SerializeField] private float Offset;          // How long until it starts moving
    [SerializeField] private bool initialState;     // Does it start raised or lowered? true == raised
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        raised = initialState;
        lowered = !initialState;
        raising = false;
        lowering = false;
        hazard = GetComponentInChildren<InstantDeath>();
        animate = GetComponent<Animator>();
        animate.updateMode = AnimatorUpdateMode.AnimatePhysics;
        animate.SetFloat("SlamTime", slamTime);
        animate.SetFloat("RetractTime", retractTime);
        hazard.active = false;
        if (initialState)
        {
            animate.SetTrigger("Retract");
            timer = timeUp;
        }
        else
        {
            animate.SetTrigger("Slam");
            timer = timeDown;
        }
        timer -= Offset;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log("Going down? " + lowering + " Going up? " + raising + " Raised? " + raised + " Lowered? " + lowered + " Time left: " + (2 > (animate.GetCurrentAnimatorStateInfo(0).length)));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorStateInfo curAnimation = animate.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(curAnimation.IsName("Raised") + " " + raised + " " + raising + " " + lowering);
        //if (!raised && !lowered)
            //Debug.Log(lowering + " " + curAnimation.IsName("Lowered") + " " + !lowered);

        if (raised && timer >= timeUp)
        {
            Animate(true);
            Debug.Log("Going down");
        }
        else if (lowered && timer >= timeDown)
        {
            Animate(false);
            Debug.Log("Going up");
        }
        else if (!raised && !lowered)
        {
            if (lowering && timer >= slamTime)
            {
                lowering = false;
                lowered = true;
                timer = 0;
                animate.SetTrigger("Transistion");
                Debug.Log("Lowered.");
            }
            else if (raising && timer >= retractTime)
            {
                raising = false;
                raised = true;
                timer = 0;
                animate.SetTrigger("Transistion");
                Debug.Log("Raised.");
            }
        }
    }

    void Animate(bool lower)        // Starts lowering platform if "lower" is true, starts raising it if "lower" is false
    {
        hazard.active = lower;
        raised = false;
        lowered = false;
        raising = !lower;
        lowering = lower;
        if (lower)
        {
            animate.SetTrigger("Slam");
        }
        else
        {
            animate.SetTrigger("Retract");
        }
        timer = 0;
    }
}
