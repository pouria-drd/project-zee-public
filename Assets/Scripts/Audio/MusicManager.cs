using UnityEngine;

namespace ProjectZee
{
    public class MusicManager : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private AudioClip mainTheme;
        [SerializeField] private AudioClip menuTheme;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
            AudioManager.instance.PlayMusic(menuTheme, 2);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) AudioManager.instance.PlayMusic(mainTheme, 3);
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}
