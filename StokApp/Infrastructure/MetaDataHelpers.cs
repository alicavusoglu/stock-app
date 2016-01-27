using System;
using System.Web.Mvc;

namespace StokApp.Infrastructure
{
    public static class MetaDataHelpers
    {
        public static string GetDisplayName<TModel, TProperty>(this TModel model, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression)
        {
            return ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, new ViewDataDictionary<TModel>(model)).DisplayName;
        }
    }
}