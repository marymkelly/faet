using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[System.Serializable]
public class Response
{
     [SerializeField] private string responseText;
     [SerializeField] private DialogueObject dialogueObject;

     public string ResponseText => responseText;
     public DialogueObject DialogueObject => dialogueObject;
     public string actionResponse;

}
