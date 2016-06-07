using UnityEngine;
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
    Dictionary<string, AudioClip> music;
    public static AudioManager instance = null;
    private bool Raise;
    private bool Lower;

    [Range(0.0f, 1.0f)]
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

        Raise = false;
        Lower = false;

        sounds = new Dictionary<string, AudioClip>();
        music = new Dictionary<string, AudioClip>();
        //AudioClip[] clips = new AudioClip[];
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        AudioClip[] songs = Resources.LoadAll<AudioClip>("Audio/Music");
        for (int i = 0; i < clips.Length; i++)
        {
            sounds.Add(clips[i].name, clips[i]);
        }
        for (int i = 0; i < songs.Length; i++)
        {
            music.Add(songs[i].name, songs[i]);
        }
        //sounds = Resources.LoadAll<AudioClip>("Audio/SFX");
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Raise || Lower)
        {
            if (Raise && Lower)
                Raise = Lower = false;
            else if (Raise)
            {
                MusicVolume += 0.01f;
                if (MusicVolume > 1.0f)
                {
                    MusicVolume = 1.0f;
                    Raise = false;
                }
            }
            else if (Lower)
            {
                MusicVolume -= 0.01f;
                if (MusicVolume < 0.0f)
                {
                    Lower = false;
                    MusicVolume = 0.0f;
                }
            }
        }


        _Music.volume = MusicVolume * MasterVolume;
    }

    public void PlaySound(AudioClip _clip, int source)
    {
        switch (source)
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
    //public void PlayThrusterSound()
    //{
    //    _Gadget.clip = sounds["MainThruster"];
    //    _Gadget.volume = SoundVolume * MasterVolume;
    //    _Gadget.Play();
    //}
    public void PlayShipRepair()
    {
        _Gadget.clip = sounds["RepairSound"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayNebulaAlarm()
    {
        _Alarms.clip = sounds["NebulaAlarm"];
        _Alarms.loop = true;
        _Alarms.volume = SoundVolume * MasterVolume * .75f;
        _Alarms.Play();
    }

    public void PlayHyperDrive()
    {
        if (!_Sonar.isPlaying || _Sonar.clip != sounds["HyperDrive"])
        {
            _Sonar.clip = sounds["HyperDrive"];
            _Sonar.volume = SoundVolume * MasterVolume;
            _Sonar.Play();
        }
    }

    public void StopNebulaAlarm()
    {
        if (_Alarms.clip == sounds["NebulaAlarm"])
            _Alarms.Stop();
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
        _Music.pitch = Time.timeScale;    
    }

    public void PlayShieldOff()
    {
        _Gadget.clip = sounds["ShieldOff"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayShieldOn()
    {
        _Gadget.clip = sounds["ShieldOn"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayMenuGood()
    {
        _Gadget.clip = sounds["MenuGood"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }
    public void PlayAmmoPickUp()
    {
        _Gadget.clip = sounds["Upgrading"];
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

    public void PlayShieldHit()
    {
        _Hit.clip = sounds["ShieldHit"];
        _Hit.volume = SoundVolume * MasterVolume;
        _Hit.Play();
    }

    public void PlayMessagePop()
    {
        _Messages.clip = sounds["Msg"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayLaser()
    {
        _Gadget.clip = sounds["LaserBeam"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayLevel1()
    {
        _Music.Stop();
        _Music.clip = music["Level1theme"];
        _Music.Play();        
    }

    public void PlayLevel2()
    {
        _Music.Stop();
        _Music.clip = music["Level2theme"];
        _Music.Play();
    }

    public void PlayLevel3()
    {
        _Music.Stop();
        _Music.clip = music["Level3theme"];
        _Music.Play();
    }

    public void PlayMainMenu()
    {
        _Music.Stop();
        _Music.clip = music["MainMenuTheme"];
        _Music.Play();
    }

    public void LowerMusicVolume(float vol = 100.0f)
    {
        if (vol != 100.0f && vol >= 0.0f && vol <= 1.0f)
            MusicVolume = vol;
        else
            Lower = true;
    }

    public void RaiseMusicVolume(float vol = 100.0f)
    {
        if (vol != 100.0f && vol >= 0.0f && vol <= 1.0f)
            MusicVolume = vol;
        else
            Raise = true;
    }
}


