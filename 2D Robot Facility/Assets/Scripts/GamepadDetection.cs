using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadDetection : MonoBehaviour
{
    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int i = 0; i < names.Length; i++)
        {
           // print(names[i].Length);
            if (names[i].Length == 19)
            {
                //print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[i].Length == 33)
            {
                //print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }


        if (Xbox_One_Controller == 1)
        {
            //do something
        }
        else if (PS4_Controller == 1)
        {
            //do something
        }
        else
        {
            // there is no controllers
        }
    }
}
