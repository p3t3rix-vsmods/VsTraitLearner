namespace TraitLearner.Patches
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using ExtensionMethods;
    using HarmonyLib;
    using ModConfig;
    using Vintagestory.API.Client;
    using Vintagestory.API.Common;
    using Vintagestory.API.Config;
    using Vintagestory.GameContent;

    /// <summary>
    /// Patch to show extra Traits in the character sheet
    /// </summary>
    public class ComposeTraitsTabPatch
    {
        private static ILogger Logger { get; set; }
        private static ICoreClientAPI Api { get; set; }
        private static ModConfig Config { get; set; }

        public static void Initialize(Harmony harmony, ICoreClientAPI api, ModConfig config)
        {
            Logger = api.Logger;
            Api = api;
            Config = config;
            var prefix = typeof(ComposeTraitsTabPatch).GetMethod(nameof(PrefixComposeTraitsTab));
            var original = typeof(CharacterSystem).GetMethod("composeTraitsTab", AccessTools.all);
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        public static bool PrefixComposeTraitsTab(CharacterSystem __instance, GuiComposer compo)
        {
            //TODO: put in scrollable container
            compo.AddRichtext(__instance.GetClassTraitText(), CairoFont.WhiteDetailText().WithLineHeightMultiplier(1.15), ElementBounds.Fixed(0, 25, 385, 150));
            compo.AddRichtext(GetExtraTraitsText(__instance), CairoFont.WhiteDetailText().WithLineHeightMultiplier(1.15), ElementBounds.Fixed(0, 175, 385, 150));

            return false;
        }

        public static string GetExtraTraitsText(CharacterSystem characterSystem)
        {
            var sb = new StringBuilder();
            var extraTraitNames = Api.World.Player.GetExtraTraitNames();

            var traitDic = characterSystem.TraitsByCode;

            if (extraTraitNames.Any())
            {
                sb.AppendLine($"{Lang.Get($"{Config.ModCode}:extra_trait_title")}:");
                foreach (var extraTraitName in extraTraitNames)
                {
                    var trait = traitDic[extraTraitName];
                    var translatedAttributes = trait.Attributes.Select(a => Lang.Get(string.Format(CultureInfo.InvariantCulture, "charattribute-{0}-{1:}", a.Key, a.Value))).ToList();
                    var attrText = translatedAttributes.Any() ? $"{string.Join(",", translatedAttributes)}" : null;
                    var traitName = Lang.Get("trait-" + trait.Code);
                    var traitDescr = Lang.GetIfExists("traitdesc-" + trait.Code);

                    sb.AppendLine(attrText != null || traitDescr != null ? Lang.Get("traitwithattributes", traitName, attrText ?? traitDescr) : traitName);
                }
            }

            return sb.ToString();
        }
    }
}
