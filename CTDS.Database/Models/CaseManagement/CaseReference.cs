﻿namespace CTDS.Database.Models.CaseManagement
{
    using System;

    using CTDS.CaseManagement.Contracts.Enums;
    
    public class CaseReference : BaseModel
    {
        public CaseReferenceType Type { get; set; }
        public string Identity { get; set; }
        public string Comment { get; set; }
        public Case Case { get; set; }
        public Guid CaseId { get; set; }
    }
}
