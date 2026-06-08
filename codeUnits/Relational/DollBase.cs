using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GentianoseRealDolls
{

    public class DollBase : MonoBehaviour
    {
        string fileName = "MustelidsAndPrimatesBase.db";
        string dbPath;
        string dbPathURI;

        /// <summary>
        /// В будущем - OCP
        /// </summary>
        [SerializeField] private string[] m_QueryToCreateBase =
        {
            "CREATE TABLE IF NOT EXISTS positions (dollID INTEGER PRIMARY KEY, levelID INTEGER, x INTEGER, y INTEGER, z INTEGER)",

        }; 
        
        [SerializeField]
        private string[] m_QueryToAddRecLeft =
        {
            "INSERT OR IGNORE INTO positions (dollID, levelID, x, y, z) VALUES ('" 
        }; 
        
        [SerializeField]
        private string[] m_QueryToAddRecRight =
        {
             "', '" + 1 + "', '" + 0 + "', '" + 0 + "', '" + 0 + "');"
        };

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


            sqlite3_initialize();

            CreateDB();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {


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
            var q1 = "CREATE TABLE IF NOT EXISTS positions (dollID INTEGER PRIMARY KEY, levelID INTEGER, x INTEGER, y INTEGER, z INTEGER)";
            CreateTable(q1);
            //CreateTablePosition();

            // Seed data.
            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                //AddDollPosition(i, 1, 0, 0, 0);

                AddOrChangeRecord("INSERT OR IGNORE INTO positions (dollID, levelID, x, y, z) VALUES ('" + i +
                        "', '" + 1 + "', '" + 0 + "', '" + 0 + "', '" + 0 + "');");
            }

            var q2 = "CREATE TABLE IF NOT EXISTS inventory (itemID INTEGER PRIMARY KEY, amount INTEGER)";

            CreateTable(q2);

            // Seed data.
            for (int i = 0; i < 16; i++)
            {
                if (i < 2)
                    AddOrChangeRecord("INSERT OR IGNORE INTO inventory (itemID, amount) VALUES ('" + i +
                        "', '" + 162 + "');");
                else 
                    AddOrChangeRecord("INSERT OR IGNORE INTO inventory (itemID, amount) VALUES ('" + i +
                        "', '" + 0 + "');");
            }

            var q3 = "CREATE TABLE IF NOT EXISTS dollStats (dollID INTEGER PRIMARY KEY, " +
                "poo INTEGER, analSpray INTEGER, pee INTEGER, bath INTEGER, brushTeeth INTEGER, " +
                "food INTEGER, sleep INTEGER)";
            CreateTable(q3);

            // Seed data.
            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                AddOrChangeRecord("INSERT OR IGNORE INTO dollStats " +
                    "(dollID, poo, analSpray, pee, bath, brushTeeth, food, sleep) " +
                    "VALUES ('" + i +
                        "', '" + 0 + "', '" + 0 + "', '" + 60 + "', '" + 0 + "', '" + 0 + "', '" +
                        0 + "', '" + 0 + "');");
            }

            var q4 = "CREATE TABLE IF NOT EXISTS dollSleeps (dollID INTEGER PRIMARY KEY, " +
                "inBed INTEGER)";
            CreateTable(q4);

            // Seed data.
            for (int i = 0; i < 3; i++)
            {
                AddOrChangeRecord("INSERT OR IGNORE INTO dollSleeps " +
                    "(dollID, inBed) " +
                    "VALUES ('" + i +
                        "', '" + 0 + "');");
            }

            var q5 = "CREATE TABLE IF NOT EXISTS dollBattle (dollID INTEGER PRIMARY KEY, " +
                "hp INTEGER)";
            CreateTable(q5);

            // Seed data.
            for (int i = 0; i < 3; i++)
            {
                AddOrChangeRecord("INSERT OR IGNORE INTO dollBattle " +
                    "(dollID, hp) " +
                    "VALUES ('" + i +
                        "', '" + 1008 + "');");
            }
        }


        public void ChangeItemAmount(int itemID, int amount)
        {
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE inventory SET " +
                        $"amount='{amount}' WHERE itemID='{itemID}';";
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

        public int[] GetItemAmounts()
        {
            int[] ia = new int[16];


            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM inventory;";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ia[int.Parse(reader["itemID"].ToString())] = int.Parse(reader["amount"].ToString());

                            
                        }
                    }
                }

                connection.Close();
            }


            return ia;
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

        // Абстрактн.


        public int GetRecordAmount(string tableName)
        {
            int i = 0;

            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {tableName}";

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

        public void CreateTable(string query)
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
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                print("-inve- table was created");

                connection.Close();
            }
        }

        public bool CheckRecordPresent(int petDollID, string tableName)
        {
            bool isPresent = false;

            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE dollID = {petDollID}";

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

        public int[] GetRecord(string tableName, string idName, int id, string[] fieldNames)
        {
            int[] arr = new int[8] {id, 0, 0, 0, 0, 0, 0, 0};

            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {tableName};";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (int.Parse(reader[fieldNames[0]].ToString()) == id)
                            {
                                for (int i = 0; i < fieldNames.Length; i++)
                                {
                                    arr[i] = int.Parse(reader[fieldNames[i]].ToString());
                                }
                            }
                        }
                    }
                }
                connection.Close();
            }
            return arr;
        }



        public void AddOrChangeRecord(string query)
        {
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }



    }

    
        

    }
