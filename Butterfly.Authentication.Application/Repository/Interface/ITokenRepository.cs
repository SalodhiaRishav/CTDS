﻿namespace Butterfly.Authentication.Application.Repository.Interfaces
{
    using Butterfly.Database.Models.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface ITokenRepository
    {
        List<Token> List { get; }
        Token Add(Token entity);
        void Delete(Token entity);
        Token Update(Token entity);
        Token FindById(int Id);
        void AddRange(IEnumerable<Token> entityList);
        void DeleteRange(IEnumerable<Token> entityList);
        List<Token> Find(Expression<Func<Token, bool>> predicate);
    }
}