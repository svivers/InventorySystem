using System;

namespace Core.Items
{
    public readonly struct ItemQuantity
    {
        private readonly ItemId m_id;
        private readonly int m_quantity;

        public ItemQuantity(ItemId id, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            m_id = id;
            m_quantity = quantity;
        }

        public ItemId Id => m_id;
        public int Quantity => m_quantity;
        public bool IsEmpty => !m_id.IsValid;
    }
}
