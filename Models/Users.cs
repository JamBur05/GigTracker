﻿using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Models
{
    public class Users : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id {  get; set; }
        [Column("username")]
        public string username { get; set; }
        [Column("password")]
        public string password { get; set; }
    }
}
