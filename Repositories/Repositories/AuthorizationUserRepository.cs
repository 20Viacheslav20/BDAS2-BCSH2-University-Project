using BDAS2_BCSH2_University_Project.Helpers;
using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models.Login;
using Oracle.ManagedDataAccess.Client;

namespace BDAS2_BCSH2_University_Project.Repositories
{
    public class AuthorizationUserRepository : IAuthorizationUserRepository
    {
        private readonly OracleConnection _oracleConnection;

        public AuthorizationUserRepository(OracleConnection oracleConnection)
        {
            _oracleConnection = oracleConnection;
        }

        public List<UserRole> Authenticate(LoginModel loginModel)
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

                List<UserRole> roles = GetRolesForUserWithOracleCommand(command, user.Id);

                return roles;
            }
        }

        public List<UserRole> GetRolesForUser(int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                return GetRolesForUserWithOracleCommand(command, userId);
            }
        }

        private List<UserRole> GetRolesForUserWithOracleCommand(OracleCommand command, int userId)
        {
            command.Parameters.Clear();

            command.CommandText = @"SELECT r.IDROLE IDROLE, r.nazev NAZEV
                FROM AUTUZIVATEL_ROLE ar
                JOIN ROLE r on r.IDROLE = ar.ROLE_IDROLE
                WHERE ar.AUTUZIVATELE_IDAUTUZ = :userId";

            command.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

            List<UserRole> roles = new List<UserRole>(); 

            using (OracleDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    roles.Add(CreateRoleFromReader(reader));
                }
                return roles;
            }
        }

        private User GetUserByLoginWithOracleCommand(OracleCommand command, string login)
        {
            command.Parameters.Clear();

            command.CommandText = "SELECT * FROM AUTUZIVATELE WHERE JMENO = :login";

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

        private User CreateUserFromReader(OracleDataReader reader)
        {
            User user = new()
            {
                Id = int.Parse(reader["IDAUTUZ"].ToString()),
                Login = reader["JMENO"].ToString(),
                PasswordHash = reader["HESLOHASH"].ToString(),
                PasswordSalt = reader["HESLOSALT"].ToString()
            };
            return user;
        }

        private UserRole CreateRoleFromReader(OracleDataReader reader)
        {
            return reader["NAZEV"].ToString().ToUserRole();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }
    }
}
