using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyLaserProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public EnemyLaserType Type;
    [SerializeField]
    private float aliveTimer;
    [SerializeField]
    private float speed;

    private float baseDmg;

    bool init = false;
    private Transform MyTransform;
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

            switch (Type)
            {
                case EnemyLaserType.Basic:
                    baseDmg = 2.5f;
                    break;
                case EnemyLaserType.Charged:
                    baseDmg = 7.5f;
                    break;
                case EnemyLaserType.MiniCannon:
                    baseDmg = 30f;
                    break;
                case EnemyLaserType.Cannon:
                    baseDmg = 80f;
                    break;
            }
        }
        else
            Invoke("Kill", aliveTimer);        
    }

    void FixedUpdate()
    {
        MyTransform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    #region Accessors
    public float GetBaseDamage()
    {
        return baseDmg;
    }
    #endregion

    #region Recycle Death
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
    #endregion

    #region Collision
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (IsInvoking("Kill"))
                CancelInvoke("Kill");

            col.transform.SendMessage("LaserDmg", this);
        }
        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion     
}