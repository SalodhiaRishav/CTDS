﻿namespace CTDS.Web.Declaration
{  
    using System;

    using CTDS.Declarations.Application.Services;
    using CTDS.Declarations.Contracts.DeclarationDTO;
    using CTDS.Web.CommonResponse;   
    using CTDS.Web.Authentication.Filters;
    using CTDS.Declarations.Contracts.Interface;

    using Serilog;
    using ServiceStack.ServiceInterface;

    public class GetDeclarationByGuid : Service
    {
        private readonly IDeclarationBll DeclarationBll;
        public GetDeclarationByGuid(IDeclarationBll declarationBll)
        {
            DeclarationBll = declarationBll;
        }

        [AuthFilter(RoleName = "User")]
        public OperationResponse<DeclarationData> Get(CTDS.Declarations.Contracts.EndPoints.GetDeclarationByGuid guid)
        {
            OperationResponse<DeclarationData> response = new OperationResponse<DeclarationData>();
            try
            {
                var id = guid.Guid;
                var declarationData = DeclarationBll.GetDeclarationById(id);
                var referenceData = DeclarationBll.GetReferenceData(id);
                DeclarationData data = new DeclarationData();
                data.Declaration = declarationData;
                data.ReferenceData = referenceData;
                response.OnSuccess(data, "Declaration successfully fetched");
                return response;
            }
            catch(Exception e)
            {
                Log.Error(e.Message+" "+e.StackTrace);
                response.OnException("Failed to Fetch declaration");
                return response;
            }
        }

    }
}