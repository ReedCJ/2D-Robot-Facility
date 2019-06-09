using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSmasher : MonoBehaviour
{
    private InstantDeath hazard;    // Script handling what happens when the hazard harmfully makes contact with the player
    private Animator animate;       // Animator used for handling the movement of the hazard
    private bool raising;           // Is it currently retract back up?
    private bool lowering;          // Is it currently lowering down to the ground?
    private bool raised;            // Is the compactor currently fully raised?
    private bool lowered;           // Is the compactor currently fully lowered?
    private float timer;            // Keeps track of how long to hold in a raised or lowered position
    private AudioSource audio;
#pragma warning disable 0649
    [SerializeField] private float timeUp;          // Time it spends staying still after retracting back to the top
    [SerializeField] private float timeDown;        // Time it spends staying still after extending to the max range
    [SerializeField] private float slamSpeed;        // How long it takes to finish the slam animation
    [SerializeField] private float retractSpeed;     // How long it takes to finish the retract animation
    [SerializeField] private float Offset;          // How long until it starts moving
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        raised = false;
        lowered = true;
        raising = false;
        lowering = false;
        timer = -Offset;

        hazard = GetComponentInChildren<InstantDeath>();
        hazard.active = false;

        animate = GetComponent<Animator>();
        animate.SetFloat("SlamSpeed", slamSpeed);
        animate.SetFloat("RetractSpeed", retractSpeed);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimatorStateInfo curAnimation = animate.GetCurrentAnimatorStateInfo(0);

        if (raised && timer >= timeUp)
        {
            Animate(true);
        }
        else if (lowered && timer >= timeDown)
        {
            Animate(false);
        }
        else if (!raised && !lowered)       // Freeze compactor in place once it retracts or slams to the max distance.
        {
            if (lowering && curAnimation.normalizedTime >= 1)
            {
                lowering = false;
                lowered = true;
                timer = 0;
                animate.SetTrigger("Transistion");
            }
            else if (raising && curAnimation.normalizedTime >= 1)
            {
                raising = false;
                raised = true;
                timer = 0;
                animate.SetTrigger("Transistion");
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
        timer = 0;

        if (lower)
        {
            audio = FindObjectOfType<AudioSource>();
            audio.Play(0);
            animate.SetTrigger("Slam");
        }
        else animate.SetTrigger("Retract");
    }
}
