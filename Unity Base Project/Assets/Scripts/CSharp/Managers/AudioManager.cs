using UnityEngine;
using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource _Music;
    public AudioSource _Alarms;
    public AudioSource _Hit;
    public AudioSource _BattleMusic;
    public AudioSource _Gadget;
    public AudioSource _Messages;
    public AudioSource _Button;
    public AudioSource _Thruster;

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
        if(_BattleMusic.clip != music["BattleTheme"])
        _BattleMusic.clip = music["BattleTheme"];


        if(SceneManager.GetActiveScene().name != "MainMenu")
            PlayThruster();


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
        _Thruster.volume = SoundVolume * MasterVolume;
        if (!_Thruster.isPlaying)
        {
            if (_Thruster.clip == sounds["Thrusters"])
                _Thruster.Play();
            else
            {
                _Thruster.clip = sounds["Thrusters"];
                _Thruster.Play();
            }
        }
        else if (_Thruster.clip != sounds["Thrusters"])
        {
            _Thruster.clip = sounds["Thrusters"];
            _Thruster.Play();
        }
    }

    public void ThrusterVolume(float amount)
    {
        _Thruster.volume = 0.15f * amount * SoundVolume * MasterVolume;
    }

    public void LowerBattleMusic()
    {
        Timing.RunCoroutine(LBM());
    }
    private IEnumerator<float> LBM()
    {
        if (Fighting && !Boss)
        {
            Fighting = false;
            _Music.volume = 0.0f;
            _Music.Play();
            _Music.volume += 0.18f;
            _BattleMusic.volume -= 0.18f;
            yield return Timing.WaitForSeconds(.5f);
            if (!Fighting)
            {
                if (_Music.volume < 1.0f)
                    _Music.volume += 0.26f;
                if (_BattleMusic.volume > 0.0f)
                    _BattleMusic.volume -= 0.26f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (!Fighting)
            {
                if (_Music.volume < 1.0f)
                    _Music.volume += 0.33f;
                if (_BattleMusic.volume > 0.0f)
                    _BattleMusic.volume -= 0.33f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (!Fighting)
            {
                if (_Music.volume < 1.0f)
                    _Music.volume += 0.38f;
                if (_BattleMusic.volume > 0.0f)
                    _BattleMusic.volume -= 0.38f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (!Fighting)
            {
                if (_BattleMusic.volume > 0.0f)
                    _BattleMusic.volume = 0.1f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (!Fighting)
                _BattleMusic.Stop();
        }
    }
    public void RaiseBattleMusic()
    {
        Timing.RunCoroutine(RBM());
    }
    public IEnumerator<float> RBM()
    {
        if (!Fighting && !Boss)
        {
            Fighting = true;
            _BattleMusic.volume = 0.0f;
            _BattleMusic.Play();
            _BattleMusic.volume += 0.12f;
            _Music.volume -= 0.12f;
            yield return Timing.WaitForSeconds(.5f);
            if (Fighting)
            {
                if (_BattleMusic.volume < 1.0f)
                    _BattleMusic.volume += 0.18f;
                if (_Music.volume > 0.0f)
                    _Music.volume -= 0.18f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (Fighting)
            {
                if (_BattleMusic.volume < 1.0f)
                    _BattleMusic.volume += 0.22f;
                if (_Music.volume > 0.0f)
                    _Music.volume -= 0.22f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (Fighting)
            {
                if (_BattleMusic.volume < 1.0f)
                    _BattleMusic.volume += 0.38f;
                if (_Music.volume > 0.0f)
                    _Music.volume -= 0.38f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (Fighting)
            {
                if (_Music.volume > 0.0f)
                    _Music.volume = 0.1f;
            }
            yield return Timing.WaitForSeconds(.5f);
            if (Fighting)
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

    public void PlayTether()
    {
        _Alarms.clip = sounds["Tether"];
        _Alarms.loop = true;
        _Alarms.volume = SoundVolume * MasterVolume * .25f;
        _Alarms.Play();
    }

    public void StopTether()
    {
        if (_Alarms.clip == sounds["Tether"] && _Alarms.isPlaying)
            _Alarms.Stop();
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
        if (_Alarms.clip == sounds["NebulaAlarm"] && _Alarms.isPlaying)
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
        _Messages.clip = sounds["MenuGood"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }
    public void PlayAmmoPickUp()
    {
        _Messages.clip = sounds["Upgrading"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayMenuBad()
    {
        _Messages.clip = sounds["MenuBad"];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
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


