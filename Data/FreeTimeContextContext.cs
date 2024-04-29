using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using freeTime.Models;
using Microsoft.EntityFrameworkCore;

    public class FreeTimeContext : DbContext
    {
        public FreeTimeContext(DbContextOptions<FreeTimeContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
    }
