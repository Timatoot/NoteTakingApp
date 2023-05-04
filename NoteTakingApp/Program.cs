using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System;
using System.Diagnostics;

namespace NoteTakingApp
{
    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            const string connectionUri = "mongodb+srv://tim4monkey:timatoot@noteapptim.9l2spze.mongodb.net/?authSource=admin";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to Stable API version 1
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            var client = new MongoClient(settings);
            // Send a ping to confirm a successful connection
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            /*string atlas = "mongodb + srv://tim4monkey:%40ItzJust2Easy@noteapptim.9l2spze.mongodb.net/test";
            MongoClient dbClient = new MongoClient(settings);
            var dbList = dbClient.ListDatabases().ToList();

            Debug.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Debug.WriteLine(db);
            }*/

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}