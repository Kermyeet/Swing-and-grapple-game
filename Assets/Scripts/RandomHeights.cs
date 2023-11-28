using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomHeights : MonoBehaviour
{
    
    // This whole script is just to give some variety to the buildings heights,
    // so if the player got above clouds they could see the variety or something, idk it felt fitting
    private void Start()
    {
        Transform buildL = transform.Find("Building Left");
        Transform buildR = transform.Find("Building Right");
        
        float random = Random.Range(4, 8);
        float buildLscale = buildL.localScale.y;
        float newYscale = buildLscale * random;
        buildL.localScale = new Vector3(buildL.localScale.x, newYscale, buildL.localScale.z);

        
        random = Random.Range(4, 8);
        float buildRscale = buildR.localScale.y;
        float newYRscale = buildRscale * random;
        buildR.localScale = new Vector3(buildR.localScale.x, newYRscale, buildR.localScale.z);
    }
}
