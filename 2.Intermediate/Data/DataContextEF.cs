using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyApp.Models;

namespace MyApp.Data {

    //con los ":" hace el extends, esta clase tiene herencia de dbContext clase q viene de entityFramewor
    public class DataContextEF: DbContext{

        public DbSet<Computer>? Computer {get; set;}
        private string ? _connectionStringMac;

        public DataContextEF(IConfiguration config){
            _connectionStringMac =  config.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured){
                //cambio el approach por el de tener la conecion en el archivo appsettings
                //optionsBuilder.UseSqlServer("Server = localhost; Database = DotNetCourseDatabase; TrustServerCertificate = true; Trusted_Connection=false; User Id = SA ; Password=SQLConnect1",
                optionsBuilder.UseSqlServer(_connectionStringMac,
                    optionsBuilder=> optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //se puede hacer asi:
            modelBuilder.Entity<Computer>().ToTable("Computer","TutorialAppSchema")
            .HasKey(c =>c.ComputerId); //se debe declarar la clave primaria
            //.HasNoKey(); //o indicar q no tiene 

            //o asi:
            //modelBuilder.HasDefaultSchema("TutorialAppSchema");
            //modelBuilder.Entity<Computer>()
        }
    }
}