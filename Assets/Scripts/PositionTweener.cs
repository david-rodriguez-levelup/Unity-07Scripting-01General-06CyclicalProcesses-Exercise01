using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position tweener.
/// Interpolates the transform.position of a GameObject by looping through the list of position steps defined in the inspector.
/// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
/// </summary>
public class PositionTweener : BaseTweener
{

    #region Fields/Properties

    [SerializeField]
    [Tooltip("List of ordered position steps to interpolate each loop.")]
    private List<Vector3> _steps;

    // Initial position of the object. Needed for the initial lerp.
    private Vector3 _initialPosition;

    #endregion

    #region Lifecycle

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    # endregion  
    

    #region BaseTweener overrides

    protected override int GetNumSteps() 
    {
        return _steps.Count;
    }  

    protected override void DoInitialLerp(float stepRatio) 
    {
        transform.position = Vector3.Lerp(_initialPosition, _steps[0], stepRatio);
    }

    protected override void DoLerp(int startIndex, int endIndex, float stepRatio)
    { 
         transform.position = Vector3.Lerp(_steps[startIndex], _steps[endIndex], stepRatio);
    }  

    #endregion

}
