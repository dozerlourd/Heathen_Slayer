using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    static SoundManager instance;
    public static SoundManager Instance => instance ? instance : new GameObject("SoundManager").AddComponent<SoundManager>();

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Variable

    [SerializeField] AudioClip[] buttonClickSounds;

    AudioSource[] audioSource = new AudioSource[3];
    AudioSource VoiceSource => audioSource[0] = audioSource[0] ? audioSource[0] : gameObject.AddComponent<AudioSource>();
    AudioSource EffectSource => audioSource[1] = audioSource[1] ? audioSource[1] : gameObject.AddComponent<AudioSource>();
    AudioSource EnvironmentSource => audioSource[2] = audioSource[2] ? audioSource[2] : gameObject.AddComponent<AudioSource>();

    #endregion

    #region Property

    public AudioClip[] ButtonClickSounds => buttonClickSounds;

    #endregion

    #region Implementation Place

    #region PlayOneShot

    #region Voice
    public void PlayVoiceOneShot(AudioClip _clip, float _volume = 1)
    {
        VoiceSource.PlayOneShot(_clip, _volume);
    }

    public void PlayVoiceOneShot(AudioClip[] _clips, float _volume = 1)
    {
        VoiceSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #region Effect
    public void PlayEffectOneShot(AudioClip _clip, float _volume = 1)
    {
        EffectSource.PlayOneShot(_clip, _volume);
    }

    public void PlayEffectOneShot(AudioClip[] _clips, float _volume = 1)
    {
        EffectSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #region Environment
    public void PlayEnvironmentOneShot(AudioClip _clip, float _volume = 1)
    {
        EnvironmentSource.PlayOneShot(_clip, _volume);
    }

    public void PlayEnvironmentOneShot(AudioClip[] _clips, float _volume = 1)
    {
        EnvironmentSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #endregion

    #region PlayDisConnect

    #region Voice
    public void PlayVoiceDisConnect(AudioClip _clip, float _volume = 1)
    {
        VoiceSource.Stop();
        VoiceSource.PlayOneShot(_clip, _volume);
    }

    public void PlayVoiceDisConnect(AudioClip[] _clips, float _volume = 1)
    {
        VoiceSource.Stop();
        VoiceSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #region Effect
    public void PlayEffectDisConnect(AudioClip _clip, float _volume = 1)
    {
        EffectSource.Stop();
        EffectSource.PlayOneShot(_clip, _volume);
    }

    public void PlayEffectDisConnect(AudioClip[] _clips, float _volume = 1)
    {
        EffectSource.Stop();
        EffectSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #region Environment
    public void PlayEnvironmentDisConnect(AudioClip _clip, float _volume = 1)
    {
        EnvironmentSource.Stop();
        EnvironmentSource.PlayOneShot(_clip, _volume);
    }

    public void PlayEnvironmentDisConnect(AudioClip[] _clips, float _volume = 1)
    {
        EnvironmentSource.Stop();
        EnvironmentSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)], _volume);
    }
    #endregion

    #endregion

    #endregion
}
