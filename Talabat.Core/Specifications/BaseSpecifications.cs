﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderDescending { get; set ; }
		public int Take { get; set; }
		public int Skip { get; set; }
		public bool IsPaginationEnabled { get; set; }

		public BaseSpecifications()
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecifications(Expression<Func<T , bool>> criteria)
        {
            Criteria = criteria;
			//Includes = new List<Expression<Func<T, object>>>();
		}
		public void AddOrderBy(Expression<Func<T , object>> expression)
		{
			OrderBy = expression;
		}
		public void AddOrderByDescending(Expression<Func<T , object>> expression)
		{
			OrderDescending = expression;
		}
		public void ApplyPagination(int skip , int take)
		{
			IsPaginationEnabled = true;
			Take = take;
			Skip = skip;
		}
    }
}
