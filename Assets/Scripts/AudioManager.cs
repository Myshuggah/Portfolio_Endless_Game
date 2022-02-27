using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        // attaching the actual properties of the sfx to a variable. 
        foreach(Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

        PlaySound("Main OST");
    }

  public void PlaySound(string name)
    {
        foreach (Sounds s in sounds)
        {
            if (s.name == name)
                s.source.Play();
        }
    }

  public void StopSound(string name)
    {
        foreach (Sounds s in sounds)
        {
            if (s.name == name)
                s.source.Stop();
        }
    }
}
