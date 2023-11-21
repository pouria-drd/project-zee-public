using UnityEngine;
using System.Collections;

namespace ProjectZee
{
    [RequireComponent(typeof(Rigidbody))]
    public class Shell : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Rigidbody myRB;

        [Header("Settings")]
        [SerializeField][Range(1f, 10f)] private float lifeTime = 4f;
        [SerializeField][Range(1f, 10f)] private float fadeTime = 2f;
        [SerializeField][Range(1f, 100f)] private float minEjectionForce = 90f;
        [SerializeField][Range(100f, 500f)] private float maxEjectionForce = 150f;

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        private void Start()
        {
            if (!myRB) myRB = GetComponent<Rigidbody>();

            SetupShell();

            StartCoroutine(FadeShell());
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        private void SetupShell()
        {
            float ejectionForce = Random.Range(minEjectionForce, maxEjectionForce);
            myRB.AddForce(transform.right * ejectionForce);
            myRB.AddTorque(Random.insideUnitSphere * ejectionForce);
        }

        private IEnumerator FadeShell()
        {
            yield return new WaitForSeconds(lifeTime);

            float percent = 0f;
            float fadeSpeed = 1 / fadeTime;

            Material mat = myRB.GetComponent<Renderer>().material;

            Color initialColor = mat.color;

            while (percent < 1)
            {
                percent += Time.deltaTime * fadeSpeed;
                mat.color = Color.Lerp(initialColor, Color.clear, percent);
                yield return null;
            }

            Destroy(gameObject);
        }

        #endregion

        // ----------------------------------------------------------------------

    }
}
