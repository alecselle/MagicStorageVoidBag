using MagicStorage.Common.Systems;
using MagicStorage.Components;
using MagicStorage.Items;
using MagicStorageVoidBag.Items;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace MagicStorageVoidBag.Hooks {
    internal class GetItemMagicStorageBagHook {
        public static Item Hook(Terraria.On_Player.orig_GetItem orig, Player player, int plr, Item newItem, GetItemSettings settings) {
            Item result = orig(player, plr, newItem, settings);

            if (result != null && result.stack > 0) {
                var i = player.inventory.FirstOrDefault(i =>
                    i.type == ModContent.ItemType<MSVoidBag>() ||
                    i.type == ModContent.ItemType<MSVoidBagHM>() ||
                    i.type == ModContent.ItemType<MSVoidBagPreHM>(), null);

                if (i != null) {
                    var bag = (MSVoidBag)i.ModItem;

                    if (!bag.GetEffectiveRange(out float playerToPylonRange, out int pylonToStorageTileRange)) {
                        return result; // Can't determine range, don't deposit
                    }
                    float dist = Vector2.Distance(player.Center, bag.Location.ToWorldCoordinates(8, 8));
                    if (dist > playerToPylonRange)
                        return result; // Out of range, don't deposit

                    if (bag.Location.X >= 0 && bag.Location.Y >= 0 &&
                        MagicStorage.Items.PortableAccess.PlayerCanBeRemotelyConnectedToStorage(player, bag.Location)) {

                        if (TileEntity.ByPosition.TryGetValue(bag.Location, out TileEntity te) &&
                            te.type == ModContent.TileEntityType<TEStorageHeart>()) {

                            TEStorageHeart heart = (TEStorageHeart)te;

                            int oldStack = result.stack;
                            heart.TryDeposit(result);
                            heart.ResetCompactStage();
                            MagicUI.SetRefresh();

                            if (result.stack != oldStack) {
                                if (newItem.IsACoin) {
                                    SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
                                } else {
                                    SoundEngine.PlaySound(SoundID.Grab, player.position);
                                }

                                if (!settings.NoText) {
                                    PopupText.NewText(PopupTextContext.ItemPickupToVoidContainer, newItem, result.stack, noStack: false, settings.LongText);
                                }

                                AchievementsHelper.NotifyItemPickup(player, result);
                            }

                            if (result.stack == 0)
                                return result;
                        }
                    }
                }
            }

            return result;
        }
    }
}