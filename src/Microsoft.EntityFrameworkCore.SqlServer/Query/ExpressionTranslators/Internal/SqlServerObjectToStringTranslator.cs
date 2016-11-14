﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Query.Expressions;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.IMethodCallTranslator" />
    public class SqlServerObjectToStringTranslator : IMethodCallTranslator
    {
        /// <summary>
        ///     Translates the given method call expression.
        /// </summary>
        /// <param name="methodCallExpression">The method call expression.</param>
        /// <returns>
        ///     A SQL expression representing the translated MethodCallExpression.
        /// </returns>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.Name == nameof(ToString))
            {
                return new SqlFunctionExpression(
                    functionName: "CONVERT",
                    returnType: methodCallExpression.Type,
                    arguments: new[]
                    {
                        new SqlFragmentExpression(GetConvertBase(methodCallExpression.Object?.Type.Name)),
                        methodCallExpression.Object
                    });
            }
            return null;
        }

        private string GetConvertBase(string typeName)
        {
            switch (typeName)
            {
                case nameof(Int64):
                    return "VARCHAR(20)";
                case nameof(Int32):
                    return "VARCHAR(10)";
                case nameof(Int16):
                    return "VARCHAR(5)";
                default:
                    return "VARCHAR(MAX)";
            }
        }
    }
}