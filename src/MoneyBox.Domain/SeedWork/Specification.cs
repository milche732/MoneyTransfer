using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Moneybox.Domain.SeedWork
{
  
    public class Specification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>> _predicate;
        private readonly Func<T, bool> _predicateCompiled;

       
        public Specification(Expression<Func<T, bool>> predicate)
        {

            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _predicateCompiled = predicate.Compile();
        }

     
        public Expression<Func<T, bool>> Predicate
        {
            get { return _predicate; }
        }

     
        public bool IsSatisfiedBy(T entity)
        {
            return _predicateCompiled.Invoke(entity);
        }

        public static Specification<T> operator &(Specification<T> leftHand, Specification<T> rightHand)
        {
            InvocationExpression rightInvoke = Expression.Invoke(rightHand.Predicate,
                                                                 leftHand.Predicate.Parameters.Cast<Expression>());
            BinaryExpression newExpression = Expression.MakeBinary(ExpressionType.AndAlso, leftHand.Predicate.Body,
                                                                   rightInvoke);
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters)
                );
        }

        public static Specification<T> operator |(Specification<T> leftHand, Specification<T> rightHand)
        {
            InvocationExpression rightInvoke = Expression.Invoke(rightHand.Predicate,
                                                                 leftHand.Predicate.Parameters.Cast<Expression>());
            BinaryExpression newExpression = Expression.MakeBinary(ExpressionType.OrElse, leftHand.Predicate.Body,
                                                                   rightInvoke);
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters)
                );
        }
    }
}
