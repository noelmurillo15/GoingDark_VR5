using UnityEngine;
using GoingDark.Core.Enums;

public class LaserProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public LaserType Type;
    [SerializeField]
    private float speed;

    private float baseDmg;

    bool init = false;
    private Transform MyTransform;
    private PlayerStats stats;
    private ObjectPoolManager poolManager;
    #endregion

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            switch (Type)
            {
                case LaserType.Basic:
                    baseDmg = 2.5f;
                    break;
                case LaserType.Charged:
                    baseDmg = 10f;
                    break;
            }
            MyTransform = transform;
            gameObject.SetActive(false);
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
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

    public float GetBaseDmg()
    {
        return baseDmg;
    }

    public void Kill()
    {
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = poolManager.GetLaserExplosion(Type);

        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    #region Collision
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Orb"))
            col.transform.SendMessage("LaserDmg", this);

        if (col.transform.CompareTag("Asteroid"))
        {            
            stats.SendMessage("UpdateCredits", 1);
            col.transform.SendMessage("Kill");
            Kill();
        }     
    }
    #endregion     
}
