using UnityEngine;
using GoingDark.Core.Enums;

public class LaserProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public LaserType Type;
    [SerializeField]
    private float speed;

    bool init = false;
    private Transform MyTransform;
    private ChargeLaser MyParent;
    private ObjectPoolManager poolManager;
    #endregion

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            MyTransform = transform;
            gameObject.SetActive(false);
            poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        }
        else
        {
            Invoke("Kill", 3f);
        }
    }

    void FixedUpdate()
    {
        MyTransform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    public void Kill()
    {
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = null;
        switch (Type)
        {
            case LaserType.Basic:
                go = poolManager.GetBaseLaserExplosion();
                break;
            case LaserType.Charged:
                go = poolManager.GetChargeLaserExplosion();
                break;
        }
        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }
        else
            Debug.Log("Laser Projectile does not have explosion : " + transform.name);

        gameObject.SetActive(false);
    }

    #region Collision
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
            col.transform.SendMessage("LaserDmg", this);

        if (col.transform.CompareTag("Orb"))
            col.transform.SendMessage("LaserDmg", this);

        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }     
    }
    #endregion     
}
