﻿using UnityEngine;
using GoingDark.Core.Enums;

public class Missile : MonoBehaviour {

    #region Properties
    public MissileType Type;
    public MovementProperties moveData;

    bool init = false;
    private bool tracking;
    private bool deflected;
    private GameObject Explosion;
    private GameObject Explosions;
    private Transform MyTransform;

    //  Raycast
    private int range;
    private RaycastHit hit;

    //  Target
    private Transform target;
    private Vector3 direction;
    #endregion


    public void OnEnable()
    {
        if (!init)
        {
            init = true;
            range = 1600;
            target = null;
            tracking = false;
            deflected = false;

            moveData.Boost = 1f;
            moveData.MaxSpeed = 500f;
            moveData.RotateSpeed = 25f;
            moveData.Acceleration = 200f;
            moveData.Speed = 100f;

            MyTransform = transform;
            direction = MyTransform.forward;

            Explosions = GameObject.Find("Explosions");
            FindExplosion();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Player Missile Used");
            target = null;
            tracking = false;
            deflected = false;
            moveData.Speed = 100f;
            direction = MyTransform.forward;
            Invoke("Kill", 3f);
        }    
    }

    void FixedUpdate()
    {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        RaycastCheck();

        MyTransform.position += direction * moveData.Speed * Time.deltaTime;
    }

    private void LookAt()
    {
        if (target != null)
        {
            direction = MyTransform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * moveData.RotateSpeed);
        }
    }

    public void Deflect()
    {
        Debug.Log("Missile was deflected by enemy shield");
        CancelInvoke();
        Invoke("Kill", 2f);
        deflected = true;
        tracking = false;
        target = null;
        direction = -direction;
    }

    private void RaycastCheck()
    {
        if (!tracking && !deflected)
        {
            if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
            {
                if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                {
                    target = hit.collider.transform;
                    tracking = true;
                }
            }
        }
    }

    void FindExplosion()
    {
        Explosion = null;
        switch (Type)
        {
            case MissileType.Basic:
                Explosion = Instantiate(Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion"), transform.position, Quaternion.identity) as GameObject; break;                
            case MissileType.Emp:
                Explosion = Instantiate(Resources.Load<GameObject>("Projectiles/Explosions/EmpExplosion"), transform.position, Quaternion.identity) as GameObject; break;
            case MissileType.ShieldBreak:
                Explosion = Instantiate(Resources.Load<GameObject>("Projectiles/Explosions/ShieldBreakExplosion"), transform.position, Quaternion.identity) as GameObject; break;
            case MissileType.Chromatic:
                Explosion = Instantiate(Resources.Load<GameObject>("Projectiles/Explosions/ChromaticExplosion"), transform.position, Quaternion.identity) as GameObject; break;                
        }
        if(Explosion != null)
        {
            Explosion.transform.parent = Explosions.transform;
        }
        else
        {
            Debug.LogError("Missile Explosion == null");
        }
    }

    public void Kill()
    {
        CancelInvoke();
        deflected = false;
        Explosion.transform.position = MyTransform.position;
        Explosion.SetActive(true);
        gameObject.SetActive(false);
    }
    private void SelfDestruct()
    {
        Debug.Log("Self Destruct Called");
        direction = MyTransform.forward;

        moveData.Speed = 50f;

        target = null;
        tracking = false;

        Invoke("Kill", 4f);
    }   
}