using System;

namespace Core.Items
{
    public readonly struct ItemId : IEquatable<ItemId>
    {
        // maybe later change to something more performant like uint or uuid
        private readonly string m_value;

        public ItemId(string value)
        {
            m_value = value;
        }

        public string Value => m_value;
        public bool IsValid => m_value != null;

        // overrides are needed store a struct in a dictionary without boxing plus a convinience of == != operators
        
        public bool Equals(ItemId other) => m_value == other.m_value;

        public override bool Equals(object obj) => obj is ItemId other && Equals(other);

        public override int GetHashCode() => m_value.GetHashCode();

        public static bool operator ==(ItemId left, ItemId right) => left.Equals(right);

        public static bool operator !=(ItemId left, ItemId right) => !left.Equals(right);
    }
}
