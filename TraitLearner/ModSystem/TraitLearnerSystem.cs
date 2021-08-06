namespace TraitLearner.ModSystem
{
    using CommandSystem2;
    using Foundation.ModConfig;
    using ModConfig;
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

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.Config = api.LoadOrCreateConfig<ModConfig>(this);
            this.RegisterCommands(api);
        }

        private void RegisterCommands(ICoreServerAPI api)
        {
            var commandSystem = new CommandSystem(this.Config, api);
            commandSystem.RegisterCommands(typeof(TraitLearnerCommands));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {

        }
    }
}
