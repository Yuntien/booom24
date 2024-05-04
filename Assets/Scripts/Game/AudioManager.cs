using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;

public class AudioManager : Singleton<AudioManager>
{
    private static string LOOP_PATH = "Audio/Loop/";
    private static string INTERACTION_PATH = "Audio/Interaction/";
    private static string VOICE_PATH = "Audio/Voice/";
    private static string MEMORY_PATH = "Audio/Memory/";

    [Header("循环音效")]
    [SerializeField] private AudioSource loopSource;
    [Header("交互音效")]
    [SerializeField] private AudioSource interactionSource;
    [Header("语音")]
    [SerializeField] private AudioSource voiceSource;
    [Header("记忆音效")]
    [SerializeField] private AudioSource memorySource;
    [Header("混合器")]
    [SerializeField] public AudioMixer mixer;

    private void Start()
    {
        var clip = Resources.Load<AudioClip>("Audio/xxx");
        if (clip == null)
        {
            Debug.Log("空的");
        }
    }

    public void FadeInLoopAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioName);
        loopSource.clip = clip;
        loopSource.volume = 0.0f;
        loopSource.Play();
        StartCoroutine(FadeTo(1.0f, 5f));
    }

    private IEnumerator FadeTo(float targetVolume, float duration)
    {
        float startVolume = loopSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            loopSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            yield return null;
        }

        loopSource.volume = targetVolume;
    }
}
