using System;
using System.Collections.Generic;

namespace VortexCore.ModelsDB
{
    public partial class UserLogins
    {
        public int Id { get; set; }
        public DateTimeOffset LoginDateTime { get; set; }

        public virtual Users Users { get; set; }
    }
}
