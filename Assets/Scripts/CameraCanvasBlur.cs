using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCanvasBlur : MonoBehaviour
{
    public static CameraCanvasBlur instance;

    [SerializeField]
    [Range(0, 10, order = 1)]
    int openDuration = 1;

    [SerializeField]
    [Range(0, 255, order = 1)]
    float alphaValue = 71;

    private Image image;
    private Button button;
    private void Awake()
    {
        instance = this;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void OpenBlur()
    {
        StartCoroutine(OpenBlurCo());
    }

    public void CloseBlur()
    {
        StartCoroutine(CloseBlurCo());
    }
    private IEnumerator OpenBlurCo()
    {
        image.enabled = true;
        float duration = 0;
        Color c = image.color;
        Color m = image.color;
        m.a = (alphaValue/256);
        while (duration <= openDuration)
        {
            image.color = Color.Lerp(c, m, (duration / openDuration));
            duration += Time.deltaTime;
            yield return null;
        }
        button.enabled = true;
    }

    private IEnumerator CloseBlurCo()
    {
        button.enabled = false;
        float duration = 0;
        Color c = image.color;
        Color m = image.color;
        m.a = 0;
        while (duration <= openDuration)
        {
            image.color = Color.Lerp(c, m, (duration / openDuration));
            duration += Time.deltaTime;
            yield return null;
        }
        image.enabled = false;
    }
}
