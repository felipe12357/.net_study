using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models;

namespace DotnetAPI.Data {

    //con los ":" hace el extends, esta clase tiene herencia de dbContext clase q viene de entityFramewor
    public class DataContextEF: DbContext{
        private string ? _connectionStringMac;
        public virtual DbSet<User> Users {get;set; }
        public virtual DbSet<UserSalary> UserSalary {get;set; }
        public virtual DbSet<UserJobInfo> UserJobInfo {get;set; }
        public virtual DbSet<UserAuth> UserAuth {get;set; }
        public virtual DbSet<Post> Post {get;set; }

        //Se pueden hacer cualquiera de las dos artenativas para obtener los parametros de la Bd
        //1 enviar la configuración desde el constructor del repositorio
        public DataContextEF(IConfiguration config){
            //2 crear una instancia de configuration builder aqui mismo
            // IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionStringMac =  config.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured){
                optionsBuilder.UseSqlServer(_connectionStringMac,
                    optionsBuilder=> optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //se indica la ruta donde se encuentra la tabla
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            //se indica la tabla correcta y cual es la clave primaria
            modelBuilder.Entity<User>().ToTable("Users","TutorialAppSchema")
            .HasKey(u =>u.UserId); //se debe declarar la clave primaria
            //.HasNoKey(); //o indicar q no tiene 
           
            modelBuilder.Entity<UserJobInfo>().ToTable("UserJobInfo","TutorialAppSchema")
            .HasKey(u =>u.UserId);

            modelBuilder.Entity<UserSalary>().ToTable("UserSalary","TutorialAppSchema")
            .HasKey(u =>u.UserId);

            modelBuilder.Entity<UserAuth>().ToTable("Auth","TutorialAppSchema")
            .HasKey(u =>u.Email);

            modelBuilder.Entity<Post>().ToTable("Posts","TutorialAppSchema")
            .HasKey(p =>p.PostId);
        }
    }
}