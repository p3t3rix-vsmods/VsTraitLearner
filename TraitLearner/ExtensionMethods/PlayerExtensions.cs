namespace TraitLearner.ExtensionMethods
{
    using Vintagestory.API.Common;

    public static class PlayerExtensions
    {
        public static string GetCharacterClass(this IPlayer player)
        {
            return player.Entity.WatchedAttributes.GetString("characterClass");
        }

        public static string[] GetExtraTraitNames(this IPlayer player)
        {
            return player.GetAttribute<string[]>("extraTraits");
        }

        public static T GetAttribute<T>(this IPlayer player, string key)
        {
            return (T)player.Entity.WatchedAttributes[key].GetValue();
        }
    }
}
