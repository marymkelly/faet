using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate_Colors : MonoBehaviour
{
    public Color[] colors;
    private Vector4[] rgba;
    private Vector4 currentColorVector;
    public float transitionRate = .008f;
    public float i = 0;

    // Start is called before the first frame update
    void Start()
    {
        int j = 0;
        rgba = new Vector4[colors.Length];
         
        foreach (Color c in colors) {
            rgba[j++] = new Vector4(c[0], c[1], c[2], c[3]);
        }

        currentColorVector = rgba[(int)i];
    }

    // Update is called once per frame
    void Update()
    {
        
        int k = (int)(i + 1 == rgba.Length ? 0 : (i + 1));

        currentColorVector = Vector4.MoveTowards(currentColorVector, rgba[k], Time.time / transitionRate);
        GetComponent<SpriteRenderer>().color = currentColorVector;

        if(currentColorVector.Equals(rgba[k])) {
            Debug.Log("Color Matched!");
            i++;

            if(i == (colors.Length)) {
                i = 0;
            }
        }
    }
}