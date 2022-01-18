using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance; 
    [HideInInspector]
    public bool AudioIsPlaying = false;
    public bool isMuted = false;

    private Scene scene;
    // [HideInInspector]
    private AudioSource currentTheme;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    // called first
    void OnEnable()
    {
        // Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(mode);
        Sound theme = null;

        if(scene.name == "Start_Screen" || scene.name == "Credits_Screen") {
            theme = CheckSound("HomeTheme");
        }

        else if(scene.name == "Level_One" || scene.name == "Level_One_Second") {
            // theme = CheckSound("HauntedTheme");
            theme = GameObject.Find("Player").GetComponent<Player_Interactions>().defeatedBossOne ? CheckSound("HomeTheme") : CheckSound("HauntedTheme");
        }

        else if (scene.name == "Level_Two" || scene.name == "Level_Two_P1" || scene.name == "Level_Two_Second")
        {
            theme = CheckSound("QuirkyTheme");
        } else if(scene.name == "Boss_One") {
            theme = CheckSound("BossOneTheme");
        }else if(scene.name == "Boss_Two") {
            theme = CheckSound("BossTwoTheme");
        }else if(scene.name == "Boss_Two_Second"){
            theme = CheckSound("BossTwoDefeatTheme");
        }else {
            if(currentTheme != null) {
                currentTheme.Stop();
            }
            currentTheme = null;
            AudioIsPlaying = false;
            return; 
        }

        if(currentTheme == null) {
            if(theme != null) {
                currentTheme = theme.source;
                currentTheme.Play();
                AudioIsPlaying = true;
            }
        } else {
            if(!theme.source.isPlaying) {
                currentTheme.Stop();
                currentTheme = theme.source;
                // Debug.Log("Playing current theme: " + theme.name);
                currentTheme.Play();  
                AudioIsPlaying = true;
            }
        }
    }


    public Sound CheckSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            Debug.Log("Sound: " + name + "was not found!");
        }

        return s;
    }

    public void Play(string name) {
        Sound s = CheckSound(name);

        if(s != null) s.source.Play();
    }

    public void ToggleMute(bool status) {
        if(currentTheme != null) {
            // currentTheme.mute = !currentTheme.mute;
            currentTheme.mute = status;
            isMuted = status;
        }
    }
}
