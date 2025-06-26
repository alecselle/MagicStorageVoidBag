using MagicStorage.Items;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorageVoidBag.Items {
    public class MSVoidBagHM : MSVoidBag {

        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 34;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.value = Item.sellPrice(gold: 3, silver: 50);
        }
        
        public override bool GetEffectiveRange(out float playerToPylonRange, out int pylonToStorageTileRange) {
            playerToPylonRange = 1500 * 16;  //1500 tiles
            pylonToStorageTileRange = 100;
            return true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<PortableAccessHM>()
                .AddIngredient(ItemID.VoidLens)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient<MSVoidBagPreHM>()
                .AddIngredient(ItemID.Pearlwood, 20)
                .AddRecipeGroup("MagicStorage:AnyMythrilBar", 15)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
