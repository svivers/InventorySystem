using System;
using System.Collections.Generic;

namespace Core.Items.Inventories
{
    public class SimpleInventory : IInventory
    {
        private readonly List<ItemQuantity> m_items;

        public SimpleInventory()
        {
            m_items = new List<ItemQuantity>();
        }

        public event Action<ItemQuantity> OnItemsAdded;
        public event Action<ItemQuantity> OnItemsRemoved;

        public InventoryResult Add(ItemQuantity item)
        {
            int index = FindItemIndex(item.Id);

            if (index != -1)
                m_items[index] = new ItemQuantity(item.Id, m_items[index].Quantity + item.Quantity);
            else
                m_items.Add(item);

            OnItemsAdded?.Invoke(item);
            return new InventoryResult(item.Quantity, item.Quantity);
        }

        public InventoryResult Remove(ItemQuantity item)
        {
            int removedQuantity = item.Quantity;
            int index = FindItemIndex(item.Id);

            if (index == -1)
                return new InventoryResult(item.Quantity, 0);

            if (removedQuantity > m_items[index].Quantity)
                removedQuantity = m_items[index].Quantity;

            m_items[index] = new ItemQuantity(item.Id, m_items[index].Quantity - removedQuantity);

            if (m_items[index].Quantity == 0)
                m_items.RemoveAt(index);

            OnItemsRemoved?.Invoke(new ItemQuantity(item.Id, removedQuantity));
            return new InventoryResult(item.Quantity, removedQuantity);
        }

        public int GetItemQuantity(ItemId id)
        {
            int index = FindItemIndex(id);

            if (index != -1)
                return m_items[index].Quantity;

            return 0;
        }

        public IReadOnlyList<ItemQuantity> GetAllItems()
        {
            return m_items;
        }

        private int FindItemIndex(ItemId id)
        {
            for (int i = 0; i < m_items.Count; i++)
                if (m_items[i].Id == id)
                    return i;

            return -1;
        }
    }
}
