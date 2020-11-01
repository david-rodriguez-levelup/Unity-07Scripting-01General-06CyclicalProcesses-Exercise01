using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract base class for all step-based tweeners.
/// Starts a corroutine that helps with the execution of lerp functions in the subclasses.
/// Keeps a counter with the number of loops completed since the script started. See <see cref="Loops"/>.
/// Each subclass manages its own list of steps.
/// A step is defined by an interpolation between to values.
/// A loop is completed each time the list of steps is traversed.
/// <see cref="ColorTweener"/>.
/// <see cref="PositionTweener"/>.
/// </summary>
public abstract class BaseTweener : MonoBehaviour
{

    #region Fields/Properties

    /// <summary>
    /// Number of loops completed since the script started.
    /// A loop is completed by going through all the steps in the list managed by the subclass.
    /// </summary>
    public int Loops { get; private set; }

    [SerializeField]
    [Tooltip("Duration of each step (seconds).")]
    private float _stepDuration = 2f;

    // Time elapsed since current step started.
    private float _stepTimeElapsed;

    // Index to current step end value in the list of steps managed by the subclass.
    private int _endIndex;

    #endregion

    #region Lifecycle

    private void Start()
    {
        // Init values!
        Loops = 0;
        _stepTimeElapsed = 0;
        _endIndex = 0;
        // Let's tween forever!
        StartCoroutine(nameof(TweenValues));
    }

    private IEnumerator TweenValues() 
    {

        int startIndex = -1;

        while (true) {
        
            // Increase time elapsed since current step started (_stepTimeElapsed) by delta time.
            _stepTimeElapsed += Time.deltaTime;
        
            // If time elapsed since current step started (_stepTimeElapsed) is greater than interpolation time defined in the inspector (_stepDuration)
            // then let's go for a new step:
            if (_stepTimeElapsed > _stepDuration) {                
                // Change index to interpolation end value (_valueIndex).
                startIndex = _endIndex;
                _endIndex++;
                // Check if a new loop is done.
                int numSteps = GetNumSteps();
                if (_endIndex >= numSteps)
                {
                    startIndex = numSteps - 1;
                    _endIndex = 0;
                    Loops++;                 
                }
                // Reset time elapsed since last step (set _transtionStep to 0f).
                _stepTimeElapsed = 0f;
            }
        
            // Call subclasses for the next lerp update!
            float stepRatio = _stepTimeElapsed / (_stepDuration);
            if (startIndex == -1) 
            {
                DoInitialLerp(stepRatio);
            }
            else
            {
                DoLerp(startIndex, _endIndex, stepRatio);
            } 

            // Wait 'til next frame update.
            yield return null;
        
        }
    
    }

    #endregion

    #region Abstract methods

    /// <summary>
    /// Subclasses must override this method to return the number of steps in its own list.
    /// </summary>
    /// <return>The number of interpolation values.</return>
    protected abstract int GetNumSteps();

    /// <summary>
    /// Subclasses must override this method to manage the lerp from initial state to the first value in the list of steps.
    /// </summary>
    /// <param name="stepRatio">Parameter (t) of the interpolation in the closed unit interval [0, 1].</param>
    protected abstract void DoInitialLerp(float stepRatio);

    /// <summary>
    /// Subclasses must override this method to manage the lerp from startIndex to endIndex values in its own list.
    /// </summary>
    /// <param name="startIndex">Index to the start value of the interpolation in the list managed by the subclass.</param>
    /// <param name="endIndex">Index to the end value of the interpolation in the list managed by the subclass.</param>
    /// <param name="stepRatio">Parameter (t) of the interpolation in the closed unit interval [0, 1].</param>
    protected abstract void DoLerp(int startIndex, int endIndex, float stepRatio);

    #endregion

}
