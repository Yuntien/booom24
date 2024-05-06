using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
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

    public int interactionSize;

    public float fadeTime;

    #region 随机播放
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

    #region 循环播放
    public void PlayLoopAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Loop/{audioName}");
        loopSource.clip = clip;
        loopSource.volume = 0f;
        loopSource.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, loopSource));
    }

    public void PauseLoopAudio()
    {
        loopSource.Pause();
    }

    public void ContinueLoopAudio()
    {
        loopSource.volume = 0.0f;
        loopSource.UnPause();
        StartCoroutine(FadeTo(1.0f, fadeTime, loopSource));
    }
    #endregion

    #region 播放记忆
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
        AudioClip intro = Resources.Load<AudioClip>($"Audio/Memory/{memoryName}_intro");
        AudioClip loop = Resources.Load<AudioClip>($"Audio/Memory/{memoryName}_loop");
        memorySource.clip = intro;
        memorySource.loop = false;
        memorySource.Play();
        yield return new WaitForSeconds(intro.length);
        memorySource.clip = loop;
        memorySource.loop = true;
        memorySource.Play();
    }
}
