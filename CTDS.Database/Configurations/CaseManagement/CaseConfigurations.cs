﻿namespace CTDS.Database.Configurations.CaseManagement
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using CTDS.Database.Models.CaseManagement;
    

    public class CaseConfigurations : EntityTypeConfiguration<Case>
    {
        public CaseConfigurations()
        {
            this.HasKey(c => c.Id);
            //this.Property(c=>c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.CaseId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasMany(c => c.CaseReferences)
                .WithRequired(caseReference => caseReference.Case)
                .HasForeignKey(caseReference => caseReference.CaseId);
        }
    }
}
