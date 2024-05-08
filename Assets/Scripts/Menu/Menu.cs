using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Menu : Singleton<Menu>
{
    [Header("�˵�����")]
    [SerializeField] private Image menuBackground;
    [Header("����")]
    [SerializeField] private Image tv;
    [Header("�˵�����")]
    [SerializeField] private CanvasGroup menuText;

    [Header("����ͼƬ")]
    [SerializeField] private Sprite news;
    [SerializeField] private Sprite champion;

    [Header("�����л��¼�")]
    [SerializeField] private FadeEventSO sceneFadeEventSO;

    [Header("�����л�ʱ��")]
    [SerializeField] private float tvDuration;

    [SerializeField] private Button start;

    [SerializeField] private GameObject tabPanel;

    [SerializeField] private GameObject levelPanel;

    [Header("�ؿ�ѡ��")]
    [SerializeField] private GameObject levelChoicePrefab;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var levelButtonObj = Instantiate(levelChoicePrefab, Vector3.zero, Quaternion.identity);
            levelButtonObj.transform.SetParent(levelPanel.transform);
            levelButtonObj.GetComponent<TextMeshPro>().text = $"�ؿ�{i}";
            Button levelButton = levelButtonObj.GetComponent<Button>();
            levelButton.onClick.AddListener(() => { ChooseLevel(i); });
        }

        var returnObj = Instantiate(levelChoicePrefab, Vector3.zero, Quaternion.identity);
        returnObj.transform.SetParent(levelPanel.transform);
        returnObj.GetComponent<TextMeshPro>().text = "����";
        Button returnButton = returnObj.GetComponent<Button>();
        returnButton.onClick.AddListener(() => {
            CloseLevelChoice();
        });
    }

    private void CloseLevelChoice()
    {
        levelPanel.SetActive(false);

        Button[] buttons = levelPanel.GetComponentsInChildren<Button>();
        foreach (Button levelButton in buttons)
        {
            levelButton.enabled = false;
        }

        tabPanel.SetActive(true);
    }

    private void OnEnable()
    {
        sceneFadeEventSO.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        sceneFadeEventSO.OnEventRaised -= OnFadeEvent;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ContinueGame()
    {

        // TODO ��Ӽ�����Ϸ�߼�
    }

    public void ShowLevel()
    {
        // չʾѡ�ؽ���
        tabPanel.SetActive(false);

        Button[] buttons = levelPanel.GetComponentsInChildren<Button>();
        foreach (Button levelButton in buttons)
        {
            levelButton.enabled = true;
        }

        levelPanel.SetActive(true);
    }

    private void ChooseLevel(int index)
    {
        Button[] buttons = levelPanel.GetComponentsInChildren<Button>();
        foreach (Button levelButton in buttons)
        {
            levelButton.enabled = false;
        }

    }

    public void StartNewGame()
    {
        // TODO ��������Ϸ
        GameManager.Instance.StartNewGame();
    }

    /// <summary>
    /// ���ֵ���Ч��
    /// </summary>
    /// <returns>Dotween�ص�</returns>
    public Tweener TextFadeOut()
    {
        Tweener fadeCallBack = menuText.DOFade(0, 2);
        start.enabled = false;
        fadeCallBack.OnComplete(() =>
        {
            menuText.gameObject.SetActive(false);
            start.enabled = true;
        });
        return fadeCallBack;
    }

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        menuBackground.DOBlendableColor(target, duration);
    }

    public Tweener FadeIn(float duration)
    {
        menuBackground.gameObject.SetActive(true);
        tv.DOBlendableColor(Color.white, duration);
        return menuBackground.DOBlendableColor(Color.white, duration);
    }

    public Tweener FadeOut(float duration)
    {
        tv.DOBlendableColor(Color.clear, duration);

        return menuBackground.DOBlendableColor(Color.clear, duration);
    }

    public Tweener TVSwitch(string state)
    {
        switch (state)
        {
            case "����":
                return tv.DOBlendableColor(Color.clear, tvDuration);
            case "����":
                AudioManager.Instance.RandomPlayInteraction("tv_in");
                tv.color = Color.black;
                tv.sprite = news;
                return tv.DOBlendableColor(Color.white, tvDuration);
            case "���۹ھ�":
                AudioManager.Instance.RandomPlayInteraction("tv_in");
                tv.color = Color.black;
                tv.sprite = champion;
                return tv.DOBlendableColor(Color.white, tvDuration);
            default:
                return null;
        }
    }
}