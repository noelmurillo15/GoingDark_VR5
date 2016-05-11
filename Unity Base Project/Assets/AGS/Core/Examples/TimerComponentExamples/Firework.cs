using UnityEngine;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;


public class Firework : MonoBehaviour
{

    private Rigidbody _rigidbody;

    private ParticleSystem[] _particleSystems;

    public Object ExplodePrefab;

    public float LaunchInSeconds = 2f;
    public float ExplodeInSeconds = 2f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var system in _particleSystems)
        {
            system.enableEmission = false;
        }

        var rocketUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);

        var explodeTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "explode timer");
        explodeTimer.TimerMethod = () =>
        {
            rocketUpdater.Stop();
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var system in _particleSystems)
            {
                Destroy(system);
            }
            if (ExplodePrefab == null) return;

            var explosionParticles = Instantiate(ExplodePrefab, transform.position, Quaternion.identity) as GameObject;

            if (explosionParticles == null) return;

            explosionParticles.SetActive(true);
        };

        var launchTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "launch timer");
        launchTimer.TimerMethod = () =>
        {
            rocketUpdater.UpdateMethod = () =>
            {
                _rigidbody.AddForce(Vector3.up * 50f);
            };
            foreach (var system in _particleSystems)
            {
                system.enableEmission = true;
            }
			explodeTimer.Invoke(ExplodeInSeconds);
        };
        launchTimer.Invoke(LaunchInSeconds);
    }

}
