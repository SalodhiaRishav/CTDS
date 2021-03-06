﻿namespace CTDS.Security.Contracts.Interfaces
{
    using CTDS.Security.Contracts.Dto;
    using CTDS.Database.Models.Authentication;
    public interface ITokenBusinessLogic
    {
        JwtTokenData CreateJwtTokens(User user);
        void AddNewToken(User user, string accessToken, string refreshTokenSerialNumber, string refreshToken);
        void DeleteToken(string token);
        string RefreshToken(string refreshTokenSerialId);
    }
}
