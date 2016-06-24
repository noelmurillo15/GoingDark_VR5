using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioSource _Music;
    public AudioSource _Alarms;
    public AudioSource _Hit;
    public AudioSource _BattleMusic;
    public AudioSource _Gadget;
    public AudioSource _Messages;
    public AudioSource _Button;
    Dictionary<string, AudioClip> sounds;
    Dictionary<string, AudioClip> music;
    public static AudioManager instance = null;
    private bool Raise;
    private bool Lower;
    private bool Fighting;
    private bool Boss;

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
        Boss = false;
        Fighting = false;
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
        _BattleMusic.clip = music["BattleTheme"];
    }
    
    void Update()
    {
        if (Raise || Lower)
        {
            if (Raise && Lower)
                Raise = Lower = false;
            else if (Raise)
            {
                MasterVolume += 0.01f;
                if (MasterVolume > 1.0f)
                {
                    MasterVolume = 1.0f;
                    Raise = false;
                }
            }
            else if (Lower)
            {
                MasterVolume -= 0.01f;
                if (MasterVolume < 0.0f)
                {
                    Lower = false;
                    MasterVolume = 0.0f;
                }
            }
        }

        if (!_BattleMusic.isPlaying || Boss)
            _Music.volume = MusicVolume * MasterVolume;
        else if(!_Music.isPlaying)
            _BattleMusic.volume = MusicVolume * MasterVolume;

        if((_BattleMusic.isPlaying && _Music.isPlaying) || (!_BattleMusic.isPlaying && !_Music.isPlaying))
        {
            _Music.Stop();
            _BattleMusic.Stop();
            _Music.Play();
        }
    }

    public void PlayBossTheme()
    {
        if (_Music.clip != music["BossTheme"])
        {
            Boss = true;
            _Music.Stop();
            _BattleMusic.Stop();
            _Music.clip = music["BossTheme"];
            _Music.Play();
        }
    }

    public void PlayMissileLaunch()
    {
        _Gadget.clip = sounds["MissileLaunch"];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayThruster()
    {
        _Gadget.volume = SoundVolume * MasterVolume;
        if (!_Gadget.isPlaying)
        {
            if (_Gadget.clip == sounds["Thrusters"])
                _Gadget.Play();
            else
            {
                _Gadget.clip = sounds["Thrusters"];
                _Gadget.Play();
            }
        }
        else if (_Gadget.clip != sounds["Thrusters"])
        {
            _Gadget.clip = sounds["Thrusters"];
            _Gadget.Play();
        }
    }

    public void ThrusterVolume(float amount)
    {
        _Gadget.volume = 0.1f * amount * SoundVolume * MasterVolume;
    }

    public IEnumerator LowerBattleMusic()
    {
        if (_BattleMusic.isPlaying && Fighting && !Boss)
        {
            Fighting = false;
            _Music.volume = 0.0f;
            _Music.Play();
            _Music.volume += 0.18f;
            _BattleMusic.volume -= 0.18f;
            yield return new WaitForSeconds(1f);
            if (_Music.volume < 1.0f)
                _Music.volume += 0.26f;
            if (_BattleMusic.volume > 0.0f)
                _BattleMusic.volume -= 0.26f;
            yield return new WaitForSeconds(1f);
            if (_Music.volume < 1.0f)
                _Music.volume += 0.33f;
            if (_BattleMusic.volume > 0.0f)
                _BattleMusic.volume -= 0.33f;
            yield return new WaitForSeconds(1f);
            if (_Music.volume < 1.0f)
                _Music.volume += 0.38f;
            if (_BattleMusic.volume > 0.0f)
                _BattleMusic.volume -= 0.38f;
            yield return new WaitForSeconds(1f);
            if (_BattleMusic.volume > 0.0f)
                _BattleMusic.volume = 0.1f;
            yield return new WaitForSeconds(1f);
            _BattleMusic.Stop();
        }
    }

    public IEnumerator RaiseBattleMusic()
    {
        if (_Music.isPlaying && !Fighting && !Boss)
        {
            Fighting = true;
            _BattleMusic.volume = 0.0f;
            _BattleMusic.Play();
            _BattleMusic.volume += 0.12f;
            _Music.volume -= 0.12f;
            yield return new WaitForSeconds(1f);

            if (_BattleMusic.volume < 1.0f)
                _BattleMusic.volume += 0.18f;
            if (_Music.volume > 0.0f)
                _Music.volume -= 0.18f;
            yield return new WaitForSeconds(1f);

            if (_BattleMusic.volume < 1.0f)
                _BattleMusic.volume += 0.22f;
            if (_Music.volume > 0.0f)
                _Music.volume -= 0.22f;
            yield return new WaitForSeconds(1f);

            if (_BattleMusic.volume < 1.0f)
                _BattleMusic.volume += 0.38f;
            if (_Music.volume > 0.0f)
                _Music.volume -= 0.38f;

            yield return new WaitForSeconds(1f);
            if (_Music.volume > 0.0f)
                _Music.volume = 0.1f;
            yield return new WaitForSeconds(1f);
            _Music.Stop();
        }
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
        if (!_Hit.isPlaying || _Hit.clip != sounds["HyperDrive"])
        {
            _Hit.clip = sounds["HyperDrive"];
            _Hit.volume = SoundVolume * MasterVolume;
            _Hit.Play();
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

    public void LowerVolume(float vol = 100.0f)
    {
        if (vol != 100.0f && vol >= 0.0f && vol <= 1.0f)
            MasterVolume = vol;
        else
            Lower = true;
    }

    public void RaiseVolume(float vol = 100.0f)
    {
        if (vol != 100.0f && vol >= 0.0f && vol <= 1.0f)
            MasterVolume = vol;
        else
            Raise = true;
    }
}


