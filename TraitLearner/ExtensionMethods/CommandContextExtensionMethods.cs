namespace TraitLearner.ExtensionMethods
{
    using CommandSystem2.Entities.VSCommand;
    using Vintagestory.API.Common;

    public static class CommandContextExtensionMethods
    {
        public static void SendCommandMessage(this CommandContext context, string message, EnumChatType chatType = EnumChatType.Notification)
        {
            context.ServerPlayer.SendMessage(context.GroupId, message, chatType);
        }
    }
}
