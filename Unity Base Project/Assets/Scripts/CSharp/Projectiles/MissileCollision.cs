using UnityEngine;

public class MissileCollision : MonoBehaviour {

    #region Properties
    private Missile missile;
    //private Hitmarker hitMarker;
    #endregion

    void Awake()
    {
        missile = GetComponent<Missile>();
        //hitMarker = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();
    }

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            col.transform.SendMessage("MissileHit", missile);
            //hitMarker.HitMarkerShow(Time.time);
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            //hitMarker.HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            missile.Kill();
        }
    }
    #endregion
}
