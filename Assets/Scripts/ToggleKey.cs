using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleKey : MonoBehaviour
{
    public GameObject shape;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {

        shape = GameObject.Find("SmokeDot2").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.C)) {
            if(shape.activeSelf) {  
                shape.SetActive(false);
            }
            active = !active;

            Debug.Log("Triggered C");
        };

        if(Input.GetKeyUp(KeyCode.D)) {
            Debug.Log("Triggered D");
        };

        if(Input.GetKeyDown(KeyCode.K)) {
            shape.SetActive(true);
            active = !active;
            Debug.Log("Triggered K Down");
        };

        Debug.Log(shape.activeSelf);
        
    }
}
