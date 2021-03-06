﻿namespace CTDS.Database.Configurations.CaseManagement
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;

    using CTDS.Database.Models.CaseManagement;
   

    public class ClientConfigurations : EntityTypeConfiguration<Client>
    {
        public ClientConfigurations()
        {
            this.HasKey(client => client.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(client => client.IdentifierType)
                .IsRequired();

            this.Property(client => client.ClientIdentifier)
                .IsRequired();

            this.Property(client => client.Name)
                .HasMaxLength(40);

            this.Property(client => client.Address)
                .HasMaxLength(200);

            this.Property(client => client.PostalCode)
                .HasMaxLength(50);

            this.Property(client => client.City)
                .HasMaxLength(50);

            this.Property(client => client.Email)
                .HasMaxLength(50);
        }
    }
}
