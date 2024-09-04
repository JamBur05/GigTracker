using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    /// <summary>
    /// UserConcert database table
    /// </summary>
    public class UserConcerts : BaseModel
    {
        [PrimaryKey("UserConcertID", false)]
        public int UserConcertID { get; set; }
        //[ForeignKey("concertID")]
        public int concertID { get; set; }
        //[ForeignKey("userID")]
        public int userID { get; set; }
    }
}
