using Provausio.Testing.Generators.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Provausio.Testing.Generators
{
    public class ObjectFill<T> 
        where T : class, new()
    {
        private Dictionary<Expression<Func<T, object>>, IGenerateData> _selectors = new Dictionary<Expression<Func<T, object>>, IGenerateData>();

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <param name="propertySelector">Selects the property against which the generator will be used.</param>
        /// <param name="provider">The instance of the generator to be used on the selected property.</param>
        /// <returns></returns>
        public ObjectFill<T> For(Expression<Func<T, object>> propertySelector, IGenerateData provider)
        {
            if(ContainsSelector(propertySelector.ToString()))
                throw new ArgumentException($"Selector {propertySelector.ToString()} already registered.");

            _selectors.Add(propertySelector, provider);

            return this;
        }

        /// <summary>
        /// Generates the objects using the current configuration.
        /// </summary>
        /// <param name="count">The number of objects to generate.</param>
        /// <returns></returns>
        public IEnumerable<T> Generate(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var instance = new T();
                FillProperties(instance);
                yield return instance;
            }
        }

        private bool ContainsSelector(string selector)
        {
            var selectors = _selectors.Keys.Select(k => k.ToString());
            return selectors.Contains(selector, StringComparer.OrdinalIgnoreCase);
        }

        private void FillProperties(T instance)
        {
            foreach (var propertySelector in _selectors)
            {
                var generator = propertySelector.Value;
                if (propertySelector.Key.Body is MemberExpression memberExpression)
                {
                    var property = memberExpression.Member as PropertyInfo;
                    if (property != null)
                    {
                        
                        var value = generator.Generate();
                        property.SetValue(instance, value);
                    }
                }
                else if(propertySelector.Key.Body is UnaryExpression unaryExpression)
                {
                    var mExpr = unaryExpression.Operand as MemberExpression;
                    var property = mExpr.Member as PropertyInfo;
                    if (property != null)
                    {
                        var value = generator.Generate();
                        property.SetValue(instance, value);
                    }
                }
                else throw new Exception("Unrecognized expression type.");
            }
        }
    }
}
