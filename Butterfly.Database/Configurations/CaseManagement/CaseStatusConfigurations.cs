﻿namespace Butterfly.Database.Configurations.CaseManagement
{
    using Butterfly.Database.Models.CaseManagement;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;

    class CaseStatusConfigurations : EntityTypeConfiguration<CaseStatus>
    {
        public CaseStatusConfigurations()
        {
            this.HasKey(caseStatus=>caseStatus.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
