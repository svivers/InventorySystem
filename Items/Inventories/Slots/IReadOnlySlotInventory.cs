using System;
using System.Collections.Generic;

namespace Core.Items.Inventories.Slots
{
    public interface IReadOnlySlotInventory : IReadOnlyInventory
    {
        int SlotCount { get; }

        event Action<int> OnSlotAdded;
        event Action<int> OnSlotRemove;
        event Action<int> OnSlotChanged;

        IReadOnlySlot GetSlot(int slotIndex);
        IReadOnlyList<IReadOnlySlot> GetAllSlots();
        int GetEmptySlotCount();
    }
}
