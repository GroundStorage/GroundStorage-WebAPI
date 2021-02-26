using Microsoft.EntityFrameworkCore;

namespace Ground_Storage_WebAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Key> Keys { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Conflict> Conflicts { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Data/Database/AppDatabase.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Key>()
                .HasKey(k => new { k.Name, k.RecordId });
        }
    }
}