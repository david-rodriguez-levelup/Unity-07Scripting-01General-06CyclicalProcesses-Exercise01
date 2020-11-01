using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Color tweener.
/// Interpolates the renderer's material.color of a GameObject by looping through the list of color steps defined in the inspector.
/// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
/// </summary>
public class ColorTweener : BaseTweener
{

    #region Fields/Properties

    [SerializeField]
    [Tooltip("List of ordered color steps to interpolate each loop.")]
    private List<Color> _steps;

    // Renderer component in the GameObject.
    private Renderer _myRenderer;

    // Initial color of the object. Needed for the initial lerp.
    private Color _initialColor;

    #endregion

    #region Lifecycle

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _initialColor = _myRenderer.material.color;
    }

    # endregion  

    #region BaseTweener overrides

    protected override int GetNumSteps() 
    {
        return _steps.Count;
    }

    protected override void DoInitialLerp(float stepRatio) 
    {
        _myRenderer.material.color = Color.Lerp(_initialColor, _steps[0], stepRatio);
    }

    protected override void DoLerp(int startIndex, int endIndex, float stepRatio)
    {
        _myRenderer.material.color = Color.Lerp(_steps[startIndex], _steps[endIndex], stepRatio);
    }  

    #endregion

}