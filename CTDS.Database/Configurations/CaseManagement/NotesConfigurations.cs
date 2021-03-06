﻿namespace CTDS.Database.Configurations.CaseManagement
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;

    using CTDS.Database.Models.CaseManagement;
   

    public class NotesConfigurations : EntityTypeConfiguration<Notes>
    {
        public NotesConfigurations()
        {
            this.HasKey(notes => notes.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(notes => notes.NotesByCpa)
                .HasMaxLength(200);
        }
    }
}
