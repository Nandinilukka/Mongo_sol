namespace Mango.Web.Models
{
    //used to retrieve a shopping cart for user we have one header and list of cart dtails
    public class CartDTO 
    {
        public CartHeaderDTO CartHeader { get; set; } 
        public IEnumerable<CartDetailsDTO>? CartDetails { get; set; }
    }
}
