namespace GameOnline.Core.ExtenstionMethods
{
    public static class PriceEx
    {
        public static int? Pricecheck(DateTime? StartDiscount, DateTime? EndDisCount, int? SpecialPrice)
        {
            if (SpecialPrice == null || SpecialPrice <= 0)
                return null;
            else
            {
                if (StartDiscount == null && EndDisCount == null)
                {
                    return SpecialPrice;
                }
                else
                {
                    if (StartDiscount == null && EndDisCount > DateTime.Now)
                        return SpecialPrice;

                    else if (StartDiscount < DateTime.Now && EndDisCount == null)
                        return SpecialPrice;

                    else if (StartDiscount < DateTime.Now && EndDisCount > DateTime.Now)
                        return SpecialPrice;

                }
                return null;
            }

        }
    }
}
