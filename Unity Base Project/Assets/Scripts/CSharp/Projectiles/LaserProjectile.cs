using UnityEngine;
using GoingDark.Core.Enums;

public class LaserProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public LaserType Type;

    bool init = false;
    public float speed;
    private Transform MyTransform;
    private ChargeLaser MyParent;
    private ObjectPoolManager poolManager;
    #endregion

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            speed = 1000f;
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
        if(IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = null;
        switch (Type)
        {
            case LaserType.Basic:
                go = poolManager.GetChargeLaserExplosion();
                break;
            case LaserType.Charged:
                go = poolManager.GetChargeLaserExplosion();
                break;
            case LaserType.Ball:
                go = poolManager.GetChargeLaserExplosion();
                break;
            case LaserType.Enemy:
                go = poolManager.GetBossLaserExplode();
                break;
        }

        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }
        else
            Debug.Log("Laser Projectile does not have explosion : " + Type.ToString());

        gameObject.SetActive(false);
    }
}
