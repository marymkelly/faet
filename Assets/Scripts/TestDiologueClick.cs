using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDiologueClick : MonoBehaviour {

    public void Diologue()
    {
        GameObject.Find("Button").GetComponent<TestDiologueTrigger>().Click();
    }
}
