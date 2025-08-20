namespace GameOnline.Core.ExtenstionMethods
{
    public static class PriceEx
    {
        public static int? Pricecheck(DateTime? startDiscount, DateTime? endDiscount, int? specialPrice)
        {
            if (specialPrice == null || specialPrice <= 0)
                return null;

            var now = DateTime.Now;

            // بدون تاریخ شروع و پایان → همیشه تخفیف
            if (startDiscount == null && endDiscount == null)
                return specialPrice;

            // فقط تاریخ پایان دارد و هنوز نگذشته
            if (startDiscount == null && endDiscount > now)
                return specialPrice;

            // فقط تاریخ شروع دارد و شروع شده
            if (startDiscount < now && endDiscount == null)
                return specialPrice;

            // بین بازه شروع و پایان
            if (startDiscount < now && endDiscount > now)
                return specialPrice;

            // هیچکدام برقرار نیست → تخفیف فعال نیست
            return null;
        }

    }
}
