using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Track Animation component
    // Track animation clips
    // Receive animation events
    // Methods to play and stop animations.

    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimationClip;
    [SerializeField] private AnimationClip _fadeInAnimationClip;

    public void OnFadeOutComplete()
    {
        Debug.LogWarning("Fade out complete");
    }

    public void OnFadeInComplete()
    {
        Debug.LogWarning("Fade in complete");
        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimationClip;
        _mainMenuAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimationClip;
        _mainMenuAnimator.Play();
    }

}
