using UnityEngine;

namespace ProjectZee
{
    [CreateAssetMenu(fileName = "GunDataSO", menuName = "Project Zee/New Gun Data", order = 0)]
    public class GunDataSO : ScriptableObject
    {
        #region Variables

        [Header("Settings")]
        public GunType gunType = GunType.Assault;
        public FireMode[] allowedFireModes;
        [Space]
        [Range(1f, 100f)] public float maxRange = 25f;
        [Range(10f, 3000f)] public float fireRateMs = 100f;
        [Range(10f, 100f)] public float projectileVelocity = 35f;

        [Header("Sounds")]
        public AudioClip shootAudio;
        public AudioClip reloadAudio;
        public AudioClip[] fireModeAudio;

        [Header("Recoil")]
        public Vector2 kickMinMax = new(0.025f, 0.05f);
        public Vector2 recoilAngleMinMax = new(5f, 10f);
        [Range(0.01f, 1f)] public float recoilSettleTime = 0.1f;
        [Range(0.01f, 1f)] public float recoilRotationSettleTime = 0.1f;

        [Header("Bullet Spread")]
        public bool addBulletSpread = true;
        [Range(0.01f, 0.1f)] public float spread = 0.04f;

        [Header("Burst")]
        public int burstCount = 3;

        [Header("Projectile")]
        public Projectile projectile;

        [Header("Shell")]
        public Shell shell;

        #endregion
    }
}
