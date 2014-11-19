//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Linq.Dynamic;
//using System.Linq.Expressions;
//using Kore.Collections;
//using Kore.Data;
//using Kore.Data.Services;
//using Kore.Linq;

//namespace Kore.Web.Mvc.JQGrid
//{
//    public interface IJQGridDataService<TEntity> : IGenericDataService<TEntity> where TEntity : class
//    {
//        IOrderedQueryable<TEntity> ApplyDefaultSort(IQueryable<TEntity> queryable);

//        IEnumerable<TEntity> GetRecords(
//            JQGridAjaxRequest request,
//            out int totalRecordsCount,
//            Expression<Func<TEntity, bool>> predicate = null,
//            params Expression<Func<TEntity, dynamic>>[] includePaths);
//    }

//    public abstract class JQGridDataService<TEntity> : GenericDataService<TEntity>, IJQGridDataService<TEntity> where TEntity : class
//    {
//        public JQGridDataService(IRepository<TEntity> repository)
//            : base(repository)
//        {
//        }

//        public abstract IOrderedQueryable<TEntity> ApplyDefaultSort(IQueryable<TEntity> queryable);

//        public IEnumerable<TEntity> GetRecords(
//            JQGridAjaxRequest request,
//            out int totalRecordsCount,
//            Expression<Func<TEntity, bool>> predicate = null,
//            params Expression<Func<TEntity, dynamic>>[] includePaths)
//        {
//            var queryable = Repository.Table;

//            if (!string.IsNullOrEmpty(request.SortColumn))
//            {
//                queryable = request.SortOrder == "asc" ?
//                    queryable.OrderBy(request.SortColumn) :
//                    queryable.OrderBy(request.SortColumn + " DESC");
//            }
//            else
//            {
//                queryable = ApplyDefaultSort(queryable);
//            }

//            if (includePaths != null && includePaths.Length > 0)
//            {
//                queryable = includePaths.Aggregate(queryable, (current, path) => current.Include(path));
//            }

//            // Filtering
//            if (request.Filter != null && request.Filter.Rules.Any())
//            {
//                var expression = GetFilters(request.Filter.Rules);
//                queryable = queryable.Where(expression);
//            }

//            if (predicate != null)
//            {
//                queryable = queryable.AsExpandable().Where(predicate);
//            }

//            totalRecordsCount = queryable.Count();

//            var items = queryable
//                .Skip((request.PageIndex - 1) * request.PageSize)
//                .Take(request.PageSize)
//                .ToHashSet();

//            return items;
//        }

//        private static Expression<Func<TEntity, bool>> GetFilters(IEnumerable<Rule> rules)
//        {
//            var expression = PredicateBuilder.True<TEntity>();
//            return rules.Aggregate(expression, (current, filter) => current.And(GetFilter(filter)));
//        }

//        private static Expression<Func<TEntity, bool>> GetFilter(Rule rule)
//        {
//            var parameter = Expression.Parameter(typeof(TEntity), "x");
//            var property = Expression.PropertyOrField(parameter, rule.Field);

//            switch (rule.Operator)
//            {
//                case "eq":
//                    {
//                        if (property.Type == typeof(string))
//                        {
//                            var value = rule.Value.ToLower();
//                            var valueConstant = Expression.Constant(value);
//                            var method = typeof(string).GetMethod("ToLower", new Type[] { });
//                            var methodCallExpression = Expression.Call(property, method);
//                            return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(methodCallExpression, valueConstant), parameter);
//                        }
//                        else
//                        {
//                            var value = Convert.ChangeType(rule.Value, property.Type);
//                            var valueConstant = Expression.Constant(value);
//                            return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property, valueConstant), parameter);
//                        }
//                    }
//                case "ne": return null;
//                case "lt": return null;
//                case "le": return null;
//                case "gt": return null;
//                case "ge": return null;
//                case "bw": return null;
//                case "bn": return null;

//                case "in":
//                    {
//                        var type = typeof(List<>).MakeGenericType(property.Type);
//                        var values = rule.Value.Split(',').Select(x => x.ConvertTo(property.Type)).ToList();
//                        var containsMethod = type.GetMethod("Contains", new[] { property.Type });
//                        var valueConstant = Expression.Constant(values, type);
//                        var methodCallExpression = Expression.Call(valueConstant, containsMethod, new Expression[] { property });
//                        return Expression.Lambda<Func<TEntity, bool>>(methodCallExpression, parameter);
//                    }

//                case "ni": return null;
//                case "ew": return null;
//                case "en": return null;

//                case "cn":
//                    {
//                        var value = Expression.Constant(rule.Value);
//                        var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[] { });
//                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
//                        var methodCallExpression = Expression.Call(property, toLowerMethod, new Expression[] { });
//                        methodCallExpression = Expression.Call(methodCallExpression, containsMethod, new Expression[] { value });
//                        return Expression.Lambda<Func<TEntity, bool>>(methodCallExpression, parameter);
//                    }

//                case "nc": return null;

//                default: throw new ArgumentOutOfRangeException();
//            }
//        }
//    }
//}