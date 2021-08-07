namespace TraitLearner.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using ExtensionMethods;
    using Vintagestory.API.Common;
    using Vintagestory.API.Common.Entities;
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
            this.Add(player.Entity, trait);
        }

        public void Add(Entity entity, Trait trait)
        {
            var attributes = entity.WatchedAttributes;
            var extraTraits = entity.GetExtraTraitNames()?.ToList() ?? new List<string>();
            extraTraits.Add(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.Distinct().ToArray());
            this.CharacterSystem.ApplyTraitAttributes(entity as EntityPlayer);
            attributes.MarkAllDirty();
        }

        public void Remove(IPlayer player, Trait trait)
        {
            this.Remove(player.Entity, trait);
        }

        public void Remove(Entity entity, Trait trait)
        {
            var attributes = entity.WatchedAttributes;
            var extraTraits = entity.GetExtraTraitNames()?.ToList() ?? new List<string>();
            extraTraits.Remove(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.Distinct().ToArray());
            this.CharacterSystem.ApplyTraitAttributes(entity as EntityPlayer);
            attributes.MarkAllDirty();
        }

        public void Reset(IPlayer player)
        {
            this.Reset(player.Entity);
        }

        public void Reset(Entity entity)
        {
            var attributes = entity.WatchedAttributes;
            attributes.SetStringArray("extraTraits", new string[0]);
            this.CharacterSystem.ApplyTraitAttributes(entity as EntityPlayer);
            attributes.MarkAllDirty();
        }
    }
}
