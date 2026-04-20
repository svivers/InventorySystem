using System;

namespace Core.Items.Recipes
{
    public readonly struct RecipeId : IEquatable<RecipeId>
    {
        private readonly string m_value;

        public RecipeId(string value)
        {
            m_value = value;
        }

        public string Value => m_value;
        public bool IsValid => m_value != null;

        public bool Equals(RecipeId other) => m_value == other.m_value;

        public override bool Equals(object obj) => obj is RecipeId other && Equals(other);

        public override int GetHashCode() => m_value.GetHashCode();

        public static bool operator ==(RecipeId left, RecipeId right) => left.Equals(right);

        public static bool operator !=(RecipeId left, RecipeId right) => !left.Equals(right);
    }
}
