﻿// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Reflection;
using System.Collections;
using NUnit.Core.Extensibility;

namespace NUnit.Core.Builders
{
    /// <summary>
    /// ValueSourceProvider supplies data items for individual parameters
    /// from named data sources in the test class or a separate class.
    /// </summary>
    public class ValueSourceProvider : IDataPointProvider
    {
        #region Constants
        public const string SourcesAttribute = "NUnit.Framework.ValueSourceAttribute";
        public const string SourceTypeProperty = "SourceType";
        public const string SourceNameProperty = "SourceName";
        #endregion

        #region IDataPointProvider Members

        /// <summary>
        /// Determine whether any data sources are available for a parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo test parameter</param>
        /// <returns>True if any data is available, otherwise false.</returns>
        public bool HasDataFor(ParameterInfo parameter)
        {
            return Reflect.HasAttribute(parameter, SourcesAttribute, false);
        }

        /// <summary>
        /// Return an IEnumerable providing test data for use with
        /// one parameter of a parameterized test.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
            ArrayList parameterList = new ArrayList();

            foreach ( ProviderInfo info in GetSourcesFor(parameter) )
            {
                if (info.Provider != null)
                    foreach (object o in info.Provider)
                        parameterList.Add(o);
            }

            return parameterList;
        }
        #endregion

        #region Helper Methods
        private static IList GetSourcesFor(ParameterInfo parameter)
        {
            ArrayList sources = new ArrayList();
            foreach (Attribute sourceAttr in Reflect.GetAttributes(parameter, SourcesAttribute, false))
            {
                Type sourceType = Reflect.GetPropertyValue(sourceAttr, SourceTypeProperty) as Type;
                if (sourceType == null)
                    sourceType = parameter.Member.ReflectedType;

                string sourceName = Reflect.GetPropertyValue(sourceAttr, SourceNameProperty) as string;
                if (sourceName != null && sourceName.Length > 0)
                {
                    sources.Add(new ProviderInfo(sourceType, sourceName));
                }
            }
            return sources;
        }
        #endregion
    }
}
