using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    internal class Concerts : BaseModel
    {
        public int id {  get; set; }
        public string BandName { get; set; }
        public string VenueName { get; set; }
        public DateTime Date { get; set; }
    }
}
