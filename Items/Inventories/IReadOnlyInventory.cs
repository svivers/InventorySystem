using System;
using System.Collections.Generic;

namespace Core.Items.Inventories
{
    public interface IReadOnlyInventory
    {
        event Action<ItemQuantity> OnItemsAdded;
        event Action<ItemQuantity> OnItemsRemoved;

        int GetItemQuantity(ItemId id);
        IReadOnlyList<ItemQuantity> GetAllItems();
    }
}
