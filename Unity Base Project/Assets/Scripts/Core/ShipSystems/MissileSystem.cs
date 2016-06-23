using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class MissileSystem : ShipDevice
{

    #region Missile Data
    public int Count { get; private set; }
    public MissileType Type { get; private set; }

    public ObjectPooling emp = new ObjectPooling();
    public ObjectPooling basic = new ObjectPooling();
    public ObjectPooling chromatic = new ObjectPooling();
    public ObjectPooling shieldbreak = new ObjectPooling();

    //  Missile Display
    private Text typeTxt;
    private Text countTxt;
    private Image missileSprite;

    private Transform leapcam;
    private Transform MyTransform;
    private GameObject projectiles;
    private x360Controller controller;
    #endregion

    void Start()
    {
        Count = 15;
        maxCooldown = 1f;
        Type = MissileType.Basic;

        // Show missile count
        projectiles = GameObject.Find("Projectiles");
        countTxt = GameObject.Find("MissileCounter").GetComponent<Text>();
        typeTxt = GameObject.Find("MissileChoice").GetComponent<Text>();
        missileSprite = GameObject.Find("MissileImage").GetComponent<Image>();

        //Missile Ammo Data    
        emp.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EmpMissile"), 4, projectiles);
        basic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/BasicMissile"), 4, projectiles);
        chromatic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ChromaticMissile"), 4, projectiles);
        shieldbreak.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ShieldBreakMissile"), 4, projectiles);

        MyTransform = transform;
        controller = GamePadManager.Instance.GetController(0);
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        CheckCount();
    }

    void Update()
    {
        if (Input.GetAxisRaw("RBumper") > 0f)
            Activate();

        if (controller.GetButtonDown("Y"))
            WeaponSwap();

        if (Activated)
            LaunchMissile();

        MyTransform.rotation = leapcam.rotation;
    }    

    public void AddMissile()
    {
        int rand = Random.Range(1, 3);
        Count += rand;
        countTxt.text = "x: " + Count.ToString();
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
        if (Count > 0)
        {
            Count--;
            DeActivate();
            countTxt.text = "x: " + Count.ToString();

            if (Type == MissileType.Basic)
            {
                GameObject obj = basic.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            else if (Type == MissileType.Emp)
            {
                GameObject obj = emp.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            else if (Type == MissileType.ShieldBreak)
            {
                GameObject obj = shieldbreak.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            else if (Type == MissileType.Chromatic)
            {
                GameObject obj = chromatic.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            CheckCount();
            AudioManager.instance.PlayMissileLaunch();
        }
    }

    public void CheckCount()
    {
        if (Count == 0)
        {
            Type = MissileType.Basic;
            countTxt.text = "";
            typeTxt.text = "Basic";
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
                countTxt.color = Color.magenta;
                typeTxt.color = Color.magenta;
                missileSprite.color = Color.magenta;
                break;
            case MissileType.Chromatic:
                typeTxt.text = "Chromatic";
                countTxt.color = Color.white;
                typeTxt.color = Color.white;
                missileSprite.color = Color.white;
                break;
        }
    }    
}
