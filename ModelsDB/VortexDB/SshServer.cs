using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VortexCore.ModelsDB.VortexDB
{
    public partial class SshServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string System { get; set; }
    }
}
