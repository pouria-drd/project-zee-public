using UnityEngine;

namespace ProjectZee
{
    public class GunController : MonoBehaviour
    {
        #region Variables
        [Header("Gun UI")]
        [SerializeField] private GameObject crossHair;

        [Header("Key Binds")]
        [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
        [SerializeField] private bool holdKeyForAiming = false;
        [SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode changeFireModeKey = KeyCode.B;

        [Header("Weapon")]
        [SerializeField] private Transform weaponHolder;
        [Space]
        [SerializeField] private Gun startingGun;
        [SerializeField] private Gun equippedGun;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
            if (startingGun) EquipGun(startingGun);
        }

        private void Update()
        {
            GunInput();
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        private void GunInput()
        {
            if (Input.GetKey(shootKey)) OnTriggerHold();
            if (Input.GetKeyUp(shootKey)) OnTriggerRelease();

            // Check for the 'B' key press to cycle through modes
            if (Input.GetKeyDown(changeFireModeKey)) ChangeFireMode();

            HandleAiming();
        }

        public void EquipGun(Gun gunToEquip)
        {
            equippedGun = Instantiate(gunToEquip, weaponHolder.position, weaponHolder.rotation);
            equippedGun.transform.parent = weaponHolder.transform;
        }

        private void OnTriggerHold()
        {
            if (equippedGun) equippedGun.OnTriggerHold();
        }

        public void OnTriggerRelease()
        {
            if (equippedGun) equippedGun.OnTriggerRelease();
        }

        private void ChangeFireMode()
        {
            if (equippedGun) equippedGun.CycleFireMode();
        }

        private void HandleAiming()
        {
            if (!equippedGun) return;

            // Check for mouse input to toggle or hold aim key for aiming gun.
            if (holdKeyForAiming)
            {
                if (Input.GetKeyDown(aimKey)) equippedGun.SetAimingStatus(true);
                else if (Input.GetKeyUp(aimKey)) equippedGun.SetAimingStatus(false);
            }
            else
            {
                if (Input.GetKeyDown(aimKey)) equippedGun.ToggleAimingStatus();
            }

            if (equippedGun.IsAiming) crossHair.SetActive(false);
            else crossHair.SetActive(true);
        }

        #endregion

        // ----------------------------------------------------------------------
    }
}
