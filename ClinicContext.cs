using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class ClinicContext : DbContext
{
    public DbSet<Person> Person { get; set; }
    public DbSet<AdminUser> AdminUser { get; set; }
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Visit> Visit { get; set; }

    public ClinicContext(DbContextOptions<ClinicContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlite("Data Source=clinic.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
