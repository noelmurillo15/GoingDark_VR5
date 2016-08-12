using UnityEngine;
using GoingDark.Core.Enums;

public class ShieldSystem : MonoBehaviour
{

    private ShieldProperties ShieldData;

    // Use this for initialization
    void Start()
    {
        ShieldData = new ShieldProperties(gameObject, 100f);
    }

    public void MissileHit(MissileProjectile missile)
    {
        if (ShieldData.GetShieldActive())
        {
            switch (missile.Type)
            {
                case MissileType.Basic:
                    missile.Deflect();
                    break;
                case MissileType.Emp:
                    missile.Deflect();
                    break;
                case MissileType.Chromatic:
                    missile.Deflect();
                    break;
                case MissileType.ShieldBreak:
                    ShieldData.Damage(100);
                    missile.Kill();
                    break;
            }
        }
    }

    public void LaserHit(LaserProjectile laser)
    {
        switch (laser.Type)
        {
            case LaserType.Basic:
                ShieldData.Damage(5f);
                break;
            case LaserType.Charged:
                ShieldData.Damage(12.5f);
                break;
        }
    }
}