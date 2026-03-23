namespace BlogApp.Application.DTOs.PaymentDTOs
{
    #region Request
    public class KhaltiRequestDTO
    {
        public string return_url { get; set; }
        public string website_url { get; set; }
        public int amount { get; set; }
        public string purchase_order_id { get; set; }
        public string purchase_order_name { get; set; }
        public CustomerInfo customer_info { get; set; }
        public List<AmountBreakdown> amount_breakdown { get; set; }
        public List<ProductDetail> product_details { get; set; }
        public string merchant_username { get; set; }
        public string merchant_extra { get; set; }
    }

    public class CustomerInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class AmountBreakdown
    {
        public string label { get; set; }
        public int amount { get; set; }
    }

    public class ProductDetail
    {
        public string identity { get; set; }
        public string name { get; set; }
        public int total_price { get; set; }
        public int quantity { get; set; }
        public int unit_price { get; set; }
    }
    #endregion

    public class KhaltiInitiateResponseDTO
    {
        public string pidx { get; set; }
        public string payment_url { get; set; }
        public DateTime expires_at { get; set; }
        public int expires_in { get; set; }
    }
}
