using Provausio.Testing.Generators.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Provausio.Testing.Generators
{
    public class ObjectFill<T> where T : class, new()
    {
        private readonly Dictionary<Expression<Func<T, object>>, FillDescriptor> _selectors = 
            new Dictionary<Expression<Func<T, object>>, FillDescriptor>();

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <param name="selector">Selects the property against which the generator will be used.</param>
        /// <param name="provider">The instance of the generator to be used on the selected property.</param>
        /// <returns></returns>
        public ObjectFill<T> For(Expression<Func<T, object>> selector, IGenerateData provider)
        {
            return For(selector, provider, null);
        }

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <param name="selector">Selects the property against which the generator will be used.</param>
        /// <param name="provider">The instance of the generator to be used on the selected property.</param>
        /// <param name="callback">Overrides the property fill with specified action.</param>
        /// <returns></returns>
        public ObjectFill<T> For(
            Expression<Func<T, object>> selector, 
            IGenerateData provider, 
            Action<T, IGenerateData> callback)
        {
            if (ContainsSelector(selector.ToString()))
                throw new ArgumentException($"Selector {selector.ToString()} already registered.");

            _selectors.Add(selector, new FillDescriptor(
                selector,
                provider,
                callback));

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
                var generator = propertySelector.Value.Generator;
                if (propertySelector.Value.HasCallback)
                {
                    RunCallback(instance, generator, propertySelector.Value.Callback);
                }
                else if (propertySelector.Key.Body is MemberExpression memberExpression)
                {
                    var property = memberExpression.Member as PropertyInfo;
                    if (property != null)
                        SetValue(instance, generator, property);
                }
                else if(propertySelector.Key.Body is UnaryExpression unaryExpression)
                {
                    var mExpr = unaryExpression.Operand as MemberExpression;
                    var property = mExpr.Member as PropertyInfo;
                    if (property != null)
                        SetValue(instance, generator, property);
                }
                else throw new Exception("Unrecognized expression type.");
            }
        }

        private void SetValue(T instance, IGenerateData generator, PropertyInfo property)
        {
            var value = generator.Generate();
            property.SetValue(instance, value);
        }

        private void RunCallback(
            T instance, IGenerateData generator, 
            Action<T, IGenerateData> callback)
        {
            callback(instance, generator);
        }

        private class FillDescriptor
        {
            public Expression<Func<T, object>> PropertySelector { get; set; }

            public IGenerateData Generator { get; set; }

            public Action<T, IGenerateData> Callback { get; set; }

            public bool HasCallback => Callback != null;

            public FillDescriptor(
                Expression<Func<T, object>> selector,
                IGenerateData generator,
                Action<T, IGenerateData> callback)
            {
                PropertySelector = selector;
                Generator = generator;
                Callback = callback;
            }
        }
    }
}
