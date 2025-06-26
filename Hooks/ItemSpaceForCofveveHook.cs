using MagicStorage.Components;
using MagicStorage.Items;
using MagicStorageVoidBag.Items;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MagicStorageVoidBag.Hooks {
    internal class ItemSpaceForCofveveHook {
        private static readonly log4net.ILog Logger = MagicStorageVoidBag.Instance.Logger;

        public static bool Hook(Terraria.On_Player.orig_ItemSpaceForCofveve orig, Player player, Item newItem) {
            var i = player.inventory.FirstOrDefault(i =>
                i.type == ModContent.ItemType<MSVoidBag>() ||
                i.type == ModContent.ItemType<MSVoidBagHM>() ||
                i.type == ModContent.ItemType<MSVoidBagPreHM>(), null);
            if (i == null) goto original;

            newItem = newItem.Clone();

            var bag = (MSVoidBag)i.ModItem;

            if (!bag.GetEffectiveRange(out float playerToPylonRange, out int pylonToStorageTileRange)) goto original;
            float dist = Vector2.Distance(player.Center, bag.Location.ToWorldCoordinates(8, 8));
            if (dist > playerToPylonRange) goto original;

            if (!PortableAccess.PlayerCanBeRemotelyConnectedToStorage(player, bag.Location)) goto original;

            if (bag.Location.X < 0 || bag.Location.Y < 0) goto original;

            Tile tile = Main.tile[bag.Location.X, bag.Location.Y];

            if (!tile.HasTile || tile.TileType != ModContent.TileType<MagicStorage.Components.StorageHeart>() || tile.TileFrameX != 0 || tile.TileFrameY != 0) goto original;
            if (!TileEntity.ByPosition.TryGetValue(bag.Location, out TileEntity te)) goto original;
            if (te.type != ModContent.TileEntityType<TEStorageHeart>()) goto original;

            if (Utility.HeartHasSpaceFor(newItem, (TEStorageHeart)te)) return true;

            original:
            if (player.HasItem(ItemID.VoidLens)) {
                return orig(player, newItem);
            } else {
                return false;
            }
        }
    }
}
