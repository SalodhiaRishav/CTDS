﻿namespace Butterfly.Authentication.Application.Repository.Interfaces
{
    using Butterfly.Database.Models.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    public interface IRoleRepository
    {
        List<Role> List { get; }
        Role Add(Role entity);
        void Delete(Role entity);
        Role Update(Role entity);
        Role FindById(int Id);
        void AddRange(IEnumerable<Role> entityList);
        void DeleteRange(IEnumerable<Role> entityList);
        List<Role> Find(Expression<Func<Role, bool>> predicate);
    }
}