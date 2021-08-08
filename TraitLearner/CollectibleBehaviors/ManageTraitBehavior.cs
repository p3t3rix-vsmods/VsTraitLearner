namespace TraitLearner.CollectibleBehaviors
{
    using System.Collections.Generic;
    using System.Linq;
    using Logic;
    using ModConfig;
    using Vintagestory.API.Client;
    using Vintagestory.API.Common;
    using Vintagestory.API.Datastructures;
    using Vintagestory.GameContent;

    public class ManageTraitBehavior : CollectibleBehavior
    {

        public List<string> TraitsToAdd { get; set; }
        public List<string> TraitsToRemove { get; set; }
        public float UsageDurationInSeconds { get; set; } = 2.0f;
        public CharacterSystem CharacterSystem { get; set; }
        public ExtraTraitManagementLogic TraitManagementLogic { get; set; }
        public bool ShouldReset { get; set; }

        public ICoreAPI Api { get; set; }


        public ManageTraitBehavior(CollectibleObject collObj) : base(collObj)
        {
        }

        public override void Initialize(JsonObject properties)
        {
            base.Initialize(properties);
            this.UsageDurationInSeconds = properties["usage_duration_in_seconds"].AsFloat(2);
            this.TraitsToAdd = properties["traits_add"]?.AsArray(new string[0]).ToList();
            this.TraitsToRemove = properties["traits_remove"]?.AsArray(new string[0]).ToList();
            this.ShouldReset = properties["reset"]?.AsBool() ?? false;
        }

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            this.Api = api;
            this.CharacterSystem = api.ModLoader.GetModSystem<CharacterSystem>();
            this.TraitManagementLogic = new ExtraTraitManagementLogic(this.CharacterSystem);
        }

        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
        {
            byEntity.AnimManager?.StartAnimation("eat");
            handHandling = EnumHandHandling.PreventDefault;
        }

        public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
        {
            if (secondsUsed > 0.5f && (int)(30 * secondsUsed) % 7 == 1)
            {
                var pos = byEntity.Pos.AheadCopy(0.4f).XYZ;
                pos.X += byEntity.LocalEyePos.X;
                pos.Y += byEntity.LocalEyePos.Y - 0.4f;
                pos.Z += byEntity.LocalEyePos.Z;
                byEntity.World.SpawnCubeParticles(pos, slot.Itemstack, 0.3f, 4, 0.5f, (byEntity as EntityPlayer)?.Player);
            }

            handling = EnumHandling.PreventDefault;
            return secondsUsed < this.UsageDurationInSeconds;
        }

        public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
        {
            if (secondsUsed > this.UsageDurationInSeconds && this.Api.Side == EnumAppSide.Server)
            {
                var traits = this.CharacterSystem.TraitsByCode;
                foreach (var trait in this.TraitsToAdd)
                {
                    if (traits.ContainsKey(trait))
                    {
                        this.TraitManagementLogic.Add(byEntity, traits[trait]);
                    }
                    else
                    {
                        this.Api.Logger.Log(EnumLogType.Warning, "Could not find trait with name {0}", trait);
                    }
                }
                foreach (var trait in this.TraitsToRemove)
                {
                    if (traits.ContainsKey(trait))
                    {
                        this.TraitManagementLogic.Remove(byEntity, traits[trait]);
                    }
                    else
                    {
                        this.Api.Logger.Log(EnumLogType.Warning, "Could not find trait with name {0}", trait);
                    }
                }
                if (this.ShouldReset)
                {
                    this.TraitManagementLogic.Reset(byEntity);
                }

                slot.Itemstack.StackSize--;
                slot.MarkDirty();
                this.Api.World.PlaySoundAt(new AssetLocation("sounds/player/coin2"), byEntity);

            }
        }

        public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
        {
            return new WorldInteraction[]
            {
                new WorldInteraction(){MouseButton = EnumMouseButton.Right, ActionLangCode = $"{Constants.DomainName}:traitbehavior_use"}
            };
        }
    }
}
