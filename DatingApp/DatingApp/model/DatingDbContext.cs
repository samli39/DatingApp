using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.model
{
    public class DatingDbContext:DbContext
    {
        public DatingDbContext(DbContextOptions<DatingDbContext> context) : base(context) { }

        public DbSet<Values> Values { set; get; }
    }
}
