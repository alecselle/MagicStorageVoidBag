using MagicStorage;
using MagicStorage.Components;
using MagicStorageVoidBag.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;

namespace MagicStorageVoidBag {
    internal static class Utility {
        internal static bool HeartHasSpaceFor(Item newItem, TEStorageHeart heart) {
            foreach (TEAbstractStorageUnit storageUnit in heart.GetStorageUnits()) {
                if (!storageUnit.Inactive) {
                    var unitItems = storageUnit.GetItems();

                    if (!storageUnit.IsFull) return true;
                    if (storageUnit.HasSpaceInStackFor(newItem)) return true;
                }
            }

            return false;
        }

        public static bool IsWithinBagRange(Player player, MSVoidBag bag, out float dist, out float maxRange) {
            if (!bag.GetEffectiveRange(out maxRange, out _)) {
                dist = float.MaxValue;
                return false;
            }
            dist = Vector2.Distance(player.Center, bag.Location.ToWorldCoordinates(8, 8));
            return dist <= maxRange;
        }
    }
}
