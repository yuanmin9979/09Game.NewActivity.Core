using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _09Game.NewActivity.Core.Enity.DBConnect
{
    public class PifuDbContext : DbContext
    {
        public PifuDbContext(DbContextOptions<PifuDbContext> opts) : base(opts) { }
    }
}
