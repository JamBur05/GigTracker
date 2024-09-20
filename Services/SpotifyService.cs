using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GigTracker.Services
{
    public class SpotifyService
    {
        private readonly string privateKey;
        private SpotifyClient _spotifyAPI;
        public SpotifyService(string privateKey)
        {
            // Set Private key once user has been authenticated
            this.privateKey = privateKey;
            _spotifyAPI = new SpotifyClient(privateKey);

            // Await the call to get user profile info
            GetUserInfoAsync();
            GetTopArtistsAsync();
        }

        private async void GetUserInfoAsync()
        {
            try
            {
                var userinfo = await _spotifyAPI.UserProfile.Current();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving user info: {ex.Message}");
            }
        }

        public async Task<UsersTopArtistsResponse> GetTopArtistsAsync()
        {
            try
            { 
                var request = new UsersTopItemsRequest(TimeRange.ShortTerm)
                {
                    Limit = 10,
                };
                
                var topArtistsResponse = await _spotifyAPI.UserProfile.GetTopArtists(request);

                return topArtistsResponse;
            }
            catch (APIUnauthorizedException)
            {
                MessageBox.Show("User is not authenticated. Please log in.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving top artists: {ex.Message}");
            }

            return null;
        }
    }
}
