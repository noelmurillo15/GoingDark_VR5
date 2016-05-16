using UnityEngine;

[RequireComponent(typeof(PatrolAi))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyAttack : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Missile Data
    public float angle;
    private bool lockedOn;
    private float missileCooldown;
    public GameObject missilePrefab;

    //  Enemy Data
    private EnemyStats stats;
    private EnemyBehavior beahvior;
    private Transform MyTransform;


    // Use this for initialization
    void Start() {
        MyTransform = transform;
        lockedOn = false;
        missileCooldown = 0f;
        stats = GetComponent<EnemyStats>();
        beahvior = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update() {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        LockOn();

        if (lockedOn)
            Fire();        
    }

    private void LockOn() {
        Vector3 playerDir = (beahvior.Target.position - MyTransform.position).normalized;
        angle = Vector3.Dot(playerDir, MyTransform.forward);

        if (angle > .985f)
            lockedOn = true;
        else
            lockedOn = false;
    }

    private void Fire() {
        if (stats.MissileCount > 0) {
            if (missileCooldown <= 0.0f) {
                missileCooldown = 5.0f;
                stats.DecreaseMissileCount();
                Instantiate(missilePrefab, new Vector3(MyTransform.position.x, MyTransform.position.y, MyTransform.position.z), MyTransform.rotation);
            }
        }
    }    
}