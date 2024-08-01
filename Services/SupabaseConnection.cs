using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace GigTracker.Services
{
    internal class SupabaseConnection
    {
        private string supabaseUrl;
        private string supabaseApiKey;
        private Supabase.Client client;

        // Set up environment variables and conncet to client
        public SupabaseConnection()
        {
            supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
            supabaseApiKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");

            
        }

        public async Task InitializeClient()
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            client = new Supabase.Client(supabaseUrl, supabaseApiKey, options);

            try
            {
                await client.InitializeAsync();
                Console.WriteLine("Client initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Supabase client: {ex.Message}");
            }
        }

        public async Task FetchConcert()
        {
            try
            {
                var result = await client.From<Concerts>().Get();

                if (result == null)
                {
                    Console.WriteLine("Result is null.");
                    return;
                }

                if (result.Models == null || !result.Models.Any())
                {
                    Console.WriteLine("No concerts found.");
                    return;
                }

                foreach (var concert in result.Models)
                {
                    MessageBox.Show($"Concert ID: {concert.id}, Name: {concert.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching concerts: {ex.Message}");
            }
        }




    }

    class Concerts : BaseModel
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
}
    