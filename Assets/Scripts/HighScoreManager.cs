using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//namespaces below are required for SQLite
using System;
using System.Data;
using Mono.Data.Sqlite;

public class HighScoreManager : MonoBehaviour
{
    private MainManager mainManager;

    public List<HighScore> highScores = new List<HighScore>();

    private string connectionString;
    public string topName;

    public int topScore;

    // Start is called before the first frame update
    void Start()
    {
        //set mainManager variable to allow use of that class's variables
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();

        //set connection string in start
        connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";
    }

    public void InsertScore(string name, int newScore)
    {
        //connects Unity to SQL db via connection string
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //opens connections to db
            dbConnection.Open();

            //creates command variable so we can send commands to db
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                //creates query (write in SQL and then paste into string for ease)
                //must format string in order to pass in usable values
                string sqlQuery = string.Format("INSERT INTO HighScores(name, score) VALUES(\"{0}\",\"{1}\")", name, newScore);

                //turns query into command sent to db
                dbCommand.CommandText = sqlQuery;

                //executues command
                dbCommand.ExecuteScalar();

                //close connection to db
                //always do outside of while loop, so you run all functions, then close
                dbConnection.Close();
            }
        }
    }

    public void GetTopScore()
    {
        //connects Unity to SQL db via connection string
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //opens connections to db
            dbConnection.Open();

            //creates command variable so we can send commands to db
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                //query to sort scores and display top score
                string sqlQuery = "Select * from HighScores Order by Score DESC limit 1";

                //turns query into command sent to db
                dbCommand.CommandText = sqlQuery;

                //executes command and returns result
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topScore = reader.GetInt32(2);
                        topName = reader.GetString(1);
                    }

                    //close connection to db
                    //always do outside of while loop, so you run all functions, then close
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }

    public void GetTopTen()
    {
        {
            //clears lsit so it can be rebuilt below
            highScores.Clear();

            //connects Unity to SQL db via connection string
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                //opens connections to db
                dbConnection.Open();

                //creates command variable so we can send commands to db
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //retrieves and sorts top 10 scores for display
                    string sqlQuery = "Select * from HighScores Order by Score DESC limit 7";

                    //turns query into command sent to db
                    dbCommand.CommandText = sqlQuery;

                    //executes command and returns result
                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //adds results to list
                            highScores.Add(new HighScore(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(1)));
                        }

                        //close connection to db
                        //always do outside of while loop, so you run all functions, then close
                        dbConnection.Close();
                        reader.Close();
                    }
                }
            }
        }
    }
} 
