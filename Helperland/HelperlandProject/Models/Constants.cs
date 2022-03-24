using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

namespace HelperlandProject.Models
{
    public class Constants
    {
        public const int CUSTOMER = 1;
        public const int SERVICE_PROVIDER = 2;
        public const int ADMIN=3;

        public const int SERVICE_PENDING = 1;
        public const int SERVICE_ACCEPTED = 2;
        public const int SERVICE_COMPLETED = 3;
        public const int SERVICE_CANCELLED = 4;
        public const int SERVICE_REFUNDED = 5;

        public const int GENDER_MALE = 1;
        public const int GENDER_FEMALE = 2;
        public const int GENDER_OTHER = 3;

        public const int USER_ACTIVE = 1;
        public const int USER_INACTIVE = 2;

        public static string key = "987654321a4e4133bbce2ea2315a1916";
        public static string GetStatus(int? status)
        {
            switch (status)
            {
                case 1: return "New";
                case 2: return "Accepted";
                case 3: return "Completed";
                case 4: return "Cancelled";
                case 5: return "Refunded";
                default: return "none";
            }
        }

        public static List<SelectListItem> timeList = new List<SelectListItem>{
                             new SelectListItem{Text="8:00", Value="08:00:00"},
                             new SelectListItem{Text="8:30", Value="08:30:00"},
                             new SelectListItem{Text="9:00", Value="09:00:00"},
                             new SelectListItem{Text="9:30", Value="09:30:00"},
                             new SelectListItem{Text="10:00",Value="10:00:00"},
                             new SelectListItem{Text="10:30",Value="10:30:00"},
                             new SelectListItem{Text="11:00",Value="11:00:00"},
                             new SelectListItem{Text="11:30",Value="11:30:00"},
                             new SelectListItem{Text="12:00",Value="12:00:00"},
                             new SelectListItem{Text="12:30",Value="12:30:00"},
                             new SelectListItem{Text="13:00",Value="13:00:00"},
                             new SelectListItem{Text="13:30",Value="13:30:00"},
                             new SelectListItem{Text="14:00",Value="14:00:00"},
                             new SelectListItem{Text="14:30",Value="14:30:00"},
                             new SelectListItem{Text="15:00",Value="15:00:00"},
                             new SelectListItem{Text="15:30",Value="15:30:00"},
                             new SelectListItem{Text="16:00",Value="16:00:00"},
                             new SelectListItem{Text="16:30",Value="16:30:00"},
                             new SelectListItem{Text="17:00",Value="17:00:00"},
                             new SelectListItem{Text="17:30",Value="17:30:00"},
                             new SelectListItem{Text="18:00",Value="18:00:00"},
                             new SelectListItem{Text="18:30",Value="18:30:00"}};

        public static List<SelectListItem> hourList = new List<SelectListItem> {
                            new SelectListItem{Text="3.0 Hrs", Value="3.0"},
                            new SelectListItem{Text="3.5 Hrs", Value="3.5"},
                            new SelectListItem{Text="4.0 Hrs", Value="4.0"},
                            new SelectListItem{Text="4.5 Hrs", Value="4.5"},
                            new SelectListItem{Text="5.0 Hrs", Value="5.0"},
                            new SelectListItem{Text="5.5 Hrs", Value="5.5"}};

        public static List<SelectListItem> birthDateList = new List<SelectListItem>{
                             new SelectListItem{Text="01", Value="01"},
                             new SelectListItem{Text="02", Value="02"}, };
        public static List<SelectListItem> birthMonthList = new List<SelectListItem>{
                             new SelectListItem{Text="January", Value="January"},
                             new SelectListItem{Text="February", Value="February"}, };
        public static List<SelectListItem> birthYearList = new List<SelectListItem>{
                             new SelectListItem{Text="2001", Value="2001"},
                             new SelectListItem{Text="2002", Value="2002"}, };
        public static List<SelectListItem> languageList = new List<SelectListItem>{
                             new SelectListItem{Text="English", Value="1"},
                             new SelectListItem{Text="French", Value="2"}, };

        public static List<SelectListItem> nationalityList = new List<SelectListItem>{
                             new SelectListItem{Text="India", Value="1"},
                             new SelectListItem{Text="United Kingdom", Value="2"},
                             new SelectListItem{Text="United States", Value="3"}};

        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}
