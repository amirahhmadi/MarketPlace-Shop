using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Comment_FAQ;
using System;
using System.Collections.Generic;

namespace GameOnline.DataBase.Entities.Users
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? ImgName { get; set; }
        public string? Name { get; set; }

        // رمز عبور هش شده
        public string Password { get; set; }

        public string? CartNumber { get; set; }
        public string? NationalCode { get; set; }
        public byte Type { get; set; }

        // فعال‌سازی حساب
        public string? ActiveCode { get; set; }
        public DateTime? ActiveCodeExpiresAt { get; set; }

        // تلاش‌های ناموفق لاگین
        public int? FailedLoginCount { get; set; }
        public DateTime? LockoutEnd { get; set; }

        // بازیابی رمز عبور
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiresAt { get; set; }

        // ارتباط با جداول دیگر
        public List<Cart> Carts { get; set; }
        public List<Question> Questions { get; set; }
        public List<FAQAnswer> FaqAnswers { get; set; }
    }
}