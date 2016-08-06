using UnityEngine;

public class MissileCollision : MonoBehaviour {

    #region Properties
    private Missile missile;
    #endregion

    void Awake()
    {
        missile = GetComponent<Missile>();
    }

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Shield"))
        {
            col.transform.SendMessage("MissileHit", missile);
        }
        else if (col.transform.CompareTag("Orb"))
        {
            col.transform.SendMessage("MissileDmg", missile);
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            missile.Kill();
        }
    }
    #endregion
}
