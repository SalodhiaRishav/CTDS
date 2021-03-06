﻿namespace CTDS.Database.Models.CaseManagement
{
    using System;

    using CTDS.CaseManagement.Contracts.Enums;
   

    public class CaseInformation : BaseModel
    {
        public string Description { get; set; }
        public string MessageFromClient { get; set; }
        public PriorityType Priority { get; set; }
        public Guid CaseId { get; set; }
    }
}
