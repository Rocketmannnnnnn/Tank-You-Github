using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static bool soloGame = true;
    public static int lives;
    public static float volume;
    public static float musicVolume;
    public static float cameraDistance;

    public static void run(bool running)
    {
        List<string> pauseTags = new List<string>();
        pauseTags.Add("Tank");
        pauseTags.Add("Bullet");
        pauseTags.Add("Mine");

        foreach (string tag in pauseTags)
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                foreach (Behaviour component in gameObject.GetComponentsInChildren<Behaviour>())
                {
                    if (!(component.GetType() == typeof(Renderer)))
                    {
                        component.enabled = running;
                    }
                }
                foreach(ParticleSystem ps in gameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    ParticleSystem.EmissionModule emission = ps.emission;
                    emission.enabled = running;
                }
            }
        }
    }
}
