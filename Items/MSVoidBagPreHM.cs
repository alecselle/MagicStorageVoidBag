using MagicStorage.Items;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorageVoidBag.Items {
    public class MSVoidBagPreHM : MSVoidBag {

        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 34;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.value = Item.sellPrice(gold: 2);
        }
        
        public override bool GetEffectiveRange(out float playerToPylonRange, out int pylonToStorageTileRange) {
            playerToPylonRange = 500 * 16;  //500 tiles
            pylonToStorageTileRange = 50;
            return true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<PortableAccessPreHM>()
                .AddIngredient(ItemID.VoidLens)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
