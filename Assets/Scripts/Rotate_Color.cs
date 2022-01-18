using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Color : MonoBehaviour
{
    public Color[] colors;
    public float transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // float newPosition = Mathf.SmoothDamp(currentColor, targetColor, 0.0, transitionTime);
        // GetComponent<SpriteRenderer>().color = new Vector4(transform.position.x, newPosition, transform.position.z);

        GetComponent<SpriteRenderer>().color = Color.Lerp(colors[0], colors[1], Mathf.PingPong((Time.time/transitionTime), 1));
        // GetComponent<SpriteRenderer>().color = Color.Lerp(colors[0], colors[Mathf.FloorToInt(Mathf.Repeat((Time.time / transitionTime ) + 1.5f, colors.Length))], Mathf.PingPong((Time.time / transitionTime), 1));
        // Debug.Log(Mathf.Repeat(Time.time, colors.Length));
    }
}