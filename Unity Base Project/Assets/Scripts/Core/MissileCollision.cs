using UnityEngine;
using GoingDark.Core.Enums;

public class MissileCollision : MonoBehaviour {

    #region Properties
    private Missile missile;
    private Hitmarker hitMarker;
    #endregion

    void Start()
    {
        missile = GetComponent<Missile>();
        hitMarker = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();
    }

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            switch (missile.Type)
            {
                case MissileType.EMP:
                    col.transform.SendMessage("EMPHit");
                    break;
                case MissileType.BASIC:
                    col.transform.SendMessage("Hit", missile);
                    break;
                case MissileType.CHROMATIC:
                    col.transform.SendMessage("Hit", missile);
                    break;
                case MissileType.SHIELDBREAKER:
                    col.transform.SendMessage("ShieldHit");
                    break;
            }
            hitMarker.HitMarkerShow(Time.time);
            missile.Kill();
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            hitMarker.HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            missile.Kill();
        }
        else if (col.transform.CompareTag("Turret"))
        {
            hitMarker.HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            missile.Kill();
        }
    }
    #endregion
}
