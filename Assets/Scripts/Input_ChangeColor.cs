using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_ChangeColor : MonoBehaviour //keyboard controls
{
    public Color color1;
    public Color color2;
    public Text instruction;
    public Image line;
    // public string key;
    public KeyCode code;

    private Color textOrigColor;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().color = color1; 
        line.GetComponent<Image>().color = color1;
        textOrigColor = instruction.GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(code)) {
            gameObject.GetComponent<Image>().color = color2;
            line.color = color2;
            instruction.color = color2;
        }

        if (Input.GetKeyUp(code)) {
            gameObject.GetComponent<Image>().color = color1;
            line.color = color1;
            instruction.color = textOrigColor;
        }
    }
}
