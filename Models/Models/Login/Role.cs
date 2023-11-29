
namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public enum UserRole
    {
        Admin = 1,
        Employee = 2,
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
    }
}