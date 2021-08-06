namespace TraitLearner.ModSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandSystem2.Attributes;
    using CommandSystem2.Entities.VSCommand;
    using ExtensionMethods;
    using Vintagestory.API.Common;
    using Vintagestory.GameContent;

    [VSCommandGroup("traitlearner")]
    [Description("Traitlearner commands")]
    public class TraitLearnerCommands
    {
        //TODO: patch postfix into getTraitText
        //TODO: make rightclick behaviour for items to add trait
        //TODO: make example recipes for common traits
        //TODO: check if attributes are affected by added traits



        [VSCommand("status")]
        [VSCommandDescription("Get the status of your current Traits (class and learned)")]
        public async Task Status(CommandContext context)
        {
            var playerAttributes = context.ServerPlayer.Entity.WatchedAttributes;
            var charClass = playerAttributes.GetString("characterClass");

            var charSystem = context.ServerAPI.ModLoader.GetModSystem<CharacterSystem>();
            var message = new StringBuilder($"Current Traits ({charClass}) \n");
            message.AppendLine("from class:");
            foreach (var trait in charSystem.characterClassesByCode[charClass].Traits)
            {
                message.AppendLine("- " + trait);
            }
            message.AppendLine("extra:");

            var extraTraits = context.ServerPlayer.Entity.WatchedAttributes.GetStringArray("extraTraits");

            foreach (var extra in extraTraits ?? Enumerable.Empty<string>())
            {
                message.AppendLine("- " + extra);
            }

            context.SendCommandMessage(message.ToString(), EnumChatType.CommandSuccess);
        }

        [VSCommand("add")]
        [VSCommandDescription("Add a trait to the player with the given name (use the current player if no name is specified)")]
        public async Task Add(CommandContext context, string traitName, string playername = null)
        {
            var player = playername == null ? context.ServerPlayer : context.ServerAPI.World.AllPlayers.FirstOrDefault(p => p.PlayerName == playername);
            var charSystem = context.ServerAPI.ModLoader.GetModSystem<CharacterSystem>();
            if (player == null)
            {
                context.SendCommandMessage($"No player found with the name {playername}",EnumChatType.CommandError);
                return;
            }

            var trait = charSystem.traits.FirstOrDefault(t => t.Code == traitName);
            if (trait == null)
            {
                context.SendCommandMessage($"No trait found with the name {traitName}", EnumChatType.CommandError);
                return;
            }
            var attributes = player.Entity.WatchedAttributes;
            var extraTraits = attributes.GetStringArray("extraTraits")?.ToList() ?? new List<string>();
            extraTraits.Add(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.ToArray());
            context.SendCommandMessage($"trait {traitName} added to {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        [VSCommand("remove")]
        [VSCommandDescription("Remove a trait to the player with the given name (use the current player if no name is specified)")]
        public async Task Remove(CommandContext context, string traitName, string playername = null)
        {
            var player = playername == null ? context.ServerPlayer : context.ServerAPI.World.AllPlayers.FirstOrDefault(p => p.PlayerName == playername);
            if (player == null)
            {
                context.SendCommandMessage($"No player found with the name {playername}", EnumChatType.CommandError);
                return;
            }

            var charSystem = context.ServerAPI.ModLoader.GetModSystem<CharacterSystem>();
            var trait = charSystem.traits.FirstOrDefault(t => t.Code == traitName);
            if (trait == null)
            {
                context.SendCommandMessage($"No trait found with the name {traitName}", EnumChatType.CommandError);
                return;
            }
            var attributes = player.Entity.WatchedAttributes;
            var extraTraits = attributes.GetStringArray("extraTraits")?.ToList() ?? new List<string>();
            extraTraits.Remove(trait.Code);
            attributes.SetStringArray("extraTraits", extraTraits.ToArray());
            context.SendCommandMessage($"trait {traitName} removed from {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        [VSCommand("reset")]
        [VSCommandDescription("Resets traits (only class traits remain) of the player with the given name (use the current player if no name is specified)")]
        public async Task Reset(CommandContext context, string playername = null)
        {
            var player = playername == null ? context.ServerPlayer : context.ServerAPI.World.AllPlayers.FirstOrDefault(p => p.PlayerName == playername);
            if (player == null)
            {
                context.SendCommandMessage($"No player found with the name {playername}", EnumChatType.CommandError);
                return;
            }
            var attributes = player.Entity.WatchedAttributes;
            attributes.SetStringArray("extraTraits", new string[0]);
            context.SendCommandMessage($"traits reset for {player.PlayerName}", EnumChatType.CommandSuccess);
        }

        [VSCommand()]
        [VSCommandDescription("all the good stuff")]
        public async Task Test(CommandContext context)
        {
            throw new ArgumentException("asdf");
        }
    }
}
