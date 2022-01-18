using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtons : MonoBehaviour
{
    private GameObject gameData;
    // Start is called before the first frame update
    void Start()
    {   
        gameData = GameObject.Find("GameData").gameObject;
    }

    // Update is called once per frame
    public void StartNewGame()
    {
        gameData.GetComponent<GameData>().NewGame();
    }

    public void LoadGame()
    {
        gameData.GetComponent<GameData>().CallForLoad();
    }


}
