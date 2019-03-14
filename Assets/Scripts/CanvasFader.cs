using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;
using System;

public class CanvasFader : MonoBehaviour
{
    [SerializeField] Image image;
    public float time = 1f;
    [SerializeField] LeanTweenType easeType = LeanTweenType.easeInOutQuint;

    public bool isFading = false;

    public Action OnFadeInComplete;
    public Action OnFadeOutComplete;

    public LTDescr FadeIn()
    {
        image.color = new Color(image.color.r, image.color.b, image.color.g, 0f);
        return LeanTween.value(0f, 1f, time)
            .setEase(easeType)
            .setOnUpdate((value) =>
            {
                image.color = new Color(image.color.r, image.color.b, image.color.g, value);
            })
            .setOnStart(() => {
                isFading = true;
                gameObject.SetActive(true);
            })
            .setOnComplete(() =>
            {
                isFading = false;
                if(OnFadeInComplete != null) OnFadeInComplete();
            });
    }

    public LTDescr FadeOut()
    {
        image.color = new Color(image.color.r, image.color.b, image.color.g, 1f);
        return LeanTween.value(1f, 0f, time)
            .setEase(easeType)
            .setOnUpdate((value) =>
            {
                image.color = new Color(image.color.r, image.color.b, image.color.g, value);
            })
            .setOnStart(() => {
                isFading = true;
                gameObject.SetActive(true);
            })
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
                isFading = false;
                if (OnFadeOutComplete != null) OnFadeOutComplete();
            });
    }
}
