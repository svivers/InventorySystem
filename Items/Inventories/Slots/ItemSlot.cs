using System;

namespace Core.Items.Inventories.Slots
{
    internal class ItemSlot : IReadOnlySlot
    {
        private readonly ItemId m_id;
        private readonly bool m_isEmpty;
        private readonly int m_capacity;
        private int m_quantity;

        public ItemSlot(ItemId id, int capacity, int quantity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (quantity < 0 || quantity > capacity)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            m_id = id;
            m_capacity = capacity;
            m_quantity = quantity;
            m_isEmpty = false;
        }

        public ItemSlot(ItemQuantity item, int capacity)
        {
            if (capacity < 0 || capacity < item.Quantity)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            m_id = item.Id;
            m_capacity = capacity;
            m_quantity = item.Quantity;
            m_isEmpty = false;
        }

        private ItemSlot()
        {
            m_id = new ItemId();
            m_isEmpty = true;
            m_capacity = 0;
            m_quantity = 0;
        }

        public ItemQuantity Item => new ItemQuantity(m_id, m_quantity);
        public ItemId ItemId => m_id;
        public bool IsEmpty => m_isEmpty;
        public int Capacity => m_capacity;
        public int RemainingCapacity => m_quantity - m_capacity;
        public int Quantity
        {
            get => m_quantity;

            set
            {
                if (value > m_capacity || value < 0)
                    throw new ArgumentOutOfRangeException($"{nameof(Quantity)} must be positive and cannot exceed {nameof(Capacity)}");

                m_quantity = value;
            }
        }

        public static ItemSlot GetEmpty()
        {
            return new ItemSlot();
        }
    }
}
