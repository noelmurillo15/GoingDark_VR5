using UnityEngine;

public class TetherAi : MonoBehaviour
{
    #region Properties
    ParticleSystem particles;
    public float speed = 10;
    public float emission = 20;

    private EnemyStateManager behavior;
    #endregion

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        behavior = GetComponent<EnemyStateManager>();
    }

    void FixedUpdate()
    {

        if (behavior.State == GoingDark.Core.Enums.EnemyStates.Attack)
        {
            particles.emissionRate = emission;
            particles.startSpeed = speed;
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[particles.particleCount + 1];
            Vector3 dir = behavior.Target.position - transform.position;
            float dis = Vector3.Distance(behavior.Target.position, transform.position);
            int l = particles.GetParticles(p);
            for (int i = 0; i < l; i++)
            {
                p[i].velocity = dir;
                p[i].velocity *= (speed * dis * Time.fixedDeltaTime);
                Debug.Log("Tether Firing");
            }

            particles.SetParticles(p, l);
        }

    }

    
}