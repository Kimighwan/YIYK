using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // �ΰ�
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    // Ÿ��Ʋ
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgressTxt;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        LogoAnim.gameObject.SetActive(true);
        Title.SetActive(false);
    }

    private void Start()
    {
        // UIManager.Instance.EnableGoodsUI(false);

        StartCoroutine(LoadGameCo());
    }

    private IEnumerator LoadGameCo()
    {
        LogoAnim.Play();
        yield return new WaitForSeconds(LogoAnim.clip.length);

        LogoAnim.gameObject.SetActive(false);
        Title.SetActive(true); 

        _asyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);
        if (_asyncOperation == null)
        {
            yield break;
        }

        _asyncOperation.allowSceneActivation = false;

        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";
        yield return new WaitForSeconds(0.5f);

        while (!_asyncOperation.isDone) // �ε��� ���� ���� ��
        {
            LoadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";

            // �ε��� �Ϸ�ȴٸ� �κ�� ����ȯ �� �ڷ�ƾ ����
            if (_asyncOperation.progress >= 0.9f)
            {
                _asyncOperation.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }
}