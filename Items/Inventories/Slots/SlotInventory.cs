using System;
using System.Collections.Generic;

namespace Core.Items.Inventories.Slots
{
    public class SlotInventory : ISlotInventory, IReadOnlySlotInventory
    {
        private readonly List<ItemSlot> m_slots;
        private readonly ISlotCapacityProvider m_capacityProvider;

        public SlotInventory(ISlotCapacityProvider capacityProvider, int slotCount)
        {
            if (capacityProvider == null)
                throw new ArgumentNullException(nameof(capacityProvider));

            if (slotCount < 0)
                throw new ArgumentOutOfRangeException(nameof(slotCount));

            m_capacityProvider = capacityProvider;
            m_slots = new List<ItemSlot>(slotCount);

            for (int i = 0; i < slotCount; i++)
                m_slots.Add(ItemSlot.GetEmpty());
        }

        public int SlotCount => m_slots.Count;
        
        public event Action<int> OnSlotAdded;
        public event Action<int> OnSlotRemove;
        public event Action<int> OnSlotChanged;
        public event Action<ItemQuantity> OnItemsAdded;
        public event Action<ItemQuantity> OnItemsRemoved;

        public InventoryResult Add(ItemQuantity item)
        {
            int quantityToAdd = item.Quantity;

            for (int i = 0; i < m_slots.Count; i++)
            {
                if (quantityToAdd == 0)
                    break;

                if (m_slots[i].ItemId != item.Id)
                    continue;

                quantityToAdd -= AddToSlot(i, quantityToAdd);
            }

            for (int i = 0; i < m_slots.Count; i++)
            {
                if (quantityToAdd == 0)
                    break;

                if (!m_slots[i].IsEmpty)
                    continue;

                m_slots[i] = CreateSlotForItem(item.Id);
                quantityToAdd -= AddToSlot(i, quantityToAdd);
            }

            OnItemsAdded?.Invoke(new ItemQuantity(item.Id, item.Quantity - quantityToAdd));
            return new InventoryResult(item.Quantity, item.Quantity - quantityToAdd);
        }

        public InventoryResult AddToSlot(int slotIndex, ItemQuantity item)
        {
            if (slotIndex < 0 || slotIndex > m_slots.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(slotIndex));

            if (m_slots[slotIndex].IsEmpty)
                m_slots[slotIndex] = CreateSlotForItem(item.Id);
            else if (m_slots[slotIndex].ItemId != item.Id)
                throw new InvalidOperationException($"Adding to slot: Item id '{item.Id}' does not match the id in slot");

            int addedQuantity = AddToSlot(slotIndex, item.Quantity);
            OnItemsAdded?.Invoke(new ItemQuantity(item.Id, addedQuantity));
            return new InventoryResult(item.Quantity, addedQuantity);
        }

        public IReadOnlyList<IReadOnlySlot> GetAllSlots()
        {
            return m_slots;
        }

        public int GetItemQuantity(ItemId id)
        {
            int quantity = 0;

            foreach (var slot in m_slots)
                if (slot.ItemId == id)
                    quantity += slot.Quantity;

            return quantity;
        }

        public IReadOnlySlot GetSlot(int slotIndex)
        {
            return m_slots[slotIndex];
        }

        public int GetEmptySlotCount()
        {
            int count = 0;

            foreach (var slot in m_slots)
                if (slot.IsEmpty)
                    count++;

            return count;
        }

        public IReadOnlyList<ItemQuantity> GetAllItems()
        {
            ItemQuantity[] items = new ItemQuantity[m_slots.Count];

            for (int i = 0; i < m_slots.Count; i++)
                items[i] = m_slots[i].Item;

            return items;
        }

        public InventoryResult Remove(ItemQuantity item)
        {
            int quantityToRemove = item.Quantity;

            for (int i = 0; i < m_slots.Count; i++)
            {
                if (quantityToRemove == 0)
                    break;

                if (m_slots[i].ItemId != item.Id)
                    continue;

                quantityToRemove -= RemoveFromSlot(i, quantityToRemove);
            }

            OnItemsRemoved?.Invoke(new ItemQuantity(item.Id, item.Quantity - quantityToRemove));
            return new InventoryResult(item.Quantity, item.Quantity - quantityToRemove);
        }

        public InventoryResult RemoveFromSlot(int slotIndex, ItemQuantity item)
        {
            if (slotIndex < 0 || slotIndex > m_slots.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(slotIndex));

            if (m_slots[slotIndex].IsEmpty)
                return new InventoryResult(item.Quantity, 0);
            else if (m_slots[slotIndex].ItemId != item.Id)
                throw new InvalidOperationException($"Removing from slot: Item id '{item.Id}' does not match the id in slot");

            int quantityRemoved = RemoveFromSlot(slotIndex, item.Quantity);
            OnItemsRemoved?.Invoke(new ItemQuantity(item.Id, quantityRemoved));
            return new InventoryResult(item.Quantity, quantityRemoved);
        }

        public bool TrySetSize(int slotCount)
        {
            if (slotCount > m_slots.Count)
            {
                for (int i = m_slots.Count; i < slotCount; i++)
                    m_slots.Add(ItemSlot.GetEmpty());

                return true;
            }

            if (slotCount < (m_slots.Count - GetEmptySlotCount()))
                return false;
            
            for (int i = 0; i < m_slots.Count; i++)
            {
                if (!m_slots[i].IsEmpty)
                    continue;

                OnSlotRemove?.Invoke(i);
                m_slots.RemoveAt(i);
            }

            return true;
        }

        private int AddToSlot(int slotIndex, int quantity)
        {
            int addedQuantity = quantity;

            if (addedQuantity > m_slots[slotIndex].RemainingCapacity)
                addedQuantity = m_slots[slotIndex].RemainingCapacity;

            m_slots[slotIndex].Quantity += addedQuantity;
            OnSlotChanged?.Invoke(slotIndex);
            return addedQuantity;
        }

        private int RemoveFromSlot(int slotIndex, int quantity)
        {
            int removedQuantity = quantity;

            if (removedQuantity > m_slots[slotIndex].Quantity)
                removedQuantity = m_slots[slotIndex].Quantity;

            m_slots[slotIndex].Quantity -= removedQuantity;
            OnSlotChanged?.Invoke(slotIndex);
            return removedQuantity;
        }

        private ItemSlot CreateSlotForItem(ItemId id)
        {
            return new ItemSlot(id, m_capacityProvider.GetSlotCapacity(id), 0);
        }
    }
}
