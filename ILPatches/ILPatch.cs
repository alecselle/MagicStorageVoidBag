using MonoMod.Cil;

namespace MagicStorageVoidBag.ILPatches {
    internal interface ILPatch {
        public abstract void Patch(ILContext il);
    }
}
