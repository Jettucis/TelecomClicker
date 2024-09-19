using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager m_Instance;

    public Sound[] m_Sounds;
    public AudioSource m_Source;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(m_Sounds, x => x.name == name);

        if (s == null) return;

        m_Source.PlayOneShot(s.clip);
    }    
}
