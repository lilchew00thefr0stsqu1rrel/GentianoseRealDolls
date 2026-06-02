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
                        "', '" + 2 + "');");
                else 
                    AddOrChangeRecord("INSERT OR IGNORE INTO inventory (itemID, amount) VALUES ('" + i +
                        "', '" + 0 + "');");
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
                    command.CommandText = "CREATE TABLE IF NOT EXISTS positions (dollID INTEGER PRIMARY KEY, levelID INTEGER, x INTEGER, y INTEGER, z INTEGER)";
                    command.ExecuteNonQuery();
                }

                print("-Position- table was created");

                connection.Close();
            }
        }



        public void CreateTableInventory()
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
                    command.CommandText = "CREATE TABLE IF NOT EXISTS inventory (itemID INTEGER PRIMARY KEY, amount INTEGER)";
                    command.ExecuteNonQuery();
                }

                print("-inve- table was created");

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
                    command.CommandText =
                    "INSERT OR IGNORE INTO positions (dollID, levelID, x, y, z) VALUES ('" + petDollID +
                        "', '" + petLevelID + "', '" + petX + "', '" + petY + "', '" + petZ + "');";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            print("Doll position was added!!!");
        }
        public void AddItem(int itemID, int amount)
        {
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT OR IGNORE INTO inventory (itemID, amount) VALUES ('" + itemID +
                        "', '" + amount + "');";
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

        public bool GetRecord(int index, string query, ref int[] dataInt, string[] fieldNames)
        {
            int[] arr = new int[7];
            int row = 0;
            using (var connection = new SqliteConnection(dbPathURI))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (row == index)
                            {
                                for (int i = 0; i < fieldNames.Length; i++)
                                {
                                    arr[i] = int.Parse(reader[fieldNames[i]].ToString());
                                }
                                return true;
                            }
                            row++;
                        }
                    }
                }
                connection.Close();
            }

            return false;
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
