namespace TraitLearner.ModSystem
{
    using CollectibleBehaviours;
    using CommandSystem2;
    using Foundation.ModConfig;
    using HarmonyLib;
    using ModConfig;
    using Patches;
    using Vintagestory.API.Common;
    using Vintagestory.API.Server;
    using Vintagestory.API.Client;

    public class TraitLearnerSystem : ModSystem
    {
        public ModConfig Config { get; set; }

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        public override void Start(ICoreAPI api)
        {
            this.Config = api.LoadOrCreateConfig<ModConfig>(this);
            api.RegisterCollectibleBehaviorClass("ManageTrait", typeof(ManageTraitBehaviour));
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.RegisterCommands(api);
        }

        private void RegisterCommands(ICoreServerAPI api)
        {
            var commandSystem = new CommandSystem(this.Config, api);
            commandSystem.RegisterCommands(typeof(TraitLearnerCommands));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            var harmony = new Harmony(nameof(TraitLearnerSystem));
            ComposeTraitsTabPatch.Initialize(harmony, api, this.Config);
        }
    }
}
