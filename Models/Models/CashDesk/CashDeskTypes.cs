//using Models.Models.Login;   TODO
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Models.Models.CashDesk
//{
//    public enum CashDeskType
//    {
//        [Description("Self")]
//        Yes = 0,

//        [Description("Not self")]
//        No = 1,

//    }
//    public static class CashDeskTypes
//    {
//        public static string GetEnumDescription(Enum value)
//        {
//            var field = value.GetType().GetField(value.ToString());
//            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
//            return attribute == null ? value.ToString() : attribute.Description;
//        }

//        public static UserRole ToUserRole(this string role)
//        {
//            _ = Enum.TryParse(role, out UserRole result);
//            return result;
//        }
//    }
//}
