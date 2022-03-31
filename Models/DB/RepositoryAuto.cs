using CarPlanet.Models.DB.sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models.DB
{
    public class RepositoryAuto : IRepositoryAuto
    {

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

        public async Task<bool> ChangeUserDataAsync(int userId, Autos newAutoData)
        {
            if (this._conn?.State == System.Data.ConnectionState.Open)
            {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "update Autos set Name = @name, " +
                    "Beschreibung = @beschreibung,  Typ = @typ , Link = @Link where user_id = @user_id";


                DbParameter paramN = cmd.CreateParameter();
                paramN.ParameterName = "name";
                paramN.DbType = System.Data.DbType.String;
                paramN.Value = newAutoData.Name;

                DbParameter paramB = cmd.CreateParameter();
                paramB.ParameterName = "beschreibung";
                paramB.DbType = System.Data.DbType.String;
                paramB.Value = newAutoData.Beschreibung;

                DbParameter paramT = cmd.CreateParameter();
                paramT.ParameterName = "typ";
                paramT.DbType = System.Data.DbType.Int32;
                paramT.Value = newAutoData.Typ;

                DbParameter paramL = cmd.CreateParameter();
                paramL.ParameterName = "Link";
                paramL.DbType = System.Data.DbType.Int32;
                paramL.Value = newAutoData.Link;

                DbParameter paramID = cmd.CreateParameter();
                paramID.ParameterName = "user_id";
                paramID.DbType = System.Data.DbType.Int32;
                paramID.Value = newAutoData.AutoId;



                cmd.Parameters.Add(paramN);
                cmd.Parameters.Add(paramB);
                cmd.Parameters.Add(paramT);
                cmd.Parameters.Add(paramL);
                cmd.Parameters.Add(paramID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }



        public async Task<bool> DeleteAsync(int autoId)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "delete from autos where autop_id = @autoID";
                //Parameter @username befüllen
                //leeres Parameter Object erzeugen
                DbParameter paramID = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramID.ParameterName = "autoID";
                paramID.DbType = DbType.Int32;
                paramID.Value = autoId;



                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramID);


                //nun senden wir das Command an den server
                return await cmdInsert.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }



        public async Task<List<Autos>> GetAllAutosAsync()
        {
            List<Autos> autos = new List<Autos>();
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdAllUsers = this._conn.CreateCommand();
                cmdAllUsers.CommandText = "select * from autos";

                //Wir bekommen nun eine komplette tabelle zurück
                //diese wird mit einem db data reader zeile für zeile durchlaufen
                using (DbDataReader reader = await cmdAllUsers.ExecuteReaderAsync())
                {
                    //mit read wird jeweils eine einzelne zeile gelesen
                    while (await reader.ReadAsync())
                    {
                        //den User in der Liste abspeichern
                        autos.Add(new Autos()
                        {
                            AutoId = Convert.ToInt32(reader["auto_id"]),
                            Beschreibung = Convert.ToString(reader["Beschreibung"]),
                            Typ = Convert.ToString(reader["Typ"]),
                            Name = Convert.ToString(reader["Name"]),
                            Link = Convert.ToString(reader["Link"])

                        }
                        );
                    }

                }//using: hier wird automatisch der DbDataReadder freigegeben
                // entspricht dem finally



            }
            //es wird entweder eine lehre liste oder eine liste mit allen usern zurückgeliefert
            return autos;
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

                using (DbDataReader reader = await cmdInsert.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
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

        public async Task<bool> InsertAsync(User user)
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
                return await cmdInsert.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }
    }
}
