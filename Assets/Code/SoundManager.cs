using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    private SoundManager() { }

    public void Start()
    {
        if (Instance != null)
        {
            print("Two Soundmanagers! not good!");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public AudioEmitter PlayButtonSound;
    public AudioEmitter ButtonSounds;

    public AudioEmitter HatDropped;
    public AudioEmitter Hatcombined;
    public AudioEmitter HatThrown;

    public AudioEmitter Jump;
    public AudioMixer MainMixer;

    public void MuteSounds()
    {
        Instance.MainMixer.SetFloat("MasterVolume", -80f);
    }
    public void UnmuteSounds()
    {
        Instance.MainMixer.SetFloat("MasterVolume", 0);
    }

    public void PlayButtonsSound()
    {
        Instantiate(PlayButtonSound);
    }

    public void JumpSound()
    {
        Instantiate(Jump);
    }


}
