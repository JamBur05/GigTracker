using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    internal class Users : BaseModel
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
