namespace Core.Items.Inventories
{
    public interface IInventory : IReadOnlyInventory
    {
        InventoryResult Add(ItemQuantity item);
        InventoryResult Remove(ItemQuantity item);
    }
}
