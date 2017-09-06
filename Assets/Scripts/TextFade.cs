using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    float fadeRange = 1;

    [SerializeField]
    Text text;

    [SerializeField]
    Material material;

    void Start()
    {
        if (!material)
            material = GetComponent<Text>().material;

        if (!text)
            text = GetComponent<Text>();
    }

    void UpdateUniform()
    {
        material.SetFloat("_Fade", fadeRange);
    }

    public void FadeAndTextChange(float time, string nextStr)
    {
        StopCoroutine("FadeAndTextChangeCroutine");
        fadeRange = 1.0f;
        StartCoroutine(FadeAndTextChangeCroutine(time, nextStr));
    }

    public void FadeIn(float time)
    {
        StopCoroutine("FadeInCoroutine");
        fadeRange = 0.0f;
        StartCoroutine(FadeInCoroutine(time));
    }

    public void FadeOut(float time)
    {
        StopCoroutine("FadeOutCoroutine");
        fadeRange = 1.0f;
        StartCoroutine(FadeOutCoroutine(time));
    }

    IEnumerator FadeAndTextChangeCroutine(float time, string nextStr)
    {
        while (true)
        {
            fadeRange -= Time.deltaTime / time;

            if (fadeRange <= 0.0f)
            {
                fadeRange = 0;
                UpdateUniform();
                break;
            }

            UpdateUniform();

            yield return null;
        }

        text.text = nextStr;

        while (true)
        {
            fadeRange += Time.deltaTime / time;

            if (fadeRange >= 1.0f)
            {
                fadeRange = 1.0f;
                UpdateUniform();
                break;
            }

            UpdateUniform();

            yield return null;
        }
    }

    IEnumerator FadeOutCoroutine(float time)
    {
        while (true)
        {
            fadeRange -= Time.deltaTime / time;

            if (fadeRange <= 0.0f)
            {
                fadeRange = 0;
                UpdateUniform();
                break;
            }

            UpdateUniform();

            yield return null;
        }
    }

    IEnumerator FadeInCoroutine(float time)
    {
        while (true)
        {
            fadeRange += Time.deltaTime / time;

            if (fadeRange >= 1.0f)
            {
                fadeRange = 1.0f;
                UpdateUniform();
                break;
            }

            UpdateUniform();

            yield return null;
        }
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        UpdateUniform();
    }
#endif
}
