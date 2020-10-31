using System.Collections;
using UnityEngine;

/// <summary>
/// Base tweener.
/// Interpolates the value of a GameObject by looping through a list of values defined in the inspector.
/// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
/// <see cref="ChangeColor"/>.
/// <see cref="PointsMovement"/>.
/// </summary>
public abstract class BaseTweener : MonoBehaviour
{

    #region Fields and properties

    /// <summary>
    /// Number of interpolation loops completed since the script started.
    /// An interpolation loop includes all interpolations in the list of values defined in the inspector.
    /// </summary>
    public int Loops { get; private set; }

    // Transition 
    [SerializeField]
    [Tooltip("Duration for each interpolation between two values.")]
    private float _transition = 2f;

    // Time elapsed since last interpolation start.
    private float _transitionStep;

    // Index to current interpolation end value.
    private int _endIndex;

    #endregion

    #region Lifecycle

    private void Start()
    {
        // Init values.
        Loops = 0;
        _transitionStep = 0;
        _endIndex = 0;
        // Let's tween forever!
        StartCoroutine(nameof(TweenValues));
    }

    private IEnumerator TweenValues() 
    {
        int startIndex = -1;
        while (true) {
            // Increase time elapsed since last interpolation start (_transitionStep) by delta time.
            _transitionStep += Time.deltaTime;
            // If time elapsed since last interpolation start (_transitionStep) is greater than interpolation time defined in the inspector (_transition)
            // then let's go for a new interpolation:
            if (_transitionStep > _transition) {                
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
                _transitionStep = 0f;          
            }
            // Next interpolation update!
            float stepRatio = _transitionStep / _transition;
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
    /// Subclasses must override this method to return the number of interpolation values.
    /// </summary>
    /// <return>The number of interpolation values.</return>
    protected abstract int GetNumSteps();

    /// <summary>
    /// Subclasses must override this method to manage the initial lerp from initial state to the first element in the list of interpolation values.
    /// </summary>
    protected abstract void DoInitialLerp(float stepRatio);

    /// <summary>
    /// Subclasses must override this method to manage the lerp from startIndex to endIndex values.
    /// </summary>
    protected abstract void DoLerp(int startIndex, int endIndex, float stepRatio);

    #endregion

}
