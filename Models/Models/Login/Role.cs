
using System.ComponentModel;

namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Employee")]
        Employee = 2,

        [Description("Shift Leader")]
        ShiftLeader = 3
    }

    public static class UserRoleExtensions
    {
        public static string ToStringValue(this UserRole role)
        {
            return role.ToString();
        }
        public static UserRole ToUserRole(this string role)
        {
            _ = Enum.TryParse(role, out UserRole result);
            return result;
        }

        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute) Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

    }
}