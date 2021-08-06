namespace TraitLearner.ExtensionMethods
{
    using HarmonyLib;
    using Vintagestory.API.Common;
    using Vintagestory.GameContent;

    public static class CharacterSystemExtensions
    {
        public static void ApplyTraitAttributes(this CharacterSystem characterSystem, EntityPlayer eplr)
        {
            var dynMethod = characterSystem.GetType().GetMethod("applyTraitAttributes", AccessTools.all);
            dynMethod?.Invoke(characterSystem, new object[] { eplr });
        }
    }
}
