using Models.Models;
using Models.Models.Login;
using Oracle.ManagedDataAccess.Client;
using Repositories.Helpers;
using Repositories.IRepositories;
using System.Data;

namespace Repositories.Repositories
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

        public SimulatedUser Simulate(int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                AutorisedUser user = GetAutorisedUser(userId);

                if (user == null)
                    throw new Exception("This user doesn't exist");

                List<UserRole> roles = GetRolesForUserWithOracleCommand(command, user.Id);

                if (roles.Contains(UserRole.Admin))
                    throw new Exception("You can't  simulate the admin of program");

                SimulatedUser simulatedUser = new()
                {
                    Login = user.Login,
                    Roles = roles
                };

                return simulatedUser;
            }
        }

        public List<UserRole> GetRolesForUser(int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
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

        public void Register(AutorisedUser registrateModel)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                string passwordSalt = PasswordHasher.GenerateSalt();
                string passwordHash = PasswordHasher.ComputeHash(registrateModel.Password, passwordSalt);

                command.CommandText = @"INSERT INTO AUTUZIVATELE(JMENO, HESLOHASH, HESLOSALT, ZAMESTNANCI_IDZAMESTNANCE) 
                                        VALUES (:login, :hesloHash, :hesloSalt, :idEmployee)";

                command.Parameters.Clear();
                command.Parameters.Add("login", OracleDbType.Varchar2).Value = registrateModel.Login;
                command.Parameters.Add("hesloHash", OracleDbType.Varchar2).Value = passwordHash;
                command.Parameters.Add("hesloSalt", OracleDbType.Varchar2).Value = passwordSalt;
                command.Parameters.Add("idEmployee", OracleDbType.Varchar2).Value = registrateModel.EmployeeId;

                command.ExecuteNonQuery();

            }
        }

        public List<AutorisedUser> GetAutorisedUsers()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @$"SELECT au.idautuz IDAUTUZ, au.jmeno LOGIN, 
                                        z.idzamestnance IDZAMESTNANCE, z.jmeno JMENO, z.prijmeni PRIJMENI 
                                        FROM AUTUZIVATELE au
                                        JOIN ZAMESTNANCI z on z.IDZAMESTNANCE = au.ZAMESTNANCI_IDZAMESTNANCE";

                List<AutorisedUser> autorisedUsers = new List<AutorisedUser>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        autorisedUsers.Add(CreateAutorisedUserFromReader(reader));
                    }
                    return autorisedUsers;
                }
            }
        }

        public AutorisedUser GetAutorisedUser(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT au.idautuz IDAUTUZ, au.jmeno LOGIN, 
                                        z.idzamestnance IDZAMESTNANCE, z.jmeno JMENO, z.prijmeni PRIJMENI 
                                        FROM AUTUZIVATELE au
                                        JOIN ZAMESTNANCI z on z.IDZAMESTNANCE = au.ZAMESTNANCI_IDZAMESTNANCE WHERE IDAUTUZ = :autorisedUserId";

                command.Parameters.Add("autorisedUserId", OracleDbType.Int32).Value = id;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    AutorisedUser autorisedUser = CreateAutorisedUserFromReader(reader);
                    autorisedUser.Roles = GetRolesForUser(id);
                    return autorisedUser;
                }
            }
        }

        public void Edit(AutorisedUser autorisedUser)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();
                AutorisedUser dbautorisedUser = GetAutorisedUser(autorisedUser.Id);

                if (dbautorisedUser == null)
                    return;

                command.Parameters.Clear();

                string query = "";

                if (dbautorisedUser.Login != autorisedUser.Login)
                {
                    query += "JMENO = :autorisedUserLogin, ";
                    command.Parameters.Add("autorisedUserLogin", OracleDbType.Varchar2).Value = autorisedUser.Login;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.TrimEnd(',', ' ');

                    command.CommandText = $"UPDATE AUTUZIVATELE SET {query} WHERE IDAUTUZ = :autorisedUserId";
                    command.Parameters.Add("autorisedUserId", OracleDbType.Int32).Value = autorisedUser.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "smazat_autuzivatele";

                command.Parameters.Add("p_IDAUTUZ", OracleDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        public void SaveImageForUser(Image image, int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                Image dbImage = GetUserImage(userId);

                if (dbImage == null)
                {
                    command.CommandText = $"INSERT INTO SOUBORY (jmeno, soubor, pripona, AUTUZIVATELE_IDAUTUZ) " +
                                  "VALUES (:imageName, :imageData, :imageExtension, :autorisedUserId)";

                }
                else
                {
                    command.CommandText = @"UPDATE SOUBORY SET jmeno = :imageName, soubor = :imageData, 
                                           pripona = :imageExtension
                                           WHERE autuzivatele_idautuz = :autorisedUserId";

                }
                command.Parameters.Clear();

                command.Parameters.Add("imageName", OracleDbType.Varchar2).Value = image.Name;
                command.Parameters.Add("imageData", OracleDbType.Blob).Value = image.Data;
                command.Parameters.Add("imageExtension", OracleDbType.Varchar2).Value = image.Extension;
                command.Parameters.Add("autorisedUserId", OracleDbType.Decimal).Value = userId;

                command.ExecuteNonQuery();

            }
        }

        public Image GetUserImage(int userId)
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                if (_oracleConnection.State == ConnectionState.Closed)
                    _oracleConnection.Open();

                command.CommandText = @$"SELECT idsouboru, jmeno NAZEV, soubor, pripona 
                                        FROM SOUBORY 
                                        WHERE autuzivatele_idautuz = :userId";

                command.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return CreateIamgeFromReader(reader);
                }
            }
        }

        public List<Image> GetAllImages()
        {
            using (OracleCommand command = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();

                command.CommandText = @"SELECT idsouboru, jmeno NAZEV, soubor, pripona 
                                        FROM SOUBORY";

                List<Image> images = new List<Image>();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        images.Add(CreateIamgeFromReader(reader));
                    }
                    return images;
                }
            }
        }

        private AutorisedUser CreateAutorisedUserFromReader(OracleDataReader reader)
        {
            AutorisedUser autorisedUser = new()
            {
                Id = int.Parse(reader["IDAUTUZ"].ToString()),
                Login = reader["LOGIN"].ToString(),
                Employee = new()
                {
                    Id = int.Parse(reader["IDZAMESTNANCE"].ToString()),
                    Name = reader["JMENO"].ToString(),
                    Surname = reader["PRIJMENI"].ToString()
                }
            };
            return autorisedUser;
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

        private Image CreateIamgeFromReader(OracleDataReader reader)
        {
            Image image = new()
            {
                Id = int.Parse(reader["idsouboru"].ToString()),
                Name = reader["NAZEV"].ToString(),
                Extension = reader["pripona"].ToString(),
                Data = (byte[])reader["soubor"]
            };
            return image;
        }
    }

}



