namespace TraitLearner.ExtensionMethods
{
    using Vintagestory.API.Common.Entities;

    public static class EntityExtensions
    {
        public static string GetCharacterClass(this Entity entity)
        {
            return entity.WatchedAttributes.GetString("characterClass");
        }

        public static string[] GetExtraTraitNames(this Entity entity)
        {
            return entity.GetAttribute<string[]>("extraTraits");
        }

        public static T GetAttribute<T>(this Entity entity, string key)
        {
            return (T)entity.WatchedAttributes[key].GetValue();
        }
    }
}
