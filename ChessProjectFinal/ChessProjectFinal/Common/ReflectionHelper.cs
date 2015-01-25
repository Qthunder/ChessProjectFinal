using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ChessProjectFinal.Common
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Extracts the property name from an expression. This allows for using the property rather than hard-coding a literal with the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression">The expression representing the property</param>
        /// <returns>The name of the property</returns>
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            return property.Name;
        }
    }
}
