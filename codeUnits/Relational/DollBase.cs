using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;

namespace GentianoseRealDolls
{

    public class DollBase : MonoBehaviour
    {
        string fileName = "MustelidsAndPrimatesBase.db";
        string dbPath;
        string dbPathURI;

        private string GetDBPathURI(string fileName)
        {
            return $"URI=file:{Application.persistentDataPath}/{fileName}";
        }


        private string GetDBPath(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }

        private void Awake()
        {
            dbPath = GetDBPath(fileName);
            dbPathURI = GetDBPathURI(fileName);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            sqlite3_initialize();

            CreateDB();
        }

        [DllImport("sqlite3.dll")]
        private static extern void sqlite3_initialize();


        public void CreateDB()
        {
            print("Whoo Mink " + File.Exists(dbPath));

            if (!File.Exists(dbPath))
            {
                SqliteConnection.CreateFile(dbPath);

            }


            CreateTablePosition();

            // Seed data.
            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                AddDollPosition(i, 1, 0, 0, 0);
            }

        }

        public void CreateTablePosition()
        {
            print("Whew");
            using (var connection = new SqliteConnection(dbPathURI))
            {
                print("Dolly");

                print($"Dolly {connection != null}");

                connection.Open(); /// !!~~!
                print("Mink");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS positions (dollID INTEGER PRIMARY KEY, levelID INTEGER, x FLOAT, y FLOAT, z FLOAT)";
                    command.ExecuteNonQuery();
                }

                print("-Position- table was created");

                connection.Close();
            }
        }

        public void AddDollPosition(int petDollID, int petLevelID, float petX, float petY, float petZ)
        {
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO positions (dollID, levelID, x, y, z) VALUES ('" + petDollID +
                        "', '" + petLevelID + "', '" + petX + "', '" + petY + "', '" + petZ + "');";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            print("Doll position was added!!!");
        }

        public void ChangeDollPosition(int petDollID, int petLevelID, float petX, float petY, float petZ)
        {
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE positions SET levelID='{petLevelID}'," +
                        $"x='{petX}', y='{petY}', z='{petZ}' WHERE dollID='{petDollID}';";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public bool CheckDollPositionPresent(int petDollID)
        {
            bool isPresent = false;

            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM positions WHERE dollID = {petDollID};";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["dollID"].ToString() == petDollID.ToString()) isPresent = true;
                        }
                    }
                }

                connection.Close();
            }
            return isPresent;
        }


        public DollPosition GetDollPosition(int petDollID)
        {
            int map = 0;
            Vector3 pos = Vector3.zero;


            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM positions WHERE dollID = {petDollID};";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["dollID"].ToString() == petDollID.ToString())
                            {

                                map = int.Parse(reader["levelID"].ToString());

                                pos.x = float.Parse(reader["x"].ToString());
                                pos.y = float.Parse(reader["y"].ToString());
                                pos.z = float.Parse(reader["z"].ToString());
                            }
                        }
                    }
                }

                connection.Close();
            }


            return new DollPosition(petDollID, map, pos, Quaternion.identity);
        }

        public int GetDollAmount()
        {
            int i = 0;

            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM positions";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                }

                connection.Close();
            }
            return i;
        }
    }



}
