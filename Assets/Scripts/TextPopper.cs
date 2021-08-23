using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextPopper : MonoBehaviour
{
    // Start is called before the first frame update
    private Text _text;
    private Image _image;
    private string _displayText;
    private CanvasGroup _canvas;

    void Start()
    {
        _text = GetComponentInChildren<Text>(); // get the child text component here
        _image = GetComponentInChildren<Image>();
        _canvas = GetComponent<CanvasGroup>();
        ShowFlickerText("Rise and Shine...");
    }
    public void SetDisplayText(string value)
    {
        _displayText = value;
        _text.text = value;
    }

    public void ShowFlickerText(string value)
    {
        SetDisplayText(value);
        StartCoroutine(FlickerIn(3.0f));
    }

    IEnumerator FlickerIn(float time)
    {
        EasingFunction.Ease ease = EasingFunction.Ease.EaseInElastic;
        EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);

        var elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            _canvas.alpha = func(_canvas.alpha, 1.0f, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return FlickerOut(time - 1.0f);
    }

    IEnumerator FlickerOut(float time)
    {
        EasingFunction.Ease ease = EasingFunction.Ease.EaseInElastic;
        EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);

        var elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            _canvas.alpha = func(_canvas.alpha, -1.0f, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
