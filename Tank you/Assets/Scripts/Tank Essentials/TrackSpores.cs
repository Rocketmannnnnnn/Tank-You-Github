using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpores : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;
    TankVelocity tankVelocity;

    List<Vector3> positions = new List<Vector3>();

    [SerializeField]
    GameObject spores;

    [SerializeField]
    GameObject rotator;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        tankVelocity = GetComponentInParent<TankVelocity>();
    }

    void Update()
    {
        if(tankVelocity.getVelocity().magnitude == 0)
        {
            //spores.SetActive(false);
            ps.enableEmission = false;
        } else
        {
            //spores.SetActive(true);
            ps.enableEmission = true;
        }

        InitializeIfNeeded();

        int amount = ps.GetParticles(particles);

        for (int i = 0; i < amount; i++)
        {
            if (!positions.Contains(particles[i].position))
            {
            if (rotator == null)
            {
                particles[i].rotation3D = transform.root.transform.rotation.eulerAngles;
            }
            else
            {
                particles[i].rotation3D = rotator.transform.rotation.eulerAngles;
            }
                positions.Add(particles[i].position);
            }            
        }
        ps.SetParticles(particles, amount);
    }

    void InitializeIfNeeded()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }
}
