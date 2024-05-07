using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("—≠ª∑“Ù–ß")]
    [SerializeField] private AudioSource loopSource;
    [Header("Ωªª•“Ù–ß")]
    [SerializeField] private AudioSource interactionSource;
    [Header("”Ô“Ù")]
    [SerializeField] private AudioSource voiceSource;
    [Header("º«“‰“Ù–ß")]
    [SerializeField] private AudioSource memorySource;
    [Header("±≥æ∞“Ù–ß1")]
    [SerializeField] private AudioSource amb1Source;
    [Header("±≥æ∞“Ù–ß2")]
    [SerializeField] private AudioSource amb2Source;
    [Header("ªÏ∫œ∆˜")]
    [SerializeField] public AudioMixer mixer;

    public int interactionSize;

    public float fadeTime;

    #region ÀÊª˙≤•∑≈
    public void RandomPlayVoice(string actorName, int voiceSize)
    {
        AudioClip clip = RandomLoadAudioClip(voiceSize, $"Audio/Voice/{actorName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void RandomPlayInteraction(string interactionName)
    {
        AudioClip clip = RandomLoadAudioClip(interactionSize, $"Audio/Interaction/{interactionName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
    }
    #endregion

    #region ≤•∑≈≥™∏Ë
    public void PlayVoice(string actorName, string voiceName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Voice/{actorName}/{voiceName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }
    #endregion

    #region —≠ª∑≤•∑≈
    public void PlayLoopAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Loop/{audioName}");
        loopSource.clip = clip;
        loopSource.Play();
    }

    public void PauseLoopAudio()
    {
        loopSource.Pause();
    }

    public void ContinueLoopAudio()
    {
        loopSource.UnPause();
    }
    #endregion

    #region ±≥æ∞“Ù—≠ª∑≤•∑≈
    public void PlayAmb1Audio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Amb/{audioName}");
        amb1Source.clip = clip;
        amb1Source.volume = 0f;
        amb1Source.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb1Source));
    }

    public void PauseAmb1Audio()
    {
        amb1Source.Pause();
    }

    public void ContinueAmb1Audio()
    {
        amb1Source.volume = 0.0f;
        amb1Source.UnPause();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb1Source));
    }

    public void PlayAmb2Audio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Amb/{audioName}");
        amb2Source.clip = clip;
        amb2Source.volume = 0f;
        amb2Source.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb2Source));
    }

    public void PauseAmb2Audio()
    {
        amb2Source.Pause();
    }

    public void ContinueAmb2Audio()
    {
        amb2Source.volume = 0.0f;
        amb2Source.UnPause();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb2Source));
    }
    #endregion

    #region ≤•∑≈º«“‰
    public void PlayMemoryAudio(string memoryName)
    {
        StartCoroutine(AsyncPlayMemoryAudio(memoryName));
    }

    public void PauseMemoryAudio()
    {
        memorySource.Pause();
    }

    public void ContinueMemoryAudio()
    {
        memorySource.volume = 0.0f;
        memorySource.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, memorySource));
    }
    #endregion

    public void FadeInLoopAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioName);
        loopSource.clip = clip;
        loopSource.volume = 0.0f;
        loopSource.Play();
        StartCoroutine(FadeTo(1.0f, 5f, loopSource));
    }

    private IEnumerator FadeTo(float targetVolume, float duration, AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private AudioClip RandomLoadAudioClip(int audioSize, string path)
    {
        int randomIndex = Random.Range(0, audioSize);
        return Resources.Load<AudioClip>($"{path}_{randomIndex}");
    }

    private IEnumerator AsyncPlayMemoryAudio(string memoryName)
    {
        AudioClip intro = Resources.Load<AudioClip>($"Audio/Memory/intro");
        AudioClip loop = Resources.Load<AudioClip>($"Audio/Memory/{memoryName}");
        memorySource.clip = intro;
        memorySource.loop = false;
        memorySource.Play();
        yield return new WaitForSeconds(intro.length);
        memorySource.clip = loop;
        memorySource.loop = true;
        memorySource.Play();
    }
}
