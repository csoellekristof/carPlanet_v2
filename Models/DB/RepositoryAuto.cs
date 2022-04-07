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

        public async Task<bool> ChangeAutoDataAsync(int autoId, Autos newAutoData)
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

        public async Task<Autos> GetAutoAsync(int autoId)
        {
            if (this._conn?.State == ConnectionState.Open)
            {
                DbCommand cmdInsert = this._conn.CreateCommand();

                cmdInsert.CommandText = "select * from autos where auto_id = @autoID";
                //leeres Parameter Object erzeugen
                DbParameter paramUN = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramUN.ParameterName = "autoID";
                paramUN.DbType = DbType.Int32;
                paramUN.Value = autoId;



                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramUN);

                using (DbDataReader reader = await cmdInsert.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Autos auto = new Autos()
                        {
                            AutoId = Convert.ToInt32(reader["auto_id"]),
                            Beschreibung = Convert.ToString(reader["Beschreibung"]),
                            Typ = Convert.ToString(reader["Typ"]),
                            Name = Convert.ToString(reader["Name"]),
                            Link = Convert.ToString(reader["Link"])
                        };
                        return auto;
                    }
                }
            }
            return new Autos();

        }

        public async Task<bool> InsertAsync(Autos auto)
        {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmd = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmd.CommandText = "insert into autos values(null, @Name, @Beschreibung,@Typ,@Link)";
                //Parameter @username befüllen
                //leeres Parameter Object erzeugen


                DbParameter paramN = cmd.CreateParameter();
                paramN.ParameterName = "Name";
                paramN.DbType = System.Data.DbType.String;
                paramN.Value = auto.Name;

                DbParameter paramB = cmd.CreateParameter();
                paramB.ParameterName = "Beschreibung";
                paramB.DbType = System.Data.DbType.String;
                paramB.Value = auto.Beschreibung;

                DbParameter paramT = cmd.CreateParameter();
                paramT.ParameterName = "Typ";
                paramT.DbType = System.Data.DbType.Int32;
                paramT.Value = auto.Typ;

                DbParameter paramL = cmd.CreateParameter();
                paramL.ParameterName = "Link";
                paramL.DbType = System.Data.DbType.Int32;
                paramL.Value = auto.Link;

                //Paraneter mit unserem Command angeben

                cmd.Parameters.Add(paramN);
                cmd.Parameters.Add(paramB);
                cmd.Parameters.Add(paramT);
                cmd.Parameters.Add(paramL);
                

                //nun senden wir das Command an den server
                return await cmd.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }
    }
}
