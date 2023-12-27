using UnityEngine;

namespace ProjectZee
{
    [CreateAssetMenu(fileName = "New Ammo Type", menuName = "Project Zee/Inventory/New Ammo Type", order = 1)]
    public class AmmoDataSO : ScriptableObject
    {
        public AmmoType ammoType;
        public int maxQuantity = 100;
    }
}
