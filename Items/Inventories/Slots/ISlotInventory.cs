namespace Core.Items.Inventories.Slots
{
    public interface ISlotInventory : IInventory, IReadOnlySlotInventory
    {
        bool TrySetSize(int slotCount);
        InventoryResult AddToSlot(int slotIndex, ItemQuantity item);
        InventoryResult RemoveFromSlot(int slotIndex, ItemQuantity item);
    }
}
