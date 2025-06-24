
using Terraria.ModLoader;
using MagicStorageVoidBag.ILPatches;
using MagicStorageVoidBag.Hooks;

namespace MagicStorageVoidBag {
    public class MagicStorageVoidBag : Mod {
        public static MagicStorageVoidBag Instance => ModContent.GetInstance<MagicStorageVoidBag>();

        // IL Patches
        private PlayerUpdatePatch playerUpdatePatch = new();

        public override void Load() { 
            Terraria.IL_Player.Update += playerUpdatePatch.Patch;

            Terraria.On_Player.GetItem_VoidVault += GetItemVoidVaultHook.Hook;
            Terraria.On_Player.ItemSpaceForCofveve += ItemSpaceForCofveveHook.Hook;
        }

        public override void Unload() {
            Terraria.IL_Player.Update -= playerUpdatePatch.Patch;

            Terraria.On_Player.GetItem_VoidVault -= GetItemVoidVaultHook.Hook;
            Terraria.On_Player.ItemSpaceForCofveve -= ItemSpaceForCofveveHook.Hook;

            base.Unload();
        }
    }
}