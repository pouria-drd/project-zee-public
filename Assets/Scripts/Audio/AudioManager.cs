using UnityEngine;
using System.Collections;

namespace ProjectZee
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]
        [SerializeField][Range(0f, 1f)] private float sfxVolumePercent = 1f;
        [SerializeField][Range(0f, 1f)] private float musicVolumePercent = 1f;
        [SerializeField][Range(0f, 1f)] private float masterVolumePercent = 0.2f;

        AudioSource[] musicSources;
        int activeMusicSourceIndex;

        public static AudioManager instance;

        Transform audioListener;
        Transform playerT;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Awake()
        {
            instance = this;

            musicSources = new AudioSource[2];

            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            audioListener = FindObjectOfType<AudioListener>().transform;
            playerT = GameObject.FindGameObjectWithTag("Player").transform;
        }

        /* private void Update()
        {
            if (playerT != null) audioListener.position = playerT.position;
        }*/

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        public void PlayMusic(AudioClip clip, float fadeDuration = 1)
        {
            activeMusicSourceIndex = 1 - activeMusicSourceIndex;
            musicSources[activeMusicSourceIndex].clip = clip;
            musicSources[activeMusicSourceIndex].Play();

            StartCoroutine(AnimateMusicCrossfade(fadeDuration));
        }

        public void PlaySound(AudioClip clip, Vector3 position)
        {
            if (clip != null) AudioSource.PlayClipAtPoint(clip, position, sfxVolumePercent * masterVolumePercent);
        }

        IEnumerator AnimateMusicCrossfade(float duration)
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * 1 / duration;
                musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
                musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
                yield return null;
            }
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}
