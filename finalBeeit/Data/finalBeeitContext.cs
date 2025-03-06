using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using finalBeeit.Models;

namespace finalBeeit.Data
{
    public class finalBeeitContext : DbContext
    {
        public finalBeeitContext (DbContextOptions<finalBeeitContext> options)
            : base(options)
        {
        }

        public DbSet<finalBeeit.Models.Game> Game { get; set; } = default!;
    }
}
