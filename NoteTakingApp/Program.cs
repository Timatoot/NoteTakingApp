using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Timers;

namespace NoteTakingApp
{
    internal static class Program
    {
        private static string _dbUser = "tim4monkey";
        private static string _dbPass = "timatoot";

        private static System.Timers.Timer tymur;

        public static string DbUser
        {
            get { return _dbUser; }
            set { _dbUser = value; }
        }
        public static string DbPass
        {
            get { return _dbPass; }
            set { _dbPass = value; }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CheckConnected();

            tymur = new System.Timers.Timer(5000); // Set up the timer for 5 seconds.
            tymur.Elapsed += TymurElapsed;
            tymur.AutoReset = true;
            tymur.Enabled = true;

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void CheckConnected()
        {
            string connectionUri = $"mongodb+srv://{_dbUser}:{_dbPass}@noteapptim.9l2spze.mongodb.net/?authSource=admin";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            var client = new MongoClient(settings);
            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("NoteApp").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Debug.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

          
        }
        private static void TymurElapsed(Object source, ElapsedEventArgs e)
        {
            var form = new Form1();
            Debug.WriteLine("Saving...");
            form.WriteDB();
            Debug.WriteLine("Saved!");
        }
    }
}