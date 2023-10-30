using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess;

public class TodoContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source = ../EfcDataAccess/Todo.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasKey(todo => todo.Id);
        modelBuilder.Entity<User>().HasKey(user => user.Id);
    }
}