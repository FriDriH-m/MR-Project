using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class ForceDisableRenderer : MonoBehaviour
{
    Renderer _renderer;

    void Awake()
    {
        CacheRenderer();
        DisableRenderer();
    }

    void OnEnable()
    {
        DisableRenderer();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        CacheRenderer();
        DisableRenderer();
    }
#endif

    void LateUpdate()
    {
        if (_renderer == null)
            return;

        if (_renderer.enabled)
            _renderer.enabled = false;

        if (!_renderer.forceRenderingOff)
            _renderer.forceRenderingOff = true;
    }

    void CacheRenderer()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
    }

    void DisableRenderer()
    {
        if (_renderer == null)
            return;

        _renderer.enabled = false;
        _renderer.forceRenderingOff = true;
    }
}
