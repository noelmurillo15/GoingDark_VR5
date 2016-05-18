using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //**        Attach to Enemy     **//

    //  Missile Data
    public float angle;
    private bool lockedOn;
    private float missileCooldown;
    public GameObject missilePrefab;

    private EnemyStats stats;


    // Use this for initialization
    void Start() {
        lockedOn = false;
        missileCooldown = 0f;
        stats = GetComponent<EnemyStats>();
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
        Vector3 playerDir = (stats.Target.position - stats.MyTransform.position).normalized;
        angle = Vector3.Dot(playerDir, stats.MyTransform.forward);

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
                Instantiate(missilePrefab, new Vector3(stats.MyTransform.position.x, stats.MyTransform.position.y, stats.MyTransform.position.z), stats.MyTransform.rotation);
            }
        }
    }    
}