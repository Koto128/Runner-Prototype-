using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSlide : MonoBehaviour
{
    private Vector3 startingBgPos = new Vector3(110, 9.5f, 4);//position i want to get the BG to
    private Vector3 trigBgPos = new Vector3(-110, 9.5f, 4);//at which position the BG snaps to "startingBgPos"

    private void Update()
    {
        if (transform.position.x <= trigBgPos.x)
        {
            transform.position = startingBgPos;
        }   
    }
}
