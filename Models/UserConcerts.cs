using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    internal class UserConcerts : BaseModel
    {
        public int UserConcertID { get; set; }
        public int concertID { get; set; }
        public int userID { get; set; }
    }
}
