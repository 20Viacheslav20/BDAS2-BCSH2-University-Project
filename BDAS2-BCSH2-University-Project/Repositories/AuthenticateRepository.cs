using BDAS2_BCSH2_University_Project.Helpers;
using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models.Login;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly OracleConnection _oracleConnection;

        public AuthenticateRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<Role> Authenticate(LoginModel loginModel)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                User user = GetUserByLoginWithOracleCommand(command, loginModel.Login);

                if (user == null)
                    throw new Exception("This user doesn't exist");

                string loginUserPasswordHash = PasswordHasher.ComputeHash(loginModel.Password, user.PasswordSalt);
                if (string.IsNullOrWhiteSpace(loginUserPasswordHash) || loginUserPasswordHash != user.PasswordHash) // login false
                    throw new Exception("Login failed");

                List<Role> roles = new List<Role>();
                roles = GetRolesForUserWithOracleCommand(command, user.Id);

                return roles;
            }
        }

        public List<Role> GetRolesForUser(int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                return GetRolesForUserWithOracleCommand(command, userId);
            }
        }

        private List<Role> GetRolesForUserWithOracleCommand(OracleCommand command, int userId)
        {
            command.Parameters.Clear();
            // TODO write normal names of fields and tables
            command.CommandText = "SELECT ur.roleid RoleID, r.nazev NAZEV " +
                "FROM UserRoles ur " +
                "JOIN ROLE r on r.roleid = ur.roleid " +
                "WHERE UserID = :userId";

            command.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

            List<Role> roles = new List<Role>(); 

            using (OracleDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    roles.Add(CreateRoleFromREader(reader));
                }
                return roles;
            }
        }

        private User GetUserByLoginWithOracleCommand(OracleCommand command, string login)
        {
            command.Parameters.Clear();
            // TODO write normal names of fields and tables
            command.CommandText = "select * from LogedUser where Username = :login";

            command.Parameters.Add("login", OracleDbType.Varchar2).Value = login;

            using (OracleDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                return CreateUserFromReader(reader);
            }
        }


        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        // TODO write normal names of fields
        private User CreateUserFromReader(OracleDataReader reader)
        {
            User user = new()
            {
                Id = int.Parse(reader["UserID"].ToString()),
                Login = reader["Username"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                PasswordSalt = reader["PasswordSalt"].ToString()
            };
            return user;
        }

        // TODO write normal names of fields
        private Role CreateRoleFromREader(OracleDataReader reader)
        {
            Role role = new()
            {
                Id = int.Parse(reader["RoleID"].ToString()),
                Name = reader["NAZEV"].ToString(),
            };
            return role;
        }

    }
}
