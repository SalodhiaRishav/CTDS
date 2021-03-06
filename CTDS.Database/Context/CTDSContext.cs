﻿
namespace CTDS.Database.Context
{
    using System.Data.Entity;
    using System.Configuration;

    using CTDS.Database.Models.Declarations;
    using CTDS.Database.Configurations.Declaration;
    using CTDS.Database.Models.CaseManagement;
    using CTDS.Database.Configurations.CaseManagement;
    using CTDS.Database.Models.Authentication;
    using CTDS.Database.Models.Common;
    using CTDS.Database.Configurations.MasterData;

    public class CTDSContext : DbContext
    {
        //private static string connection =
        //ConfigurationManager.ConnectionStrings["CTDSContext"].ConnectionString;
        public CTDSContext() : base("CTDSContext")
        {

        }
        public CTDSContext(string connectionString) :base(connectionString)
        {

        }
        public DbSet<Declaration> Declaration { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<CaseInformation> CaseInformation {get; set;}
        public DbSet<CaseReference> CaseReference { get; set; }
        public DbSet<CaseStatus> CaseStatus { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<ReferenceTable> Reference { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Token> UserToken { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<MasterData> MasterData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure Column
            modelBuilder.Configurations.Add(new DeclarationConfig());
            modelBuilder.Configurations.Add(new CaseConfigurations());
            modelBuilder.Configurations.Add(new CaseInformationConfigurations());
            modelBuilder.Configurations.Add(new CaseReferenceConfigurations());
            modelBuilder.Configurations.Add(new CaseStatusConfigurations());
            modelBuilder.Configurations.Add(new ClientConfigurations());
            modelBuilder.Configurations.Add(new NotesConfigurations());
            modelBuilder.Configurations.Add(new ReferenceConfig());
            modelBuilder.Configurations.Add(new MasterDataConfig());


        }

    }
}
