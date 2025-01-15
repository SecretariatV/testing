using Microsoft.EntityFrameworkCore;
using Test2Server.Data.Entities;

namespace Test2Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> users { get; set; }
        public DbSet<TodoEntity> todos { get; set; }
        public DbSet<TodoItemEntity> TodoItems { get; set; }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     modelBuilder.Entity<TodoItemEntity>()
        //         .HasOne(ti => ti.Todo)
        //         .WithMany()
        //         .HasForeignKey(ti => ti.TodoId)
        //         .OnDelete(DeleteBehavior.SetNull);
        // }
    }
}
