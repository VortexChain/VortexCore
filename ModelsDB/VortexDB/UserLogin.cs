using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VortexCore.ModelsDB.VortexDB
{
    public partial class UserLogin
    {
        public int Id { get; set; }
        public DateTimeOffset LoginDateTime { get; set; }
    }
}
