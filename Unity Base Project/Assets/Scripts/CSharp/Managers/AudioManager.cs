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
    public AudioClip[] songs;

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

    void Awake()
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
        if (_BattleMusic.clip != music["BattleTheme"])
            _BattleMusic.clip = music["BattleTheme"];


        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LevelSelect")
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
        else if (!_Music.isPlaying)
            _BattleMusic.volume = MusicVolume * MasterVolume;
    }


    void OnLevelWasLoaded()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                _Music.clip = music["G_D_MainMenu_Music"];
                break;
            case "Level1":
                _Music.clip = music["G_D_Level1_Music"];
                break;
            case "Level2":
                _Music.clip = music["G_D_Level2_Music"];
                break;
            case "Level3":
                _Music.clip = music["G_D_Level3_Music"];
                break;
            case "Level4":
                _Music.clip = music["G_D_Level4_Music"];
                break;
            case "LevelSelect":
                _Music.clip = music["G_D_LevelSelect_Music"];
                break;
            default:
                return;
        }

        PlayMusic();
    }

    public void PlayBossTheme()
    {
        if (_Music.clip != music["G_D_Boss_Music"])
        {
            Boss = true;
            _Music.Stop();
            _BattleMusic.Stop();
            _Music.clip = music["G_D_Boss_Music"];
            _Music.Play();
        }
    }

    public void PlayMissileLaunch()
    {
        int r = Random.Range(1, 2);
        string aud = "G_D_Missile_Launching_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayThruster()
    {
        _Thruster.volume = SoundVolume * MasterVolume;
        int r = Random.Range(1, 2);
        string aud = "G_D_Ship_Thrusters_0" + r;
        if (!_Thruster.isPlaying)
        {

            if (_Thruster.clip == sounds[aud])
                _Thruster.Play();
            else
            {
                _Thruster.clip = sounds[aud];
                _Thruster.Play();
            }
        }
        else if (_Thruster.clip != sounds["G_D_Ship_Thrusters_01"] && _Thruster.clip != sounds["G_D_Ship_Thrusters_02"])
        {
            _Thruster.clip = sounds[aud];
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
            _Music.loop = true;
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
            _BattleMusic.loop = true;
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
        _Alarms.clip = sounds["HyperDrive"];
        _Alarms.volume = SoundVolume * MasterVolume;
        _Alarms.Play();
    }

    public void PlayEMP()
    {
        int r = Random.Range(1, 3);
        string aud = "G_D_EMP_Blast_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayCloak()
    {
        int r = Random.Range(2, 3);
        string aud = "G_D_Cloak_Activate_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
        _Music.pitch = Time.timeScale;
    }

    public void PlayShieldOff()
    {
        int r = Random.Range(1, 3);
        string aud = "G_D_Player_Shield_Deactivate_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayShieldOn()
    {
        int r = Random.Range(1, 3);
        string aud = "G_D_Player_Shield_Activate_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
    }

    public void PlayMenuGood()
    {
        int r = Random.Range(1, 2);
        string aud = "G_D_Menu_Confirm_0" + r;
        _Messages.clip = sounds[aud];
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
        int r = Random.Range(1, 2);
        string aud = "G_D_Menu_Deny_0" + r;
        _Messages.clip = sounds[aud];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayCollect()
    {
        int r = Random.Range(1, 2);
        string aud = "G_D_Obj_Collected_0" + r;
        _Messages.clip = sounds[aud];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayHit()
    {
        int r = Random.Range(1, 3);
        string aud = "G_D_Player_Ship_Hit_0" + r;
        _Hit.clip = sounds[aud];
        _Hit.volume = SoundVolume * MasterVolume;
        _Hit.Play();
    }

    public void PlayShieldHit()
    {
        int r = Random.Range(1, 3);
        string aud = "G_D_Player_Shield_Hit_0" + r;
        _Hit.clip = sounds[aud];
        _Hit.volume = SoundVolume * MasterVolume;
        _Hit.Play();
    }

    public void PlayMessagePop()
    {
        int r = Random.Range(1, 2);
        string aud = "G_D_Menu_Message_0" + r;
        _Messages.clip = sounds[aud];
        _Messages.volume = SoundVolume * MasterVolume;
        _Messages.Play();
    }

    public void PlayMusic()
    {
        if (_BattleMusic.isPlaying)
            _BattleMusic.Stop();
        _Music.loop = true;
        _Music.Play();
    }

    public void PlayLaser()
    {
        int r = Random.Range(1, 5);
        string aud = "G_D_Laser_REDBeam_0" + r;
        _Gadget.clip = sounds[aud];
        _Gadget.volume = SoundVolume * MasterVolume;
        _Gadget.Play();
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