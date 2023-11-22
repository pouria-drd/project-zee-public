using UnityEngine;

namespace ProjectZee
{
    [RequireComponent(typeof(RectTransform))]
    public class Reticle : MonoBehaviour
    {
        #region Variables

        [Header("Reticle")]
        [SerializeField][Range(1, 100)] private float normalSize = 25;
        [SerializeField][Range(1, 100)] private float maximumSize = 75;
        [SerializeField][Range(1, 10)] private float transitionSpeed = 25;

        private float currentSize;

        private RectTransform reticle;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
            reticle = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            ReticleController();
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        private void ReticleController()
        {
            if (IsMoving)
            {
                currentSize = Mathf.Lerp(currentSize, maximumSize, Time.deltaTime * transitionSpeed);
            }
            else
            {
                currentSize = Mathf.Lerp(currentSize, normalSize, Time.deltaTime * transitionSpeed);
            }

            reticle.sizeDelta = new Vector2(currentSize, currentSize);
        }

        private bool IsMoving
        {
            get
            {
                if (
                    Input.GetAxis("Vertical") != 0f ||
                    Input.GetAxis("Horizontal") != 0f ||

                    Input.GetAxis("Mouse X") != 0f ||
                    Input.GetAxis("Mouse Y") != 0f
                ) return true;

                else return false;
            }
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}
