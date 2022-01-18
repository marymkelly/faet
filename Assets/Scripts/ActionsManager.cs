using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ActionsManager : MonoBehaviour
{   
    public ActionResponse[] Actions;
    public ActionsManager instance;
    
    public void InvokeAction(string name) {
        // Debug.Log($"Action Exists? :  {Array.Exists(Actions, action => action.name == name)}");
        // Debug.Log($"Action Index Found :  {Array.FindIndex(Actions, action => action.name == name)}");
        if(Array.Exists(Actions, action => action.name == name)){
            Debug.Log("Action INVOKED " + name);
            Actions[Array.FindIndex(Actions, action => action.name == name)].ActionEvent.Invoke();
        } else {
            Debug.Log("Action NOT FOUND");
        }
        // foreach(ActionResponse action in Actions) {
        //     if(action.name == name) {
        //         Debug.Log("Action INVOKED " + action.name);
        //         action.ActionEvent.Invoke();
        //     } else {
        //         Debug.Log("Action NOT FOUND");
        //     }
        // }
    }
}
