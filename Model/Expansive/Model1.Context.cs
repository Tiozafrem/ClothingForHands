using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ClothingForHands.Model
{
    public partial class Session_1Entities : DbContext
    {
        public static Session_1Entities Context { get; set; } = new Session_1Entities();

        
    }
}
