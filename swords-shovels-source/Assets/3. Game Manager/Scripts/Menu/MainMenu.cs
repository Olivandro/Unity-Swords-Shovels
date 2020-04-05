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

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start() 
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    public void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
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

    private void HandleGameStateChange(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        if (previousGameState == GameManager.GameState.PREGAME && currentGameState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }

        if (previousGameState != GameManager.GameState.PREGAME && currentGameState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

}
