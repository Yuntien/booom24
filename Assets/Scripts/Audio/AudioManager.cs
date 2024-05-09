using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private static Dictionary<string, string> nameReplace = new Dictionary<string, string>();

    [Header("ѭ����Ч")]
    [SerializeField] private AudioSource loopSource;
    [Header("������Ч")]
    [SerializeField] private AudioSource interactionSource;
    [Header("����")]
    [SerializeField] private AudioSource voiceSource;
    [Header("������Ч")]
    [SerializeField] private AudioSource memorySource;
    [Header("������Ч1")]
    [SerializeField] private AudioSource amb1Source;
    [Header("������Ч2")]
    [SerializeField] private AudioSource amb2Source;
    [Header("�����")]
    [SerializeField] public AudioMixer mixer;

    public int interactionSize;

    public float fadeTime;

    // ����

    private void Start()
    {
        nameReplace.Add("Seller", "huoshan");
        nameReplace.Add("Can", "huoshan_incan");
        nameReplace.Add("Firefly", "peipei");
        nameReplace.Add("Express", "shitou");
        nameReplace.Add("Himeko", "yingli");
    }

    #region �������
    public void RandomPlayVoice(string actorName, int voiceSize)
    {
        Debug.Log(actorName);
        string audioName;
        if (nameReplace.ContainsKey(actorName))
        {
            audioName = nameReplace[actorName];
        }
        else
        {
            audioName = actorName;
        }

        AudioClip clip = RandomLoadAudioClip(3, $"Audio/voice_{audioName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void RandomPlayInteraction(string interactionName)
    {
        AudioClip clip = RandomLoadAudioClip(interactionSize, $"Audio/{interactionName}");
        if (clip == null)
        {
            clip = Resources.Load<AudioClip>($"Audio/{interactionName}");
        }
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
    }

    public void StopInteraction()
    {
        interactionSource.Stop();
    }
    #endregion

    #region ���ų���
    public void PlayVoice(string actorName, string voiceName)
    {
        AudioClip clip = RandomLoadAudioClip(3, $"Audio/{actorName}");
        voiceSource.clip = clip;
        voiceSource.Play();
    }
    #endregion

    #region ѭ������
    public void PlayLoopAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{audioName}");
        if (clip == null)
        {
            clip = RandomLoadAudioClip(3, $"Audio/{audioName}");
        }
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

    public bool IsLoopPlaying()
    {
        return loopSource.isPlaying;
    }
    #endregion

    #region ������ѭ������
    public void PlayAmb1Audio(string audioName)
    {
        amb1Source.Stop();
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{audioName}");
        amb1Source.clip = clip;
        amb1Source.volume = 0f;
        amb1Source.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb1Source));
    }

    public void PauseAmb1Audio()
    {
        amb1Source.Pause();
    }

    public void FadeOutAmb1Audio()
    {
        StartCoroutine(FadeTo(0.0f, fadeTime, amb1Source));
    }

    public void ContinueAmb1Audio()
    {
        amb1Source.volume = 0.0f;
        amb1Source.UnPause();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb1Source));
    }

    public void PlayAmb2Audio(string audioName)
    {
        amb2Source.Stop();
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{audioName}");
        amb2Source.clip = clip;
        amb2Source.volume = 0f;
        amb2Source.Play();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb2Source));
    }

    public void PauseAmb2Audio()
    {
        amb2Source.Pause();
    }

    public void FadeOutAmb2Audio()
    {
        StartCoroutine(FadeTo(0.0f, fadeTime, amb2Source));
    }

    public void ContinueAmb2Audio()
    {
        amb2Source.volume = 0.0f;
        amb2Source.UnPause();
        StartCoroutine(FadeTo(1.0f, fadeTime, amb2Source));
    }
    #endregion

    #region ���ż���
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
        int randomIndex = Random.Range(0, audioSize) + 1;
        return Resources.Load<AudioClip>($"{path}_{randomIndex}");
    }

    private IEnumerator AsyncPlayMemoryAudio(string memoryName)
    {
        AudioClip intro = Resources.Load<AudioClip>($"Audio/mem_play");
        AudioClip loop = Resources.Load<AudioClip>($"Audio/{memoryName}");
        memorySource.clip = intro;
        memorySource.loop = false;
        memorySource.Play();
        yield return new WaitForSeconds(intro.length);
        memorySource.clip = loop;
        memorySource.loop = true;
        memorySource.Play();
    }
}
