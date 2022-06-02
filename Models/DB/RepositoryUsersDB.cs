using CarPlanet.Models;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace FirstWebApp.Models.DB
{
    //Diese Klasse implementiert unser Interface
    public class RepositoryUsersDB : IRepositoryUsers
    {
        private IHttpContextAccessor _httpContextAccessor;

        public RepositoryUsersDB(IHttpContextAccessor _httpContextAccessor) {
            this._httpContextAccessor = _httpContextAccessor;
        }

        //Verbindungszeichenkette: enthält Server Ip, Datendbankname, User + Passwort
        // DB - Server
        private string _connectionString = "Server=localhost;database=carplanet;user=root;password=";
        //über diese verbindung wird mit dem sever komuniziert
        private DbConnection _conn;
        public async Task DisconnectAsync()
        {
            //fals die Verbindung existiert und geöffnet ist
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                await this._conn.CloseAsync();
            }
        }
        public async Task ConnectAsync()
        {
            if (this._conn == null)
            {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open)
            {
                //await wartet bis die Methode fertig ausgeführt wurde
                await this._conn.OpenAsync();
            }
        }
        public async Task<bool> ChangeUserDataAsync(int userId, User newUserData)
        {
            if (this._conn?.State == System.Data.ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "update users set password = sha2(@password, 512), " +
                    "email = @email, birthdate = @birthdate, gender = @gender where user_id = @user_id";
                

                DbParameter paramPW = cmd.CreateParameter();
                paramPW.ParameterName = "password";
                paramPW.DbType = System.Data.DbType.String;
                paramPW.Value = newUserData.Passwort;

                DbParameter paramEmail = cmd.CreateParameter();
                paramEmail.ParameterName = "email";
                paramEmail.DbType = System.Data.DbType.String;
                paramEmail.Value = newUserData.Email;

                DbParameter paramBD = cmd.CreateParameter();
                paramBD.ParameterName = "birthdate";
                paramBD.DbType = System.Data.DbType.Date;
                paramBD.Value = newUserData.Birthdate;

                DbParameter paramGender = cmd.CreateParameter();
                paramGender.ParameterName = "gender";
                paramGender.DbType = System.Data.DbType.Int32;
                paramGender.Value = newUserData.Gender;

                DbParameter paramID = cmd.CreateParameter();
                paramGender.ParameterName = "user_id";
                paramGender.DbType = System.Data.DbType.Int32;
                paramGender.Value = newUserData.UserID;


                
                cmd.Parameters.Add(paramPW);
                cmd.Parameters.Add(paramEmail);
                cmd.Parameters.Add(paramBD);
                cmd.Parameters.Add(paramGender);
                cmd.Parameters.Add(paramID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }



        public async Task<bool> DeleteAsync(int userId)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "delete from users where user_id = @userID";
                //Parameter @username befüllen
                //leeres Parameter Object erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramUN.ParameterName = "userID";
                paramUN.DbType = DbType.Int32;
                paramUN.Value = userId;

               

                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramUN);
                

                //nun senden wir das Command an den server
                return await cmdInsert.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }

        

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            if(this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdAllUsers = this._conn.CreateCommand();
                cmdAllUsers.CommandText = "select * from users";

                //Wir bekommen nun eine komplette tabelle zurück
                //diese wird mit einem db data reader zeile für zeile durchlaufen
                using(DbDataReader reader = await cmdAllUsers.ExecuteReaderAsync())
                {
                    //mit read wird jeweils eine einzelne zeile gelesen
                    while (await reader.ReadAsync())
                    {
                        //den User in der Liste abspeichern
                        users.Add(new User()
                        {
                            UserID = Convert.ToInt32(reader["user_id"]),
                            Passwort = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Email = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            IsAdmin = Convert.ToBoolean(reader["isAdmin"])

                        }
                        );
                    }

                }//using: hier wird automatisch der DbDataReadder freigegeben
                // entspricht dem finally



            }
            //es wird entweder eine lehre liste oder eine liste mit allen usern zurückgeliefert
            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();
                
                cmdInsert.CommandText = "select * from users where user_id = @userID";
                //leeres Parameter Object erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramUN.ParameterName = "userID";
                paramUN.DbType = DbType.Int32;
                paramUN.Value = userId;



                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramUN);

                using(DbDataReader reader = await cmdInsert.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        User user = new User()
                        {
                            UserID = Convert.ToInt32(reader["user_id"]),
                            
                            Passwort = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Email = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            IsAdmin = Convert.ToBoolean(reader["isAdmin"])
                        };
                        return user;
                    }
                }
            }
            return new User();

        }

        public async Task<bool> InsertAsync(User user)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "insert into users values(null, sha2(@password, 512), @mail,@Date,@Gender,0)";
                //Parameter @username befüllen
                //leeres Parameter Object erzeugen
                

                DbParameter paramPWD = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = user.Passwort;

                DbParameter paramEmail = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramEmail.ParameterName = "mail";
                paramEmail.DbType = DbType.String;
                paramEmail.Value = user.Email;

                DbParameter paramDate = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramDate.ParameterName = "Date";
                paramDate.DbType = DbType.Date;
                paramDate.Value = user.Birthdate;

                DbParameter paramG = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramG.ParameterName = "Gender";
                paramG.DbType = DbType.Int32;
                paramG.Value = user.Gender;

                //Paraneter mit unserem Command angeben
                
                cmdInsert.Parameters.Add(paramPWD);
                cmdInsert.Parameters.Add(paramEmail);
                cmdInsert.Parameters.Add(paramDate);
                cmdInsert.Parameters.Add(paramG);

                //nun senden wir das Command an den server
                return await cmdInsert.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }

        

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();

                cmdInsert.CommandText = "select email, isAdmin from users where email = @email and password = sha2(@password, 512)";
                //leeres Parameter Object erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramUN.ParameterName = "email";
                paramUN.DbType = DbType.String;
                paramUN.Value = email;

                DbParameter paramPWD = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramPWD.ParameterName = "password";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = password;


                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramUN);
                cmdInsert.Parameters.Add(paramPWD);
                using (DbDataReader reader = await cmdInsert.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string Username = Convert.ToString(reader["email"]);
                        int IsAdmin = Convert.ToInt32(reader["isAdmin"]);
                        if (Username.Equals(email))
                        {
                            
                            _httpContextAccessor.HttpContext.Session.SetString("Username", Username);
                            _httpContextAccessor.HttpContext.Session.SetInt32("IsAdmin", IsAdmin);
                            return true;
                        }


                    }
                }
            }
            return false;
        }

        
    }
}
