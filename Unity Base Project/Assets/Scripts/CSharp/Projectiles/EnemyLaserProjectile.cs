using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyLaserProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public EnemyLaserType Type;
    [SerializeField]
    private float speed;

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
            case EnemyLaserType.Basic:
                go = poolManager.GetBaseLaserExplosion();
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
        if (col.transform.CompareTag("Player"))
        {
            col.transform.SendMessage("Hit");
            Kill();
        }

        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion     
}