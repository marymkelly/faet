using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public VideoPlayer video;
    public GameObject loadingScreen;
    public Slider slider;

    void Start() {
        
        // Debug.Log(SceneManager.GetActiveScene().name);

        if (video)
        {
            video.loopPointReached += EndReached;
        }
    }

    void Update()
    {
        // if(Input.GetMouseButtonDown(0)) {
        //     LoadNextLevel();
        // }
        // if(SceneManager.GetActiveScene().name == "Intro_Scene") {
    
        //     video =  GameObject.FindObjectOfType<VideoPlayer>();
        //     video.loopPointReached += EndReached;       
        // }
    }

    // public delegate void EventHandler(VideoPlayer video);
    void EndReached(VideoPlayer video) {
        // Debug.Log("End of video Reached!");
        // LoadNextLevel();
        if(SceneManager.GetActiveScene().name == "Outro_Scene") {
            // StartLoadingLevel(10);
            StartCoroutine(LoadLevel(10));
        } else {
            StartLoadingLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void LoadNextLevel(int index = 0) {
        if(index > 0) {
            StartCoroutine(LoadLevel(index));
        } else {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void StartLoadingLevel(int sceneIndex) {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public IEnumerator LoadLevel(int lvlIndex) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(lvlIndex);

    }

    IEnumerator LoadAsync(int sceneIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while(!operation.isDone) {
            float progress = Mathf.Clamp01((operation.progress) / .9f);
            // Debug.Log(operation.progress);
            // Debug.Log(progress);
            slider.value = progress;

            yield return null;
            // Debug.Log("async start transitiontrigger");
            transition.SetTrigger("Start");
        }
    }

}
