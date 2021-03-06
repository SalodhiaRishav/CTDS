﻿namespace CTDS.Security.Application.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using CTDS.Database.Models.Authentication;
    
    public interface IRoleRepository
    {
        List<Role> List { get; }
        Role Add(Role entity);
        void Delete(Role entity);
        Role Update(Role entity);
        Role FindById(int id);
        void AddRange(IEnumerable<Role> entityList);
        void DeleteRange(IEnumerable<Role> entityList);
        List<Role> Find(Expression<Func<Role, bool>> predicate);
    }
}