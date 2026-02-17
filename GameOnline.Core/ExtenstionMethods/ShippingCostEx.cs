namespace GameOnline.Core.ExtenstionMethods;

public static class ShippingCostEx
{
    public static int Cost(this int price)
    {
        if (price >= 2500000)
            return 0;

        return 490000;
    }
}