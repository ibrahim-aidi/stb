using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        
        public DbSet<User> Users { get; set; }
    }
}
