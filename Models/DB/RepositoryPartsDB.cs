using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CarPlanet.Models.DB {
    public class RepositoryPartsDB : IRepositoryParts {

        private string _connectionString = "Server=localhost;database=carplanet;user=root;password=";
        //über diese verbindung wird mit dem sever komuniziert
        private DbConnection _conn;
        public async Task DisconnectAsync() {
            //fals die Verbindung existiert und geöffnet ist
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open))
            {
                await this._conn.CloseAsync();
            }
        }
        public async Task ConnectAsync() {
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

        public async Task<bool> DeleteAsync(int partID) {
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdInsert = this._conn.CreateCommand();
                //SQL Befahl angeben und Parameter verwenden um sql injections zu vermeiden
                //  @username ... kann frei gewählt werden
                //SQL injection: es versucht ein Angreifer einen SQL-Befehl an den MySQL server zu senden
                cmdInsert.CommandText = "delete from parts where part_id = @partID";
                //Parameter @username befüllen
                //leeres Parameter Object erzeugen
                DbParameter paramID = cmdInsert.CreateParameter();
                // hier denn oben gewählten Parameter name verwenden
                paramID.ParameterName = "partID";
                paramID.DbType = DbType.Int32;
                paramID.Value = partID;



                //Paraneter mit unserem Command angeben
                cmdInsert.Parameters.Add(paramID);


                //nun senden wir das Command an den server
                return await cmdInsert.ExecuteNonQueryAsync() == 1;

            }
            return false;
        }

        public async Task<List<Part>> GetCompatiblePartsAsync(int autoID) {

            List<Part> parts = new List<Part>();
            if (this._conn?.State == ConnectionState.Open)
            {

                DbCommand cmdCompParts = this._conn.CreateCommand();
                cmdCompParts.CommandText = "select part_id, name, description, link from parts join compatible using(part_id) where auto_id = @autoID";

                DbParameter paramID = cmdCompParts.CreateParameter();

                paramID.ParameterName = "autoID";
                paramID.DbType = DbType.Int32;
                paramID.Value = autoID;
                cmdCompParts.Parameters.Add(paramID);

                //Wir bekommen nun eine komplette tabelle zurück
                //diese wird mit einem db data reader zeile für zeile durchlaufen
                using (DbDataReader reader = await cmdCompParts.ExecuteReaderAsync())
                {
                    //mit read wird jeweils eine einzelne zeile gelesen
                    while (await reader.ReadAsync())
                    {
                        //den User in der Liste abspeichern
                        parts.Add(new Part()
                        {
                            PartID = Convert.ToInt32(reader["part_id"]),
                            Description = Convert.ToString(reader["description"]),
                            Name = Convert.ToString(reader["Name"]),
                            Link = Convert.ToString(reader["Link"])

                        }
                        );
                    }

                }//using: hier wird automatisch der DbDataReadder freigegeben
                // entspricht dem finally



            }
            //es wird entweder eine lehre liste oder eine liste mit allen usern zurückgeliefert
            return parts;
        }

        public async Task<bool> InsertAsync(Part part) {
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
                paramN.Value = part.Name;

                DbParameter paramB = cmd.CreateParameter();
                paramB.ParameterName = "Beschreibung";
                paramB.DbType = System.Data.DbType.String;
                paramB.Value = part.Description;

                

                DbParameter paramL = cmd.CreateParameter();
                paramL.ParameterName = "Link";
                paramL.DbType = System.Data.DbType.Int32;
                paramL.Value = part.Link;

                //Paraneter mit unserem Command angeben

                cmd.Parameters.Add(paramN);
                cmd.Parameters.Add(paramB);
                cmd.Parameters.Add(paramL);


                //nun senden wir das Command an den server
                return await cmd.ExecuteNonQueryAsync() == 1;

            }
            return false;

        }
    }





    }

