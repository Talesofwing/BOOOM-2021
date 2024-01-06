using System;
using System.Collections;
using System.Collections.Generic;
using Kuma.Utils.Singleton;

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup bgmMixer;
    [SerializeField] private AudioMixerGroup seMixer;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private SoundClip[] SEList;
    [SerializeField] private SoundClip[] BGMList;

    private List<AudioSource> audioSourcesList = new List<AudioSource>();
    private AudioSource bgmSource;
    private Dictionary<string, AudioClip> seDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    public float BGMVol { get; private set; } = 1;
    public float SEVol { get; private set; } = 1;

    protected override void Awake()
    {
        base.Awake();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmMixer;
        bgmSource.loop = true;
        
        foreach (var se in SEList)
        {
            seDictionary.Add(se.name, se.audioClip);
        }

        foreach (var bgm in BGMList)
        {
            bgmDictionary.Add(bgm.name, bgm.audioClip);
        }
    }
    

    private AudioSource GetAudioSourceReady()
    {
        foreach (var audioSource in audioSourcesList)
        {
            if(!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = seMixer;
        audioSourcesList.Add(newSource);
        return newSource;
    }

    public void PlaySE(AudioClip clip)
    {
        var audioSource = GetAudioSourceReady();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySE(string name)
    {
        if (seDictionary.TryGetValue(name, out var audioClip))
        {
            PlaySE(audioClip);
        }
    }

    public void PlayClick()
    {
        PlaySE(clickSound);
    }
    
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlayBGM(string name)
    {
        if (bgmDictionary.TryGetValue(name, out var audioClip))
        {
            PlayBGM(audioClip);
        }
    }

    public void SetBGMVolume (float value)
    {
        BGMVol = value;
        if(value == 0)
        {
            mixer.SetFloat("BGMVol", -80);
        }
        else
        {
            const float scale = 0.5f;  //BGM音量
            mixer.SetFloat("BGMVol", Mathf.Log10(value * scale) * 20);
        }
    }

    public void SetSEVolume (float value)
    {
        SEVol = value;
        if(value == 0)
        {
            mixer.SetFloat("SEVol", -80);
        }
        else
        {
            mixer.SetFloat("SEVol", Mathf.Log10(SEVol) * 20);
        }
    }

    private float _moveSoundRest;
    public void PlayMove()
    {
        if(_moveSoundRest > 0)
        {
            return;
        }

        PlaySE("foot0" + UnityEngine.Random.Range(0, 2));

        _moveSoundRest = 0.4f;
    }

    public void PlayInteraction () {
        PlaySE ("magic");
    }

    void Update()
    {
        if(_moveSoundRest > 0)
        {
            _moveSoundRest -= Time.deltaTime;
        }   
    }
    
    [Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip audioClip;
    }
}
