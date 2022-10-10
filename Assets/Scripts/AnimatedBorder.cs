using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedBorder : MonoBehaviour
{
    private float transparency = 0;
    private float value;
    private bool ascending = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ascending)
        {
            if (transparency <= 254)
            {
                transparency = transparency + 0.5f;
            } else
            {
                ascending = false;
            }
        } else
        {
            if (transparency >= 1)
            {
                transparency = transparency - 0.4f;
            } else
            {
                ascending = true;
            }
        }
        value = (float)transparency / 255.0f;
        gameObject.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, value);
    }
}
