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
        public Vector3 normalLocalPosition;
        public Vector3 aimingLocalPosition;
        [Space]
        [Range(1f, 100f)] public float aimSpeed = 10f;
        [Range(1f, 100f)] public float maxRange = 25f;
        [Range(50f, 3000f)] public float fireRateMs = 100f;
        [Range(10f, 100f)] public float projectileVelocity = 35f;

        [Header("Sounds")]
        public AudioClip shootAudio;
        public AudioClip reloadAudio;
        public AudioClip emptyMagShootAudio;
        public AudioClip[] fireModeAudio;

        [Header("Recoil")]
        public bool useRecoil = true;
        [Range(0.01f, 1f)] public float kickForce = 0.1f;
        public Vector3 recoilPattern = new(0.015f, 0.015f, 0.025f);
        [Range(0.1f, 1f)] public float recoilSettleTime = 0.1f;

        [Header("Weapon Sway")]
        [Range(1f, 10f)] public float weaponSwayAmount = 1f;

        [Header("Bullet Spread")]
        public bool addBulletSpread = true;
        [Range(0.01f, 0.1f)] public float spread = 0.04f;

        [Header("Burst")]
        public int burstCount = 3;

        [Header("Projectile")]
        public Projectile projectile;

        [Header("Shell")]
        public bool useShell = true;
        public Shell shell;

        #endregion
    }
}
