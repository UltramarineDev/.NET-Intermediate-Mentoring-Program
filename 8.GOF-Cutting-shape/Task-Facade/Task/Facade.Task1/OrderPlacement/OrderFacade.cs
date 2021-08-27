namespace Facade.Task1.OrderPlacement
{
    public class OrderFacade
    {
        private readonly InvoiceSystem _invoiceSystem;
        private readonly PaymentSystem _paymentSystem;
        private readonly ProductCatalog _productCatalog;
        
        public OrderFacade(InvoiceSystem invoiceSystem, PaymentSystem paymentSystem, ProductCatalog productCatalog)
        {
            _invoiceSystem = invoiceSystem;
            _paymentSystem = paymentSystem;
            _productCatalog = productCatalog;
        }
        
        public void PlaceOrder(string productId, int quantity, string email)
        {
            var product = _productCatalog.GetProductDetails(productId);
            
            MakePayment(product, quantity);
            SendInvoice(product, quantity, email);
        }
        
        private void MakePayment(Product product, int quantity)
        {
            var payment = new Payment
            {
                ProductId = product.Id, 
                ProductName = product.Name,
                Quantity = quantity, 
                TotalPrice = product.Price
            };
            
            _paymentSystem.MakePayment(payment);
        }

        private void SendInvoice(Product product, int quantity, string email)
        {
            var invoice = new Invoice
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = quantity,
                TotalPrice = product.Price,
                CustomerEmail = email
            };

            _invoiceSystem.SendInvoice(invoice);
        }
    }
}
