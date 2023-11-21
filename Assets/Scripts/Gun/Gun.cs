using UnityEngine;

namespace ProjectZee
{
    public class Gun : MonoBehaviour
    {
        #region Variables

        [Header("Data")]
        [SerializeField] private GunDataSO gunData;

        [Header("References")]
        public Transform shellEjectionPoint;
        [Space]
        public Transform[] projectileSpawnPoints;
        [Space]
        public GameObject muzzleLight;
        public ParticleSystem muzzleFlash;

        private float nextShotTime;
        private float recoilAngle;
        private float recoilAngleSmoothDampVelocity;

        private int currentFireModeIndex = 0;
        private int shotsRemainingInBurst;

        private bool triggerReleasedSinceLastShot;

        private Vector3 originalPosition;
        private Vector3 recoilSmoothDampVelocity;

        private FireMode fireMode;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
            SetupGun();
        }

        private void Update()
        {
            ResetRecoil();
        }

        private void LateUpdate()
        {
            WeaponSway();
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        /// <summary>
        /// Setup initial information for gun when game is about to start.
        /// </summary>
        private void SetupGun()
        {
            shotsRemainingInBurst = gunData.burstCount;

            originalPosition = gunData.startPosition;

            // Set the initial mode to the first allowed mode in the array
            if (gunData.allowedFireModes.Length > 0)
            {
                SetFireMode(gunData.allowedFireModes[currentFireModeIndex], false);
            }

            DeactivateMuzzleLight();
        }

        /// <summary>
        /// Handles the shooting logic by checking the fire mode, rate, and ammo,
        /// spawning projectiles, playing sound, applying recoil, showing muzzle flash, and spawning shell.
        /// </summary>
        private void Shoot()
        {
            if (Time.time > nextShotTime)
            {
                if (fireMode == FireMode.Burst)
                {
                    if (shotsRemainingInBurst == 0) return;
                    shotsRemainingInBurst--;
                }
                else if (fireMode == FireMode.Single)
                {
                    if (!triggerReleasedSinceLastShot) return;
                }

                nextShotTime = Time.time + gunData.fireRateMs / 1000f;

                SpawnProjectile();
                SpawnShell();

                // Play muzzle flash.
                muzzleFlash.Play();
                ActivateMuzzleLight();
                Invoke(nameof(DeactivateMuzzleLight), 0.1f);

                Recoil();

                // Play shoot sound;
                AudioManager.instance.PlaySound(gunData.shootAudio, transform.position);
            }
        }

        /// <summary>
        /// Invokes the Shoot method when the trigger is held and sets the trigger state.
        /// </summary>
        public void OnTriggerHold()
        {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }

        /// <summary>
        /// Resets the burst count and sets the trigger state.
        /// </summary>
        public void OnTriggerRelease()
        {
            shotsRemainingInBurst = gunData.burstCount;
            triggerReleasedSinceLastShot = true;
        }

        /// <summary>
        /// Applies the recoil logic by adjusting the gun's position based on recoil values.
        /// </summary>
        private void Recoil()
        {
            // Recoil in x, y, x axis. (kick back in z axis)
            if (gunData.useRecoil)
            {
                Vector3 recoil = new(gunData.recoilPattern.x, gunData.recoilPattern.y, -gunData.recoilPattern.z);
                transform.localPosition += recoil;
            }
        }

        /// <summary>
        /// Resets the recoil by smoothly transforming the gun's position back to its original state.
        /// </summary>
        private void ResetRecoil()
        {
            // Smoothly reset the gun's position.
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, originalPosition, ref recoilSmoothDampVelocity, gunData.recoilSettleTime);
        }

        private void WeaponSway()
        {
            Vector2 mouseAxis = new(
                Input.GetAxisRaw("Mouse X"),
                Input.GetAxisRaw("Mouse Y")
                );

            transform.localPosition += (Vector3)mouseAxis * -gunData.weaponSwayAmount / 1000f;
        }

        /// <summary>
        /// Spawns projectiles from the projectile spawn points based on the gun's configuration.
        /// </summary>
        private void SpawnProjectile()
        {
            for (int i = 0; i < projectileSpawnPoints.Length; i++)
            {
                // Apply spread to the projectile direction
                Vector3 spreadDirection;

                if (gunData.addBulletSpread) spreadDirection = projectileSpawnPoints[i].forward + Random.insideUnitSphere * gunData.spread;
                else spreadDirection = projectileSpawnPoints[i].forward + Random.insideUnitSphere * 0;


                Projectile newProjectile = Instantiate(
                    gunData.projectile,
                    projectileSpawnPoints[i].position,
                    Quaternion.LookRotation(spreadDirection)
                    );

                newProjectile.SetProjectileRange(gunData.maxRange);
                newProjectile.SetProjectileSpeed(gunData.projectileVelocity);
            }
        }

        /// <summary>
        /// Spawns shell objects from the shell ejection point based on the gun's configuration.
        /// </summary>
        private void SpawnShell()
        {
            if (!gunData.useShell) return;
            Shell newShell = Instantiate(gunData.shell, shellEjectionPoint.position, shellEjectionPoint.rotation);
        }

        /// <summary>
        /// Activates the muzzle light game object.
        /// </summary>
        private void ActivateMuzzleLight()
        {
            if (!muzzleLight.activeInHierarchy) muzzleLight.SetActive(true);
        }

        /// <summary>
        /// Deactivates the muzzle light game object.
        /// </summary
        private void DeactivateMuzzleLight()
        {
            if (muzzleLight.activeInHierarchy) muzzleLight.SetActive(false);
        }

        /// <summary>
        /// Handles the cycling of fire modes by updating the current mode index and setting the fire mode accordingly.
        /// </summary>
        public void CycleFireMode()
        {
            // Increment the current mod index.
            currentFireModeIndex++;

            // If the index goes beyond the array bounds, reset to 0
            if (currentFireModeIndex >= gunData.allowedFireModes.Length)
            {
                currentFireModeIndex = 0;
            }

            // Set the gun mode to the newly selected mode.
            SetFireMode(gunData.allowedFireModes[currentFireModeIndex]);
        }

        /// <summary>
        /// Sets the fire mode based on the selected mode and applies the corresponding logic for each mode.
        /// </summary>
        private void SetFireMode(FireMode mode, bool playChangeSound = true)
        {
            // Logic to apply the selected mod to the gun.
            switch (mode)
            {
                case FireMode.Auto:
                    // Apply auto mode logic.
                    fireMode = FireMode.Auto;
                    Debug.Log("Auto mode selected");
                    break;

                case FireMode.Burst:
                    // Apply burst mode logic.
                    fireMode = FireMode.Burst;
                    Debug.Log("Burst mode selected");
                    break;

                case FireMode.Single:
                    // Apply single mode logic.
                    fireMode = FireMode.Single;
                    Debug.Log("Single mode selected");
                    break;
                    // Add cases for more mods as needed
            }

            // Play chnageFireMode sound if is allowed.
            if (playChangeSound)
            {
                AudioManager.instance.PlaySound
                    (
                    gunData.fireModeAudio[Random.Range(0, gunData.fireModeAudio.Length)],
                    transform.position
                    );
            }
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}
