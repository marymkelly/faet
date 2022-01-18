using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Npc {
    [SerializeField] private string name;
    // [SerializeField] private Image avatar;

    public string Name { get => name; set => name = value; }
    // public Image Avatar =>  avatar;

    public Npc() {

    }
    
    public Npc(string name) {
        this.name = name;
    }
}
