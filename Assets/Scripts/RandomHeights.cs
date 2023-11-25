using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomHeights : MonoBehaviour
{
    private void Start()
    {
        Transform buildL = transform.Find("Building Left");
        Transform buildR = transform.Find("Building Right");
        
        float random = Random.Range(4, 8);
        //float buildLY = buildL.position.y;
        float buildLscale = buildL.localScale.y;
        float newYscale = buildLscale * random;
        //float newYheight = buildLY * random;
        buildL.localScale = new Vector3(buildL.localScale.x, newYscale, buildL.localScale.z);
        //buildL.position = new Vector3(buildL.position.x, newYheight, buildL.position.z);

        
        random = Random.Range(4, 8);
        //float buildRY = buildR.position.y;
        float buildRscale = buildR.localScale.y;
        float newYRscale = buildRscale * random;
        //float newYRheight = buildRY * random;
        buildR.localScale = new Vector3(buildR.localScale.x, newYRscale, buildR.localScale.z);
        //buildR.position = new Vector3(buildR.position.x, newYRheight, buildR.position.z);
    }
}
