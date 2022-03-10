using CarPlanet.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApp.Models.DB
{
    //Diese Klasse implementiert unser Interface
    public class RepositoryUsersDB : IRepositoryUsers
    {
        //Verbindungszeichenkette: enthält Server Ip, Datendbankname, User + Passwort
        // DB - Server
        private string _connectionString = "Server=localhost;database=web_4b_gl;user=root;password=";
        //über diese verbindung wird mit dem sever komuniziert
        private DbConnection _conn;
        public void Disconnect()
        {
           //fals die Verbindung existiert und geöffnet ist
           if((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                this._conn.Close();
            }
        }
        public void Connect()
        {
            if (this._conn == null)
            {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open)
            {
                this._conn.Open();
            }
        }
        public bool ChangeUserData(int userId, User newUserData)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "update @username from users where user_id = @userID";
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
                return cmdInsert.ExecuteNonQuery() == 1;

            }
            return false;
        }

        

        public bool Delete(int userId)
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
                return true;

            }
            return false;
        }

        

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            if(this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdAllUsers = this._conn.CreateCommand();
                cmdAllUsers.CommandText = "select * from users";

                //Wir bekommen nun eine komplette tabelle zurück
                //diese wird mit einem db data reader zeile für zeile durchlaufen
                using(DbDataReader reader = cmdAllUsers.ExecuteReader())
                {
                    //mit read wird jeweils eine einzelne zeile gelesen
                    while (reader.Read())
                    {
                        //den User in der Liste abspeichern
                        users.Add(new User()
                        {
                            UserID = Convert.ToInt32(reader["user_id"]),
                            Passwort = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Email = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"])

                        }
                        );
                    }

                }//using: hier wird automatisch der DbDataReadder freigegeben
                // entspricht dem finally



            }
            //es wird entweder eine lehre liste oder eine liste mit allen usern zurückgeliefert
            return users;
        }

        public User GetUser(int userId)
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

                using(DbDataReader reader = cmdInsert.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User user = new User()
                        {
                            UserID = Convert.ToInt32(reader["user_id"]),
                            
                            Passwort = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Email = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"])
                        };
                        return user;
                    }
                }
            }
            return new User();

        }

        public bool Insert(User user)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "insert into users values(null, sha2(@password, 512), @mail,@Date,@Gender)";
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
                return cmdInsert.ExecuteNonQuery() == 1;

            }
            return false;
        }

        public bool Login(string username, string password)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();

                cmdInsert.CommandText = "select password from users where username = @username";
                //leeres Parameter Object erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramUN.ParameterName = "username";
                paramUN.DbType = DbType.Int32;
                paramUN.Value = username;



                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramUN);

                using (DbDataReader reader = cmdInsert.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        String Passwort = Convert.ToString(reader["password"]);
                        if (Passwort.Equals(password))
                        {
                            return true;
                        }    
                        
                        
                    }
                }
            }
            return false;
        }

        
    }
}
