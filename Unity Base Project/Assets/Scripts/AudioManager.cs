using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource _Music;
    public AudioSource _Tactician;
    public AudioSource _WeaponSpecialist;
    public AudioSource _Alarms;
    public AudioSource _Hit;
    public AudioSource _Sonar;
    public AudioSource _Gadget;
    public AudioSource _Messages;
    public AudioSource _Button;


    [Range(0.0f,1.0f)]
    public float MasterVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float SoundVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float MusicVolume = 1.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Music.volume *= MusicVolume * MasterVolume;
    }

    void PlaySound(AudioClip _clip, int source)
    {
        switch(source)
        {
            case 0:
                _Tactician.clip = _clip;
                _Tactician.volume *= SoundVolume * MasterVolume;
                _Tactician.Play();
                break;
            case 1:
                _WeaponSpecialist.clip = _clip;
                _WeaponSpecialist.volume *= SoundVolume * MasterVolume;
                _WeaponSpecialist.Play();
                break;
            case 2:
                _Alarms.clip = _clip;
                _Alarms.volume *= SoundVolume * MasterVolume;
                _Alarms.Play();
                break;
            case 3:
                _Hit.clip = _clip;
                _Hit.volume *= SoundVolume * MasterVolume;
                _Hit.Play();
                break;
            case 4:
                _Sonar.clip = _clip;
                _Sonar.volume *= SoundVolume * MasterVolume;
                _Sonar.Play();
                break;
            case 5:
                _Gadget.clip = _clip;
                _Gadget.volume *= SoundVolume * MasterVolume;
                _Gadget.Play();
                break;
            case 6:
                _Messages.clip = _clip;
                _Messages.volume *= SoundVolume * MasterVolume;
                _Messages.Play();
                break;
            case 7:
                _Button.clip = _clip;
                _Button.volume *= SoundVolume * MasterVolume;
                _Button.Play();
                break;
            default:
                Debug.Log("Invalid audio source chosen!");
                break;
        }
    }

    void PlayMusic(AudioClip audio)
    {
        _Music.Stop();
        _Music.clip = audio;
        _Music.Play();
    }
}
