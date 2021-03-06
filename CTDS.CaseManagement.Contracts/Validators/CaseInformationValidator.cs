﻿namespace CTDS.CaseManagement.Contracts.Validators
{
    using CTDS.CaseManagement.Contracts.Dto;

    using FluentValidation;

    public class CaseInformationValidator : AbstractValidator<CaseInformationDto>
    {
        public CaseInformationValidator()
        {
            RuleFor(caseInformation => caseInformation.Description).MaximumLength(200);
            RuleFor(caseInformation => caseInformation.MessageFromClient).MaximumLength(200);
        }
    }
}