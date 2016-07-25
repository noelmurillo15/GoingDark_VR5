using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class MissileSystem : ShipSystem
{
    #region Missile Data
    private int[] Count = new int[4];
    public MissileType Type { get; private set; }

    // Missile Pools
    private ObjectPoolManager poolmanager;

    //  Missile Display
    private Text typeTxt;
    private Text countTxt;
    private Image missileSprite;

    // Misc
    private Hitmarker lockon;
    private Transform MyTransform;
    private x360Controller controller;
    private Transform leap;
    #endregion

    void Start()
    {
        Count[0] = PlayerPrefs.GetInt("BasicMissileCount");
        Count[1] = PlayerPrefs.GetInt("EMPMissileCount");
        Count[2] = PlayerPrefs.GetInt("ShieldbreakMissileCount");
        Count[3] = PlayerPrefs.GetInt("ChromaticMissileCount");

        maxCooldown = 1f;
        Type = MissileType.ShieldBreak;

        // Show missile count
        typeTxt = GameObject.Find("MissileChoice").GetComponent<Text>();
        countTxt = GameObject.Find("MissileCounter").GetComponent<Text>();
        missileSprite = GameObject.Find("MissileImage").GetComponent<Image>();

        leap = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //Missile Ammo Data    
        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();

        MyTransform = transform;
        controller = GamePadManager.Instance.GetController(0);
        lockon = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();

        CheckCount();
    }

    void Update()
    {
        if (controller.GetButtonDown("Y"))
            WeaponSwap();

        if (Activated)
            LaunchMissile();

        MyTransform.rotation = leap.rotation;
    }    

    public void AddMissile()
    {
        int typemiss = Random.Range(0, 3);

        switch ((MissileType)typemiss)
        {
            case MissileType.Basic:
                Count[typemiss] += 5;
                break;
            case MissileType.Emp:
                Count[typemiss] += 3;
                break;
            case MissileType.ShieldBreak:
                Count[typemiss] += 3;
                break;
            case MissileType.Chromatic:
                Count[typemiss] += 2;
                break;
        }        
        CheckCount();
    }

    public void WeaponSwap()
    {
        int curr = (int)(Type + 1);
        if (System.Enum.GetValues(typeof(MissileType)).Length == curr)
            curr = 0;

        Type = (MissileType)curr;
        CheckCount();
    }

    public void LaunchMissile()
    {
        if (Count[(int)Type] > 0)
        {            
            DeActivate();

            GameObject obj = null;
            switch (Type)
            {
                case MissileType.Basic:
                    obj = poolmanager.GetBaseMissile();
                    break;
                case MissileType.Emp:
                    obj = poolmanager.GetEmpMissile();
                    break;
                case MissileType.ShieldBreak:
                    obj = poolmanager.GetSBMissile();
                    break;
                case MissileType.Chromatic:
                    obj = poolmanager.GetChromeMissile();
                    break;
            }

            if (obj != null)
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                Count[(int)Type]--;

                if (lockon.GetLockedOn())
                    obj.SendMessage("LockedOn", lockon.GetRaycastHit());

                AudioManager.instance.PlayMissileLaunch();
                CheckCount();
            }
        }
    }

    public void CheckCount()
    {
        if (Count[(int)Type] == 0)
        {
            countTxt.text = "";
            typeTxt.text = Type.ToString();
            typeTxt.color = Color.grey;
            countTxt.color = Color.grey;
            missileSprite.color = Color.grey;
            return;
        }

        switch (Type)
        {
            case MissileType.Basic:
                typeTxt.text = "Basic";
                countTxt.color = Color.yellow;
                typeTxt.color = Color.yellow;
                missileSprite.color = Color.yellow;
                break;
            case MissileType.Emp:
                typeTxt.text = "Emp";
                countTxt.color = Color.cyan;
                typeTxt.color = Color.cyan;
                missileSprite.color = Color.cyan;
                break;
            case MissileType.ShieldBreak:
                typeTxt.text = "ShieldBreak";
                countTxt.color = Color.red;
                typeTxt.color = Color.red;
                missileSprite.color = Color.red;
                break;
            case MissileType.Chromatic:
                typeTxt.text = "Chromatic";
                countTxt.color = Color.green;
                typeTxt.color = Color.green;
                missileSprite.color = Color.green;
                break;
        }
        countTxt.text = "x: " + Count[(int)Type].ToString();
    }    
}
