namespace Core.Items.Inventories.Slots
{
    public interface IReadOnlySlot
    {
        ItemQuantity Item { get; }
        bool IsEmpty { get; }
        int Capacity { get; }
        int RemainingCapacity { get; }
    }
}
