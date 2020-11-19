namespace Zoughaibandco.ViewModel
{
    public class CommonResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }

    public enum MenuName
    {
        Collection,
        Jewellery
    }

    public enum GenderType
    {
        men,
        women
    }

    public enum PriceFilter
    {
        hl, //Price High to Low
        lh  //Price Low to High
    }

    public class CountWishListCart
    {
        public int WishListCount { get; set; }
        public int CartCount { get; set; }
    }

    public enum ProductFilter
    {
       none, // No Filter
       category // Filter by category
    }

    public enum PaymentType
    {
        ONLINE = 1,
        COD = 2
    }

    public enum PaymentStatus
    {
        PAID = 1,
        NOTPAID = 2,
        CANCEL = 3
    }

    public enum CheckoutTye
    {
        WISHLIST = 1,
        CART = 2
    }
}
