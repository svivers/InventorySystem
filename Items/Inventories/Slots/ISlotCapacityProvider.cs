namespace Core.Items.Inventories.Slots
{
    public interface ISlotCapacityProvider
    {
        int GetSlotCapacity(ItemId id);
    }
}