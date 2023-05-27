using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data
{
    public class InventoryContext : IdentityDbContext
    {
        public InventoryContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Unit> Units { get; set; }
    }

 }
