using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MagicStorageVoidBag.Items;

namespace MagicStorageVoidBag.ILPatches {
    internal class PlayerUpdatePatch : ILPatch {
        public void Patch(ILContext il) {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(ItemID.VoidLens)))
                return;
            c.Index--;

            c.RemoveRange(3);

            var hasItemMethod = typeof(Player).GetMethod(nameof(Player.HasItem), new[] { typeof(int) });

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldc_I4, ModContent.ItemType<MSVoidBag>());
            c.Emit(OpCodes.Call, hasItemMethod);

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldc_I4, ModContent.ItemType<MSVoidBagHM>());
            c.Emit(OpCodes.Call, hasItemMethod);
            c.Emit(OpCodes.Or);

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldc_I4, ModContent.ItemType<MSVoidBagPreHM>());
            c.Emit(OpCodes.Call, hasItemMethod);
            c.Emit(OpCodes.Or);

            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldc_I4, ItemID.VoidLens);
            c.Emit(OpCodes.Call, hasItemMethod);
            c.Emit(OpCodes.Or);
        }
    }
}
