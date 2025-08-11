namespace GameOnline.Core.ExtenstionMethods
{
    public static class AccountType
    {
        ///<summary>
        /// حساب غیر فعال است
        ///</summary>
        public const byte NotActive = 1;

        ///<summary>
        /// حساب کاربر فعال است
        ///</summary>
        public const byte Active = 2;

        ///<summary>
        /// کاربر بن شده
        ///</summary>
        public const byte Ban = 3;
    }
}
