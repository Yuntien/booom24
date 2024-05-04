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

    [Header("ѭ����Ч")]
    [SerializeField] private AudioSource loopSource;
    [Header("������Ч")]
    [SerializeField] private AudioSource interactionSource;
    [Header("����")]
    [SerializeField] private AudioSource voiceSource;
    [Header("������Ч")]
    [SerializeField] private AudioSource memorySource;
    [Header("�����")]
    [SerializeField] public AudioMixer mixer;

    private void Start()
    {
        var clip = Resources.Load<AudioClip>("Audio/xxx");
        if (clip == null)
        {
            Debug.Log("�յ�");
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
