using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class ActionResponse
{
    [SerializeField] private UnityEvent actionEvent;

    public string name;
    public UnityEvent ActionEvent => actionEvent;
}
