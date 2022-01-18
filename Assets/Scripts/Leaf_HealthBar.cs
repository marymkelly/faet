using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaf_HealthBar : MonoBehaviour
{
    private Transform[] segments;
    public int maxSegment;
    public int currentSegment;

    void Start()
    {
        segments = new Transform[transform.childCount];
        int i = 0;

        foreach (Transform t in transform)
        {
            segments[i++] = t;
        } 

        maxSegment = transform.childCount;
        currentSegment = (GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Leaf_Update>().currentHealth == 0) ? maxSegment : GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Leaf_Update>().currentHealth;
        SetCurrentSegment(currentSegment);
    }

    void Update() {
        if(GameObject.FindGameObjectWithTag("Player")) {
            int currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Leaf_Update>().currentHealth;
            if(currentSegment != currentHealth) {
                SetCurrentSegment(currentHealth);
            }
        } else {
            if(currentSegment != 0) {
                SetCurrentSegment(0);
            }
        }
    }

    public int GetSegmentTotal() 
    {
        return transform.childCount;
    }

    public void SetCurrentSegment(int level) 
    {
        int segmentIndex = level > 0 ? level - 1 : -1;

        for (int i = 0; i < maxSegment; i++) {
            GameObject leaf = segments[i].gameObject;
            Toggle toggle = leaf.GetComponent(typeof(Toggle)) as Toggle;

            toggle.isOn = (i <= segmentIndex) ? true : false;
        }

        currentSegment = level > 0 ? level : 0;
    }
}
