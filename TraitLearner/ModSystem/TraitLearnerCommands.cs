namespace TraitLearner.ModSystem
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandSystem2.Attributes;
    using CommandSystem2.Attributes.CheckAttributes;
    using CommandSystem2.Entities.VSCommand;
    using Exceptions;
    using ExtensionMethods;
    using Logic;
    using Vintagestory.API.Common;
    using Vintagestory.GameContent;

    [VSCommandGroup("traitlearner")]
    [Description("Traitlearner commands")]
    public class TraitLearnerCommands
    {
        //TODO: patch postfix into getTraitText
        //TODO: make rightclick behaviour for items to add trait
        //TODO: make example recipes for common traits

        [VSCommand("status")]
        [VSCommandDescription("Get the status of your current Traits (class and learned)")]
        public async Task Status(CommandContext context)
        {
            var charClass = context.ServerPlayer.GetCharacterClass();

            var charSystem = GetCharacterSystem(context);
            var message = new StringBuilder($"Current Traits ({charClass}) \n");
            message.AppendLine("from class:");
            foreach (var trait in charSystem.characterClassesByCode[charClass].Traits)
            {
                message.AppendLine("- " + trait);
            }

            message.AppendLine("extra:");

            var extraTraits = context.ServerPlayer.GetExtraTraits()?.ToList() ?? new List<string>();
            foreach (var extra in extraTraits)
            {
                message.AppendLine("- " + extra);
            }

            context.SendCommandMessage(message.ToString(), EnumChatType.CommandSuccess);
        }

        [VSCommand("add")]
        [VSCommandDescription("Add a trait to the player with the given name (use the current player if no name is specified)")]
        [VSCommandRequirePermission(PermissionCheckMode.Any, "root")]
        public async Task Add(CommandContext context, string traitName, string playerName = null)
        {
            var player = GetPlayer(context, playerName);
            var trait = GetTrait(context, traitName);

            var logic = new ExtraTraitManagementLogic(GetCharacterSystem(context));
            logic.Add(player, trait);

            context.SendCommandMessage($"trait {traitName} added to {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        [VSCommand("remove")]
        [VSCommandDescription("Remove a trait to the player with the given name (use the current player if no name is specified)")]
        [VSCommandRequirePermission(PermissionCheckMode.Any, "root")]
        public async Task Remove(CommandContext context, string traitName, string playerName = null)
        {
            var player = GetPlayer(context, playerName);
            var trait = GetTrait(context, traitName);

            var logic = new ExtraTraitManagementLogic(GetCharacterSystem(context));
            logic.Remove(player, trait);

            context.SendCommandMessage($"trait {traitName} removed from {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        [VSCommand("reset")]
        [VSCommandDescription("Resets traits (only class traits remain) of the player with the given name (use the current player if no name is specified)")]
        [VSCommandRequirePermission(PermissionCheckMode.Any, "root")]
        public async Task Reset(CommandContext context, string playerName = null)
        {
            var player = GetPlayer(context, playerName);

            var logic = new ExtraTraitManagementLogic(GetCharacterSystem(context));
            logic.Reset(player);

            context.SendCommandMessage($"traits reset for {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        private static IPlayer GetPlayer(CommandContext context, string playerName)
        {
            var player = playerName == null ? context.ServerPlayer : context.ServerAPI.World.AllPlayers.FirstOrDefault(p => p.PlayerName == playerName);
            if (player == null)
            {
                context.SendCommandMessage($"No player found with the name {playerName}", EnumChatType.CommandError);
                throw new CommandException();
            }

            return player;
        }

        private static CharacterSystem GetCharacterSystem(CommandContext context)
        {
            return context.ServerAPI.ModLoader.GetModSystem<CharacterSystem>();
        }

        private static Trait GetTrait(CommandContext context, string traitName)
        {
            var charSystem = GetCharacterSystem(context);

            var trait = charSystem.traits.FirstOrDefault(t => t.Code == traitName);
            if (trait == null)
            {
                context.SendCommandMessage($"No trait found with the name {traitName}", EnumChatType.CommandError);
                throw new CommandException();
            }

            return trait;
        }
    }
}
