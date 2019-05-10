using Provausio.Testing.Generators.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Provausio.Testing.Generators
{
    public class ObjectFill<T> where T : class, new()
    {
        private bool _fillUnmappedProperties;
        private readonly Dictionary<Expression<Func<T, object>>, FillDescriptor> _selectors = 
            new Dictionary<Expression<Func<T, object>>, FillDescriptor>();

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <param name="selector">Selects the property against which the generator will be used.</param>
        /// <param name="generator">The instance of the generator to be used on the selected property.</param>
        /// <returns></returns>
        public ObjectFill<T> For(Expression<Func<T, object>> selector, IGenerateData generator)
        {
            return For(selector, generator, null);
        }

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <typeparam name="TGeneratorType">The type of the generator type.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public ObjectFill<T> For<TGeneratorType>(Expression<Func<T, object>> selector, IServiceProvider serviceProvider = null)
            where TGeneratorType : IGenerateData
        {
            var generator = serviceProvider == null
                ? Activator.CreateInstance<TGeneratorType>()
                : ActivatorUtilities.CreateInstance<TGeneratorType>(serviceProvider);

            return For(selector, generator);
        }

        /// <summary>
        /// Configure a property to be generated.
        /// </summary>
        /// <param name="selector">Selects the property against which the generator will be used.</param>
        /// <param name="generator">The instance of the generator to be used on the selected property.</param>
        /// <param name="callback">Overrides the property fill with specified action.</param>
        /// <returns></returns>
        public ObjectFill<T> For(
            Expression<Func<T, object>> selector, 
            IGenerateData generator, 
            Action<T, IGenerateData> callback)
        {
            if (ContainsSelector(selector.ToString()))
                throw new ArgumentException($"Selector {selector} already registered.");

            _selectors.Add(selector, new FillDescriptor(
                selector,
                generator,
                callback));

            return this;
        }

        /// <summary>
        /// Properties that have not been defined will also be filled.
        /// </summary>
        /// <returns></returns>
        public ObjectFill<T> FillUnmappedProperties()
        {
            _fillUnmappedProperties = true;
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

        private void FillObject(T instance)
        {
            var properties = instance
                .GetType()
                .GetProperties();
        }

        private void FillProperties(T instance)
        {
            var complexObjects = new List<PropertyInfo>();

            foreach (var propertySelector in _selectors)
            {
                var generator = propertySelector.Value.Generator;
                if (propertySelector.Value.HasCallback)
                {
                    RunCallback(instance, generator, propertySelector.Value.Callback);
                }
                else switch (propertySelector.Key.Body)
                {
                    case MemberExpression memberExpression:
                    {
                        var property = memberExpression.Member as PropertyInfo;
                        if (property != null && !complexObjects.Contains(property))
                            SetValue(instance, generator, property);

                        break;
                    }
                    case UnaryExpression unaryExpression:
                    {
                        if(!(unaryExpression.Operand is MemberExpression mExpr))
                            throw new Exception("Unrecognized expression type.");

                        var property = mExpr.Member as PropertyInfo;

                        if (property != null)
                            SetValue(instance, generator, property);
                        break;
                    }
                    default:
                        throw new Exception("Unrecognized expression type.");
                }
            }

            // map complex objects
        }

        private static void SetValue(T instance, IGenerateData generator, PropertyInfo property)
        {
            var value = generator.Generate();
            property.SetValue(instance, value);
        }

        private static void RunCallback(
            T instance, IGenerateData generator, 
            Action<T, IGenerateData> callback)
        {
            callback(instance, generator);
        }

        private class FillDescriptor
        {
            private Expression<Func<T, object>> PropertySelector { get; }

            public IGenerateData Generator { get; }

            public Action<T, IGenerateData> Callback { get; }

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
