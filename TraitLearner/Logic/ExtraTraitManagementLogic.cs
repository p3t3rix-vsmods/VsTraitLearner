namespace TraitLearner.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using ExtensionMethods;
    using Vintagestory.API.Common;
    using Vintagestory.GameContent;

    public class ExtraTraitManagementLogic
    {
        public CharacterSystem CharacterSystem { get; }

        public ExtraTraitManagementLogic(CharacterSystem characterSystem)
        {
            this.CharacterSystem = characterSystem;
        }

        public void Add(IPlayer player, Trait trait)
        {
            var attributes = player.Entity.WatchedAttributes;
            var extraTraits = player.GetExtraTraitNames()?.ToList() ?? new List<string>();
            extraTraits.Add(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.Distinct().ToArray());
            this.CharacterSystem.ApplyTraitAttributes(player.Entity);
            attributes.MarkAllDirty();
        }

        public void Remove(IPlayer player, Trait trait)
        {
            var attributes = player.Entity.WatchedAttributes;
            var extraTraits = player.GetExtraTraitNames()?.ToList() ?? new List<string>();
            extraTraits.Remove(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.Distinct().ToArray());
            this.CharacterSystem.ApplyTraitAttributes(player.Entity);
            attributes.MarkAllDirty();
        }

        public void Reset(IPlayer player)
        {
            var attributes = player.Entity.WatchedAttributes;
            attributes.SetStringArray("extraTraits", new string[0]);
            this.CharacterSystem.ApplyTraitAttributes(player.Entity);
            attributes.MarkAllDirty();
        }
    }
}
