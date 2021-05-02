using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAfterTime : MonoBehaviour
{
    public float WaitToFade = 2f;
    public float FadeTime = 1f;

    [SerializeField]
    private TMP_Text tmpText;


    public void SetText(string text)
    {
        text = text.Replace("NL", "\n");
        tmpText.SetText(text);
    }

    public void OnEnable()
    {
        var initColor = tmpText.color;
        initColor.a = 1f;
        var endColor = initColor;
        endColor.a = 0f;

        tmpText.color = initColor;
        LeanTween.delayedCall(WaitToFade,
            () => LeanTween.value(gameObject, (Color c) => tmpText.color = c, initColor, endColor, FadeTime)
            .setOnComplete(
                () => gameObject.SetActive(false)
                )
            );
    }
}
