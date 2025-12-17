using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private const float FullAlpha = 1f;
    private const float ZeroAlpha = 0f;
    private const float InitialTimer = 0f;
    
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _fadeSpeed = 1f;
    
    private Color _startValue;
    private Coroutine _fadeCoroutine;
    
    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
            
        _startValue = _renderer.material.color;
    }
    
    public void StartFade(float duration)
    {
        StopFade();
        
        _fadeCoroutine = StartCoroutine(FadeRoutine(duration));
    }
    
    public void StopFade()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
    }
    
    public void ResetVisuals()
    {
        StopFade();
        
        Color currentColor = _renderer.material.color;
        currentColor.a = FullAlpha;
        _renderer.material.color = currentColor;
    }
    
    private IEnumerator FadeRoutine(float duration)
    {
        float timer = InitialTimer;
        Material material = _renderer.material;
        
        while (timer < duration)
        {
            timer += Time.deltaTime * _fadeSpeed;
            float progress = timer / duration;
            Color currentColor = material.color;
            currentColor.a = FullAlpha - progress;
            material.color = currentColor;

            yield return null;
        }
        
        Color finalColor = material.color;
        finalColor.a = ZeroAlpha;
        material.color = finalColor;
    }
}