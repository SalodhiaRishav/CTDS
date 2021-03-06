﻿namespace CTDS.CaseManagement.Application.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Linq.Dynamic;

    using CTDS.CaseManagement.Application.Repository.Interfaces;
    using CTDS.Database.Context;
    using CTDS.Database.Models.CaseManagement;
    using CTDS.Common.ExtensionMethods;
    using CTDS.CaseManagement.Contracts.Enums;
    using CTDS.CaseManagement.Contracts.Dto;

    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        public int FindCaseCount()
        {
            try
            {
                using (var context = new CTDSContext())
                {
                    return context.Case.Count();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Dictionary<string,int> FindFilteredCaseCount()
        {
            try
            {
                using (var context = new CTDSContext())
                {
                    var a = context.CaseStatus.AsQueryable()
                    .Join(context.CaseInformation.AsQueryable(),
                    cs => cs.CaseId, ci => ci.CaseId,
                    (cs, ci) => new { cs.Status, ci.Priority })
                    .GroupBy(cs => cs.Status).ToList();

                    var filteredList = new Dictionary<string, int>();
                    foreach (var item in a)
                    {
                        var processing = item.GroupBy(x => x.Priority).ToList();
                        foreach (var priority in processing)
                        {
                            string status = item.Key.ToString();
                            string key = status + priority.Key.ToString();

                            filteredList.Add(key, priority.Count());
                        }
                    }
                    return filteredList;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public int FindCasesInLastSevenDays()
        {
            DateTime date = DateTime.Now.AddDays(-6);
            try
            {
                using (var context = new CTDSContext())
                {
                    return context.Case.Where(c => c.CreatedOn >= date).Count();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<int> FindCasesPerDayLastWeek()
        {
            List<int> caseCountList = new List<int>();

            DateTime date = DateTime.Now.AddDays(-6);
            try
            {
                using (var context = new CTDSContext())
                {
                    for (DateTime i = date.Date; i <= DateTime.Now.Date; i = i.AddDays(1).Date)
                    {
                        int count = context.Case.Where(c => c.CreatedOn == i).Count();
                        caseCountList.Add(count);
                    }
                    return caseCountList;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public List<Case> GetAllFilteredCases(int index, string sort)
        {
            List<Case> cases;
            try
            {
                using (var context = new CTDSContext())
                {
                    int maxRows = 3;
                    var query = context.Case;
                    cases = query.CustomOrderBy(sort).Skip((index - 1) * maxRows).Take(maxRows).ToList();
                }
                return cases;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OpenCasesDto GetAllCasesWithQuery(List<QueryDto> queries,int pageNumber,int maxRowsPerPage,string sortBy,bool sortDesc)
        {
            try
            {
                using (var context = new CTDSContext())
                {
                    var expression = QueryBuilder(queries);
                    var filteredCaseTableDtos = (from mcase in context.Case
                                                 join caseStatus in context.CaseStatus on mcase.Id equals caseStatus.CaseId
                                                 join caseInformation in context.CaseInformation on mcase.Id equals caseInformation.CaseId
                                                 join client in context.Client on mcase.Id equals client.CaseId
                                                 join notes in context.Notes on mcase.Id equals notes.CaseId
                                                 select new CaseTableDto
                                                 {
                                                     Id = mcase.Id,
                                                     CaseId = mcase.CaseId,
                                                     Status = caseStatus.Status,
                                                     Priority = caseInformation.Priority,
                                                     CreatedOn = mcase.CreatedOn,
                                                     Description = caseInformation.Description,
                                                     Client = client.ClientIdentifier,
                                                     Notes = notes.NotesByCpa
                                                 })
                                .Where(expression)
                                .ToList();
                    var sortedList = new List<CaseTableDto>();
                    
                    if(sortDesc)
                    {
                        sortedList = filteredCaseTableDtos.OrderByDescending(a => a.GetType().GetProperty(sortBy).GetValue(a, null)).ToList();
                    }
                    else
                    {
                        sortedList = filteredCaseTableDtos.OrderBy(a => a.GetType().GetProperty(sortBy).GetValue(a, null)).ToList();
                    }

                    var perPageFilteredCaseTableDtos = sortedList.Skip((pageNumber - 1) * maxRowsPerPage).Take(maxRowsPerPage).ToList();
                    OpenCasesDto openCasesDto = new OpenCasesDto()
                    {
                        TotalCount = filteredCaseTableDtos.Count,
                        Cases = perPageFilteredCaseTableDtos
                    };

                    return openCasesDto;
                }
              
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //TODO making generic using  private Expression<Func<TClass,bool>> QueryBuilder<TClass>(string query)
        private Expression<Func<CaseTableDto,bool>> QueryBuilder(List<QueryDto> queries)
        {
            Expression finalBody = Expression.Constant(true);
            var parameter = Expression.Parameter(typeof(CaseTableDto), "selectedCase");
            if (queries != null)
            {
                for (int i = 0; i < queries.Count; ++i)
                {
                    var propertyName = queries[i].Property;
                    if (queries[i].Values == null || queries[i].Values.Count == 0)
                    {
                        continue;
                    }

                    var constantValues = queries[i].Values;
                    Expression orBody = Expression.Constant(false);
                    if (queries[i].ValueDataType == "StringMatch")
                    {
                        var constantValue = constantValues[0];
                        var member = Expression.Property(parameter, propertyName);
                        ConstantExpression constant = Expression.Constant(constantValue);
                        var body = Expression.Equal(member, constant);
                        orBody = Expression.OrElse(body, orBody);
                    }
                    else orBody = RangeQuery(queries, parameter, i, propertyName, constantValues, orBody);
                    finalBody = Expression.AndAlso(orBody, finalBody);
                }
            }
            
            var finalExpression= Expression.Lambda<Func<CaseTableDto, bool>>(finalBody, parameter);
            return finalExpression;
        }

        private Expression RangeQuery(List<QueryDto> queries, ParameterExpression parameter, int i, string propertyName, List<string> constantValues, Expression orBody)
        {

            if (queries[i].ValueDataType == "EnumRange")
            {
                for (int j = 0; j < constantValues.Count; ++j)
                {
                    var constantValue = constantValues[j];
                    var member = Expression.Property(parameter, propertyName);
                    var instance = Activator.CreateInstance(member.Type);
                    var enumType = Enum.Parse(instance.GetType(), constantValue);
                    ConstantExpression constant = Expression.Constant(enumType);
                    var body = Expression.Equal(member, constant);
                    orBody = Expression.OrElse(body, orBody);
                }
            }
            else if (queries[i].ValueDataType == "DateRange")
            {
                var fromDate = DateTime.ParseExact(constantValues[0], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var toDate = DateTime.ParseExact(constantValues[1], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var member = Expression.Property(parameter, propertyName);
                ConstantExpression fromDateConstant = Expression.Constant(fromDate);
                var fromDateBody = Expression.GreaterThanOrEqual(member, fromDateConstant);
                ConstantExpression toDateConstant = Expression.Constant(toDate);
                var toDateBody = Expression.LessThanOrEqual(member, toDateConstant);
                orBody = Expression.AndAlso(fromDateBody, toDateBody);
            }

            return orBody;
        }
        public List<CaseTableDto> GetAllCasesByStatus(CaseStatusType? status, DateTime startDate, DateTime endDate)
        {

            List<CaseTableDto> cases = new List<CaseTableDto>();
            try
            {
                using (var context = new CTDSContext())
                {
                    if (status == null)
                    {
                        cases = (from mcase in context.Case
                                 join caseStatus in context.CaseStatus on mcase.Id equals caseStatus.CaseId
                                 join caseInformation in context.CaseInformation on mcase.Id equals caseInformation.CaseId
                                 join client in context.Client on mcase.Id equals client.CaseId
                                 join notes in context.Notes on mcase.Id equals notes.CaseId
                                 select new CaseTableDto
                                 {
                                     Id = mcase.Id,
                                     CaseId = mcase.CaseId,
                                     Status = caseStatus.Status,
                                     Priority = caseInformation.Priority,
                                     CreatedOn = mcase.CreatedOn,
                                     Description = caseInformation.Description,
                                     Client = client.ClientIdentifier,
                                     Notes = notes.NotesByCpa
                                 })
                                .Where(c => startDate <= c.CreatedOn && c.CreatedOn <= endDate)
                                .ToList();
                        return cases;
                    }
                    cases = (from mcase in context.Case
                             join caseStatus in context.CaseStatus on mcase.Id equals caseStatus.CaseId
                             join caseInformation in context.CaseInformation on mcase.Id equals caseInformation.CaseId
                             join client in context.Client on mcase.Id equals client.CaseId
                             join notes in context.Notes on mcase.Id equals notes.CaseId
                             select new CaseTableDto
                             {
                                 Id = mcase.Id,
                                 CaseId = mcase.CaseId,
                                 Status = caseStatus.Status,
                                 Priority = caseInformation.Priority,
                                 CreatedOn = mcase.CreatedOn,
                                 Description = caseInformation.Description,
                                 Client = client.ClientIdentifier,
                                 Notes = notes.NotesByCpa
                             })
                             .Where(c => c.Status == status && startDate <= c.CreatedOn && c.CreatedOn <= endDate)
                             .ToList();
                    return cases;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CaseChartDataDto> GetCaseChartData(CaseStatusType? status, DateTime startDate, DateTime endDate)
        {
            var cases= GetAllCasesByStatus(status, startDate, endDate);
            var groupedByDateCases = cases.GroupBy(c => c.CreatedOn.Date).ToList();
            var caseChartDataDtos = new List<CaseChartDataDto>();
            foreach (var @case in groupedByDateCases)
            {
                CaseChartDataDto caseChartDataDto = new CaseChartDataDto() { Count = @case.Count(), Date = @case.Key };
                caseChartDataDtos.Add(caseChartDataDto);
            }

            var result = caseChartDataDtos.OrderBy(data => data.Date).ToList();
            return result;
        }

    }
}