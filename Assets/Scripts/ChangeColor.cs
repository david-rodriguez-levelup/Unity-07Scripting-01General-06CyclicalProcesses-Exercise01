using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Color tweener.
/// Interpolates the renderer's material.color of a GameObject by looping through a list of colors defined in the inspector.
/// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
/// </summary>
public class ChangeColor : BaseTweener // TODO: Rename to ColorTweener!
{

    #region Fields and properties

    [SerializeField]
    [Tooltip("List of ordered color values to interpolate each interpolation loop.")]
    private List<Color> _values;

    private Renderer _myRenderer;

    private Color _initialValue;

    #endregion

    #region Lifecycle

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _initialValue = _myRenderer.material.color;
    }

    # endregion  

    #region TweenerBase overrides

    protected override int GetNumSteps() 
    {
        return _values.Count;
    }

    protected override void DoInitialLerp(float stepRatio) 
    {
        _myRenderer.material.color = Color.Lerp(_initialValue, _values[0], stepRatio);
    }

    protected override void DoLerp(int startIndex, int endIndex, float stepRatio)
    {
        _myRenderer.material.color = Color.Lerp(_values[startIndex], _values[endIndex], stepRatio);
    }  

    #endregion

    #region Backup

    /* V2

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Color interpolation behaviour.
    /// Interpolates the renderer's material.color of a GameObject by looping through a list of colors defined in the inspector.
    /// Keeps a counter to the number of interpolation loops completed since the script started. See <see cref="Loops"/>.
    /// </summary>
    public class ChangeColor : MonoBehaviour
    {

        #region Fields and properties

        //
        // Public
        //

        /// <summary>
        /// Number of interpolation loops completed since the script started.
        /// An interpolation loop includes all interpolations in the list of colors defined in the inspector.
        /// </summary>
        public int Loops { get; private set; }

        //
        // Inspector
        //

        [SerializeField]
        [Tooltip("List of ordered color values to interpolate each interpolation loop.")]
        private List<Color> _values;

        // Transition 
        [SerializeField]
        [Tooltip("Duration for each interpolation between two colors.")]
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

        // Current interpolation start color.
        private Color _currentValue;

        // Index to current interpolation end color.
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
            _currentValue = _myRenderer.material.color;
            _valueIndex = 0;
            // Let's tween forever!
            StartCoroutine(nameof(TweenColor));
        }

        private IEnumerator TweenColor() 
        {
            while (true) {
                // Increase time elapsed since last interpolation start (_transitionStep) by delta time.
                _transitionStep += Time.deltaTime;
                // If time elapsed since last interpolation start (_transitionStep) is greater than interpolation time defined in the inspector (_transition)
                // then let's go for a new interpolation:
                if (_transitionStep > _transition) {
                    // Change interpolation start color (_currentValue).
                    _currentValue = _values[_valueIndex];
                    // Change index to interpolation end color (_valueIndex).
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
                _myRenderer.material.color = Color.Lerp(_currentValue, _values[_valueIndex], step);     
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

    public class ChangeColor : MonoBehaviour
    {
        public int Loops { get; private set; }

        [SerializeField]
        private List<Color> _values;

        [SerializeField]
        private float _transition = 2f;

        private float _transitionStep;

        private Renderer _myRenderer;

        private Color _currentValue;

        private int _valueIndex;

        private void Awake()
        {
            _myRenderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            _transitionStep = 0;

            _currentValue = _myRenderer.material.color;

            _valueIndex = 0;

            Loops = 0;
        }

        void Update()
        {

            if (_transition > _transitionStep)
            {
                _transitionStep += Time.deltaTime;

                float step = _transitionStep / _transition;

                _myRenderer.material.color = Color.Lerp(_currentValue, _values[_valueIndex], step);
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