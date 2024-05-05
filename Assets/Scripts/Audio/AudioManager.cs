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

    public int voiceSize;
    public int interactionSize;

    public float fadeTime;

    private void Start()
    {
        var clip = Resources.Load<AudioClip>("Audio/xxx");
        if (clip == null)
        {
            Debug.Log("空的");
        }
    }

    #region 随机播放
    public void RandomPlayVoice(string actorName)
    {
        AudioClip clip = RandomLoadAudioClip(voiceSize, $"Audio/Voice/{actorName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void RandomPlayInteraction(string interactionName)
    {
        AudioClip clip = RandomLoadAudioClip(voiceSize, $"Audio/Interaction/{interactionName}");
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
        AudioClip clip = Resources.Load<AudioClip>(audioName);
        loopSource.clip = clip;
        loopSource.Play();
    }

    public void PauseLoopAudio()
    {
        loopSource.Pause();
    }

    public void ContinueLoopAudio()
    {
        loopSource.volume = 0.0f;
        loopSource.Play();
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
        return Resources.Load<AudioClip>($"{path}/{randomIndex}");
    }

    private IEnumerator AsyncPlayMemoryAudio(string memoryName)
    {
        AudioClip intro = Resources.Load<AudioClip>($"Audio/Memory/{memoryName}/intro");
        AudioClip loop = Resources.Load<AudioClip>($"Audio/Memory/{memoryName}/loop");
        memorySource.clip = intro;
        memorySource.loop = false;
        memorySource.Play();
        yield return new WaitForSeconds(intro.length);
        memorySource.clip = loop;
        memorySource.loop = true;
        memorySource.Play();
    }
}
