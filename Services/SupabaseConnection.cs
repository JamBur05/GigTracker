using GigTracker.Models;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
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

        public async Task<int> ValidUser(string username, string password)
        {
            try
            {
                var userSearch = await client
                    .From<Users>()
                    .Where(u => u.username == username)
                    .Where(u => u.password == password)
                    .Get();

                // Check if any user is found
                if(userSearch.Models != null && userSearch.Models.Any())
                {
                    var user = userSearch.Models.First();
                    return user.id; // Return the userID
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("No user found");
                return -1;
            }

        }

        public async void CreateAccount(string username, string password)
        {
            try
            {
                var account = new Users
                {
                    username = username,
                    password = password
                };

                await client.From<Users>().Insert(account);
                MessageBox.Show("Account created!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to create account" + ex.Message);
            }
        }

        private async Task CreateUserConcertRelationship(int userID, int concertID)
        {
            var newRelationship = new UserConcerts
            {
                concertID = concertID,
                userID = userID
            };
            await client.From<UserConcerts>().Insert(newRelationship);
        }

        public async void AddConcert(int userID, string bandName, string venueName, DateTime date)
        {
            try
            {
                // Add new concert
                var newConcert = new Concerts
                {
                    BandName = bandName,
                    VenueName = venueName,
                    Date = date
                };

                // Insert the new concert and get the response
                var insertResponse = await client.From<Concerts>().Insert(newConcert);

                // Check if the insert was successful and retrieve the ID
                if (insertResponse.Models.Any())
                {
                    var insertedConcert = insertResponse.Models.First();
                    int concertID = insertedConcert.id; // Retrieve the auto-generated ID

                    // Now create the relationship in the UserConcerts table
                    await CreateUserConcertRelationship(userID, concertID);
                }
                else
                {
                    throw new Exception("Failed to insert concert.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new concert" + ex.Message);
            }
        }

        public async Task<List<Concerts>> FetchConcerts(int userID)
        {
            try
            {
                // Fetch the list of UserConcerts records associated with the given userID
                var userConcertsResult = await client
                    .From<UserConcerts>()
                    .Where(uc => uc.userID == userID)
                    .Get();

                // Check if the result contains models
                if (userConcertsResult.Models == null || !userConcertsResult.Models.Any())
                {
                    MessageBox.Show("No UserConcerts records found.");
                    return new List<Concerts>(); // Return an empty list if no records are found
                }

                // Log the number of records fetched
                var userConcertsCount = userConcertsResult.Models.Count;
                MessageBox.Show($"Number of UserConcerts records found: {userConcertsCount}");

                // Extract the list of concert IDs
                var concertIDs = userConcertsResult.Models.Select(uc => uc.concertID).ToList();

                // Check if there are any concert IDs to fetch
                if (!concertIDs.Any())
                {
                    MessageBox.Show("No concert IDs found.");
                    return new List<Concerts>(); // Return an empty list if no concert IDs are found
                }

                // Create a list to hold the fetched concerts
                var concertsList = new List<Concerts>();

                // Fetch each concert by ID
                foreach (var concertID in concertIDs)
                {
                    var concertResult = await client
                        .From<Concerts>()
                        .Where(c => c.id == concertID)
                        .Get(); // Assuming id is unique, you can use SingleOrDefault

                    if (concertResult.Models != null && concertResult.Models.Any())
                    {
                        concertsList.Add(concertResult.Models.First());
                    }
                }

                // Log the number of concerts fetched
                var concertsCount = concertsList.Count;

                // Return the list of concerts
                return concertsList;
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                MessageBox.Show($"An error occurred: {ex.Message}");
                return new List<Concerts>(); // Return an empty list in case of error
            }
        }

    }
}
    