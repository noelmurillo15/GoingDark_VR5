using UnityEngine;
using System.Collections;

public class Tether : MonoBehaviour
{
    ParticleSystem particles;
    public Vector3 endpos;
    public float speed = 5;
    public float emission = 5;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        particles.emissionRate = emission;
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[particles.particleCount + 1];
        Vector3 dir = endpos - transform.position;
        int l = particles.GetParticles(p);
        for (int i = 0; i < l;  i++ )
        {
            p[i].velocity = dir;
            p[i].velocity *= speed;
        }

        particles.SetParticles(p, l);    
         

    }
}
