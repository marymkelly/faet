using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcHandler : MonoBehaviour
{

    private Npc npc;

    public void AddNpc(Npc npc)
    {
        this.npc = npc;
    }

    public string GetNpcName() {
        return npc.Name;
    }

    // public Image GetNpcAvatar()
    // {
    //     return npc.Avatar;
    // }

    // public void SetDialogueSpeaker(string name) {
    //     characterName.gameObject.SetActive(true);
    //     characterName.text = name;
    // }   
}
