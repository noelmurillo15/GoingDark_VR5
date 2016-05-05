﻿using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioSource _Music;
    public AudioSource _Alarms;
    public AudioSource _Hit;
    public AudioSource _Sonar;
    public AudioSource _Gadget;
    public AudioSource _Messages;
    public AudioSource _Button;
    Dictionary<string, AudioClip> sounds;
    public static AudioManager instance = null;

    [Range(0.0f,1.0f)]
    public float MasterVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float SoundVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float MusicVolume = 1.0f;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        sounds = new Dictionary<string, AudioClip>();
        //AudioClip[] clips = new AudioClip[];
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        for (int i = 0; i < clips.Length; i++)
        {
            sounds.Add(clips[i].name, clips[i]);
        }
        //sounds = Resources.LoadAll<AudioClip>("Audio/SFX");
        
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _Music.volume = MusicVolume * MasterVolume;
    }

    public void PlaySound(AudioClip _clip, int source)
    {
        switch(source)
        {
            case 0:
                _Alarms.clip = _clip;
                _Alarms.volume *= SoundVolume * MasterVolume;
                _Alarms.Play();
                break;
            case 1:
                _Hit.clip = _clip;
                _Hit.volume *= SoundVolume * MasterVolume;
                _Hit.Play();
                break;
            case 2:
                _Sonar.clip = _clip;
                _Sonar.volume *= SoundVolume * MasterVolume;
                _Sonar.Play();
                break;
            case 3:
                _Gadget.clip = _clip;
                _Gadget.volume *= SoundVolume * MasterVolume;
                _Gadget.Play();
                break;
            case 4:
                _Messages.clip = _clip;
                _Messages.volume *= SoundVolume * MasterVolume;
                _Messages.Play();
                break;
            case 5:
                _Button.clip = _clip;
                _Button.volume *= SoundVolume * MasterVolume;
                _Button.Play();
                break;
            default:
                Debug.Log("Invalid audio source chosen!");
                break;
        }
    }

    public void PlayMissileLaunch()
    {
        _Gadget.clip = sounds["MissileLaunch"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayEMP()
    {
        _Gadget.clip = sounds["EMP"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayCloak()
    {
        _Gadget.clip = sounds["Cloak"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayMenuGood()
    {
        _Gadget.clip = sounds["MenuGood"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayMenuBad()
    {
        _Gadget.clip = sounds["MenuBad"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayCollect()
    {
        _Messages.clip = sounds["ObjectiveCollect"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayHit()
    {
        _Hit.clip = sounds["Hit"];
        _Hit.volume = SoundVolume * MasterVolume;
        _Hit.Play();
    }

    public void PlayMessagePop()
    {
        _Messages.clip = sounds["Msg"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayMusic(AudioClip audio)
    {
        _Music.Stop();
        _Music.clip = audio;
        _Music.Play();
    }
}

