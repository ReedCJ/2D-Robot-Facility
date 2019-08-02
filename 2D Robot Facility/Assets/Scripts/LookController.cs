using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    [System.NonSerialized] public PlayerController player;            // Used to grab variables from the player.
    [SerializeField] private GameObject cam;    // Game object that the camera follows.
    private Quaternion rotation;
    [System.NonSerialized] bool following;      // Is the camera following the player?
    [System.NonSerialized] bool lookUp;         // Is the player looking up?
    [System.NonSerialized] bool lookDown;         // Is the player looking down?
    [Tooltip("How far the camera will move up when the character looks up.")]
    public float camUp;
    [Tooltip("How far the camera will move down when the character looks down.")]
    public float camDown;

    // Start is called before the first frame update
    void Start()
    {
        lookDown = false;
        lookUp = false;
        following = true;
        player = PlayerController.instance;
        rotation = cam.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.grounded && player.hMove == 0 && player.vMove != 0 && following)
        {
            cam.SetActive(true);
            if (player.vMove > 0)
            {
                cam.transform.localPosition = new Vector3(0, camUp, 0);
            }
            else
            {
                cam.transform.localPosition = new Vector3(0, -camDown, 0);
            }
        }
        else if (cam.activeSelf)
        {
            cam.SetActive(false);
            cam.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void LateUpdate()
    {
        cam.transform.rotation = rotation;
    }
}
