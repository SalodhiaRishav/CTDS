﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Butterfly.web.Declaration
{
    using Butterfly.web.CommonResponse;
    using Butterfly.Declarations.Contracts.DeclarationDTO;
    using Butterfly.Declarations.Application.Services;
    using ServiceStack.ServiceInterface;

    public class GetAllDeclarations : Service
    {
        private readonly DeclarationBll declarationBll;
        public GetAllDeclarations()
        {
            declarationBll = new DeclarationBll();
        }

        public OperationResponse<IEnumerable<DeclarationDto>> GET()
        {
            OperationResponse<IEnumerable<DeclarationDto>> response = new OperationResponse<IEnumerable<DeclarationDto>>();
            try
            {
                var data = declarationBll.GetAllDeclaration();
                response.OnSuccess(data, "Declarations Fecthed Successfully");
                return response;
            }
            catch(Exception e)
            {
                response.OnException("Declaration failed to fetched");
                return response;
            }
        }
    }
}