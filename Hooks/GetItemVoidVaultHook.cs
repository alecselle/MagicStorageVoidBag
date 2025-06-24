using MagicStorage;
using MagicStorage.Components;
using MagicStorage.Common.Systems;
using MagicStorageVoidBag.Items;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorageVoidBag.Hooks {
    internal class GetItemVoidVaultHook {
        private static readonly log4net.ILog Logger = MagicStorageVoidBag.Instance.Logger;
        public static bool Hook(Terraria.On_Player.orig_GetItem_VoidVault orig, Player player, int plr, Item[] inventory, Item newItem, GetItemSettings settings, Item returnItem) {
            var i = player.inventory.FirstOrDefault(i => i.type == ModContent.ItemType<MSVoidBag>(), null);

            newItem = newItem.Clone();

            if (i == null) goto original;

            var bag = (MSVoidBag)i.ModItem;

            if (bag.Location.X < 0 || bag.Location.Y < 0) goto original;

            Tile tile = Main.tile[bag.Location.X, bag.Location.Y];

            if (!tile.HasTile || tile.TileType != ModContent.TileType<StorageHeart>() || tile.TileFrameX != 0 || tile.TileFrameY != 0) goto original;
            if (!TileEntity.ByPosition.TryGetValue(bag.Location, out TileEntity te)) goto original;
            if (te.type != ModContent.TileEntityType<TEStorageHeart>()) goto original;

            TEStorageHeart heart = (TEStorageHeart)te;

            heart.TryDeposit(returnItem);
            heart.ResetCompactStage();
            MagicUI.SetRefresh();

            if (returnItem.stack != newItem.stack) {
                if (newItem.IsACoin) {
                    SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
                } else {
                    SoundEngine.PlaySound(SoundID.Grab, player.position);
                }

                if (!settings.NoText) {
                    PopupText.NewText(PopupTextContext.ItemPickupToVoidContainer, newItem, returnItem.stack, noStack: false, settings.LongText);
                }

                AchievementsHelper.NotifyItemPickup(player, returnItem);
            }

            if (returnItem.stack == 0) return true;

            original:
            if (player.HasItem(ItemID.VoidLens)) {
                return orig(player, plr, inventory, newItem, settings, returnItem);
            } else {
                return false;
            }
        }
    }
}
