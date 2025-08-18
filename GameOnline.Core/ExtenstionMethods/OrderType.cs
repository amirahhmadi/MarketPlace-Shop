namespace GameOnline.Core.ExtenstionMethods
{

    public static class OrderType
    {
        ///<summary>
        /// انتخاب کالا - سفارش باز
        ///</summary>
        public const byte Product_selection = 1;

        ///<summary>
        /// کنسل شده
        ///</summary>
        public const byte Canceled = 2;

        ///<summary>
        /// پرداخت شده
        ///</summary>
        public const byte Paid = 3;

        ///<summary>
        /// در صف بررسی
        ///</summary>
        public const byte review_queue = 4;

        ///<summary>
        /// تایید سفارش
        ///</summary>
        public const byte order_confirmation = 5;

        ///<summary>
        /// آماده‌سازی سفارش
        ///</summary>
        public const byte Order_preparation = 6;

        ///<summary>
        /// خروج از مرکز پردازش
        ///</summary>
        public const byte Leaving_the_processing_center = 7;

        ///<summary>
        /// تحویل به اداره پست
        ///</summary>
        public const byte Delivery_to_the_post_office = 8;

        ///<summary>
        /// دریافت در مرکز مبادلات پست
        ///</summary>
        public const byte Postal_exchange_center = 9;

        ///<summary>
        /// تحویل مرسوله به مشتری
        ///</summary>
        public const byte Delivery_to_the_customer = 10;

    }

}
