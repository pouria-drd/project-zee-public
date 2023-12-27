using System;
using UnityEngine;

namespace ProjectZee
{
    [Serializable]
    public class AmmoSlot
    {
        public AmmoType ammoType;
        public int quantity;
    }

    public class PlayerInventory : MonoBehaviour
    {
        public AmmoSlot[] ammoSlots;

        public void AddAmmo(AmmoType ammoType, int quantity)
        {
            foreach (var slot in ammoSlots)
            {
                if (slot.ammoType == ammoType)
                {
                    slot.quantity = Mathf.Clamp(slot.quantity + quantity, 0, int.MaxValue);
                    // You can add additional logic or events here if needed
                    break;
                }
            }
        }

        public void ConsumeAmmo(AmmoType ammoType, int quantity)
        {
            foreach (var slot in ammoSlots)
            {
                if (slot.ammoType == ammoType)
                {
                    slot.quantity = Mathf.Clamp(slot.quantity - quantity, 0, int.MaxValue);
                    // You can add additional logic or events here if needed
                    break;
                }
            }
        }
    }
}
