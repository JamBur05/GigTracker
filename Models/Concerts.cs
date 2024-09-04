using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    /// <summary>
    /// Concert database table
    /// </summary>
    public class Concerts : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id {  get; set; }
        [Column("BandName")]
        public string BandName { get; set; }
        [Column("VenueName")]
        public string VenueName { get; set; }
        [Column("Date")]
        public DateTime Date { get; set; }
    }
}
