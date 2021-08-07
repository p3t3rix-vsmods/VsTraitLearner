namespace TraitLearner.ModConfig
{
    using System;
    using Foundation.ModConfig;
    using CommandSystem2.Entities.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public class ModConfig : ModConfigBase, ICommandSystemConfig
    {
        //Foundation config:
        public override string ModCode => Constants.DomainName;

        //Command System config:
        [JsonIgnore]
        public IServiceProvider Services { get; set; } = new ServiceCollection().BuildServiceProvider(true);
        [JsonIgnore]
        public bool IgnoreExtraArguments => false;
    }
}
