using GigTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Events
{
    public class LoginSuccessEventArgs : EventArgs
    {
        public Users User { get; }
        public LoginSuccessEventArgs(Users user) => User = user;
    }
}
