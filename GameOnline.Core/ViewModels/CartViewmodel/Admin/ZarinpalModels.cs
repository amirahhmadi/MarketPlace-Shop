namespace GameOnline.Core.ViewModels.CartViewmodel.Admin;

public class ZarinpalModels
{
    public class ZarinpalRequestResponse
    {
        public ZarinpalData data { get; set; }
        public List<ZarinpalError> errors { get; set; }
    }

    public class ZarinpalVerifyResponse
    {
        public ZarinpalVerifyData data { get; set; }
        public List<ZarinpalError> errors { get; set; }
    }

    public class ZarinpalData
    {
        public int code { get; set; }
        public string authority { get; set; }
    }

    public class ZarinpalVerifyData
    {
        public int code { get; set; }
        public long ref_id { get; set; }
    }

    public class ZarinpalError
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public string Message { get; set; }
}