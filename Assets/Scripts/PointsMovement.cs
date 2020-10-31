using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position tweener.
/// Interpolates the transform.position of a GameObject by looping through a list of positions defined in the inspector.
/// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
/// </summary>
public class PointsMovement : BaseTweener // TODO: Rename to PositionTweener!
{

    #region Fields and properties

    [SerializeField]
    [Tooltip("List of ordered position values to interpolate each interpolation loop.")]
    private List<Vector3> _values;

    private Vector3 _initialValue;

    #endregion

    #region Lifecycle

    private void Awake()
    {
        _initialValue = transform.position;
    }

    # endregion  
    

    #region TweenerBase overrides

    protected override int GetNumSteps() 
    {
        return _values.Count;
    }

    protected override void DoInitialLerp(float stepRatio) 
    {
        transform.position = Vector3.Lerp(_initialValue, _values[0], stepRatio);
    }

    protected override void DoLerp(int startIndex, int endIndex, float stepRatio)
    { 
         transform.position = Vector3.Lerp(_values[startIndex], _values[endIndex], stepRatio);
    }  

    #endregion

    #region Backup

    /* V2

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Position interpolation behaviour.
    /// Interpolates the transform.position of a GameObject by looping through a list of positions defined in the inspector.
    /// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
    /// </summary>
    public class PointsMovement : MonoBehaviour
    {

    #region Fields and properties

        //
        // Public
        //

        /// <summary>
        /// Number of interpolation loops completed since the script started.
        /// An interpolation loop includes all interpolations in the list of positions defined in the inspector.
        /// </summary>
        public int Loops { get; private set; }

        //
        // Inspector
        //

        [SerializeField]
        [Tooltip("List of ordered position values to interpolate each interpolation loop.")]
        private List<Vector3> _values;

        // Transition 
        [SerializeField]
        [Tooltip("Duration for each interpolation between two positions.")]
        private float _transition = 2f;

        //
        // Components/Children
        //

        private Renderer _myRenderer;

        //
        // Other
        //

        // Time elapsed since last interpolation start.
        private float _transitionStep;

        // Current interpolation start position.
        private Vector3 _currentValue;

        // Index to current interpolation end position.
        private int _valueIndex;

        #endregion

        #region Lifecycle

        private void Awake()
        {
            // Components/Children
            _myRenderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            // Init values.
            Loops = 0;
            _transitionStep = 0;
            _currentValue = transform.position;
            _valueIndex = 0;
            // Let's tween forever!
            StartCoroutine(nameof(TweenPosition));
        }

        private IEnumerator TweenPosition() 
        {
            while (true) {
                // Increase time elapsed since last interpolation start (_transitionStep) by delta time.
                _transitionStep += Time.deltaTime;
                // If time elapsed since last interpolation start (_transitionStep) is greater than interpolation time defined in the inspector (_transition)
                // then let's go for a new interpolation:
                if (_transitionStep > _transition) {
                    // Change interpolation start position (_currentValue).
                    _currentValue = _values[_valueIndex];
                    // Change index to interpolation end position (_valueIndex).
                    _valueIndex++;
                    // Check if a new loop is done.
                    if (_valueIndex >= _values.Count)
                    {
                        _valueIndex = 0;
                        Loops++;   
                    }
                    // Reset time elapsed since last step (set _transtionStep to 0f).
                    _transitionStep = 0f;           
                }
                // Next interpolation update!
                float step = _transitionStep / _transition;
                transform.position = Vector3.Lerp(_currentValue, _values[_valueIndex], step);            
                // Wait 'til next frame update.
                yield return null;
            }
        }

        # endregion 

    }

    */

    /* V1

    using System.Collections.Generic;
    using UnityEngine;

    public class PointsMovement : MonoBehaviour
    {
        public int Loops { get; private set; }

        [SerializeField]
        private List<Vector3> _values;

        [SerializeField]
        private float _transition = 2f;

        private Vector3 _currentValue;

        private float _transitionStep;

        private int _valueIndex;

        private void Start()
        {
            _transitionStep = 0;

            _valueIndex = 0;

            Loops = 0;

            _currentValue = transform.position;
        }

        void Update()
        {
            if (_transition > _transitionStep)
            {
                _transitionStep += Time.deltaTime;

                float step = _transitionStep / _transition;

                transform.position = Vector3.Lerp(_currentValue, _values[_valueIndex], step);
            }
            else
            {
                _transitionStep = 0;

                _currentValue = _values[_valueIndex];

                _valueIndex = (_valueIndex + 1) % _values.Count;

                Loops++;
            }
        }
    }

    */

    #endregion

}
