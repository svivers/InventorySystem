namespace Core.Items.Inventories
{
    public readonly struct InventoryResult
    {
        private readonly int m_requestedQuantity;
        private readonly int m_transferedQuantity;

        public InventoryResult(int requestedQuantity, int transferedQuantity)
        {
            m_requestedQuantity = requestedQuantity;
            m_transferedQuantity = transferedQuantity;
        }

        public int RequestedQuantity => m_requestedQuantity;
        public int TransferedQuantity => m_transferedQuantity;
        public int RejectedQuantity => m_requestedQuantity - m_transferedQuantity;
    }
}