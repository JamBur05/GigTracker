using GigTracker.Models;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;
using System.Windows;


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

        public async Task<Users> ValidUser(string username, string password)
        {
            try
            {
                var userSearch = await client
                    .From<Users>()
                    .Where(u => u.username == username)
                    .Get();

                // Check if any user is found
                if (userSearch.Models != null && userSearch.Models.Any())
                {
                    var foundUser = userSearch.Models.First();

                    // Extract the salt from the database
                    string storedSalt = foundUser.user_salt;

                    password += storedSalt;


                    // Compare the hashed password with the one stored in the database
                    if (PasswordService.VerifyPassword(password, foundUser.password, storedSalt))
                    {
                        // Password matches return the user
                        return foundUser;
                    }
                    else
                    {
                        // Password wrong return null or indicate failure
                        return null;
                    }
                }
                else
                {
                    // No user found with the given username return null
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return null;
            }
        }


        public async Task CreateAccount(string username, string password, string userSalt)
        {
            try
            {
                // Search if username already taken
                var userSearch = await client
                    .From<Users>()
                    .Where(u => u.username == username)
                    .Get();
                if (userSearch.Models != null && userSearch.Models.Any())
                {
                    MessageBox.Show("Username already taken!");
                    return;
                }

                var account = new Users
                {
                    username = username,
                    password = password,
                    user_salt = userSalt
                };

                await client.From<Users>().Insert(account);
                MessageBox.Show("Account created!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to create account" + ex.Message);
            }
        }

        public async Task DeleteConcert(Concerts concert)
        {
            try
            {
                // Delete relationship between user and concert
                await client
                    .From<UserConcerts>()
                    .Where(x => x.concertID == concert.id)
                    .Delete();

                // Delete concert
                await client
                    .From<Concerts>()
                    .Where(x => x.id == concert.id)
                    .Delete();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to delete account" + ex.Message);
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

        public async Task AddConcert(int userID, string bandName, string venueName, DateTime date)
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
                    MessageBox.Show("Concert added!");
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

        public async Task UpdateConcert(Concerts selectedConcert, Concerts newConcert)
        {
            try
            {
                var update = await client
                    .From<Concerts>()
                    .Where(x => x.id == selectedConcert.id)
                    .Set(x => x.BandName, newConcert.BandName)
                    .Set(x => x.VenueName, newConcert.VenueName)
                    .Set(x => x.Date, newConcert.Date)
                    .Update();

                MessageBox.Show("Concert Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating concert" + ex.Message);
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
                    return new List<Concerts>(); // Return an empty list if no records are found
                }

                // Log the number of records fetched
                var userConcertsCount = userConcertsResult.Models.Count;

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
    