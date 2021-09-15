using Progress.Sitefinity.Renderer.Entities.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;



using System.Linq.Dynamic;
using Telerik.Sitefinity.Data;

namespace SitefinityWebApp.Custom.Helpers
{
    public class ManagerBase
    {
        public static IEnumerable<IDataItem> GetItems(MixedContentContext context, string contentType = null)
        {
            var clrType = TypeResolutionService.ResolveType(contentType, false);
            if (clrType == null)
                throw new ArgumentException(nameof(contentType));

            var collectionContextForAll = new List<IDataItem>();
            if (context != null)
            {
                foreach (var contentContext in context.Content)
                {
                    if (contentContext.Variations != null)
                    {
                        foreach (var variation in contentContext.Variations)
                        {
                            var filter = variation.Filter;
                            string serializedFilterExpression = null;

                            object filterObject = null;
                            if (!string.IsNullOrEmpty(filter.Key))
                            {
                                var filterConverterType = TypeResolutionService.ResolveType("Progress.Sitefinity.Renderer.DetailItem.Filters.FilterConverter");
                                var fromMethod = filterConverterType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).FirstOrDefault(x => x.Name == "From" && x.ReturnType == typeof(object));
                                filterObject = fromMethod.Invoke(null, new object[] { filter });

                                if (filter.Key == "Ids" && filterObject.GetType().Name == "CombinedFilter" && typeof(ILifecycleDataItemGeneric).IsAssignableFrom(clrType))
                                {
                                    var childFiltersProperty = filterObject.GetType().GetProperty("ChildFilters", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                    var childFilters = childFiltersProperty.GetValue(filterObject) as IEnumerable;
                                    var enumerator = childFilters.GetEnumerator();
                                    while (enumerator.MoveNext())
                                    {
                                        var fieldNameProp = enumerator.Current.GetType().GetProperty("FieldName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                        fieldNameProp.SetValue(enumerator.Current, nameof(ILifecycleDataItemGeneric.OriginalContentId));
                                    }
                                }
                            }

                            if (filterObject != null)
                            {
                                var filterContextType = TypeResolutionService.ResolveType("Progress.Sitefinity.Renderer.DetailItem.Filters.FilterContext");
                                var filterContextInstance = filterContextType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                                filterContextType.GetProperty("Filter").SetValue(filterContextInstance, filterObject);
                                filterContextType.GetProperty("Type").SetValue(filterContextInstance, clrType.FullName);

                                var serializerType = TypeResolutionService.ResolveType("Progress.Sitefinity.Renderer.DetailItem.Filters.CSharpDynamicLinqFilterSerializer");
                                var serializerInstance = serializerType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                                serializedFilterExpression = serializerType.GetMethod("Serialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(serializerInstance, new object[] { filterContextInstance }).ToString();
                            }

                            var providerName = variation.Source;
                            var manager = Telerik.Sitefinity.Data.ManagerBase.GetMappedManager(clrType, providerName);
                            var itemsForContext = manager.GetItems(clrType, serializedFilterExpression, null, 0, 0).AsQueryable();

                            if (typeof(ILifecycleDataItemGeneric).IsAssignableFrom(clrType))
                            {
                                itemsForContext = itemsForContext.Cast<ILifecycleDataItem>().AsQueryable().Where(x => x.Status == ContentLifecycleStatus.Live);
                                var enhanceMethod = typeof(LifecycleExtensions).GetMethod("EnhanceQueryToFilterPublished", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(typeof(ILifecycleDataItem));
                                itemsForContext = enhanceMethod.Invoke(null, new object[] { itemsForContext, SystemManager.CurrentContext.Culture }) as IQueryable;
                            }

                            foreach (var dataItem in itemsForContext.Cast<IDataItem>().ToList())
                            {
                                collectionContextForAll.Add(dataItem);
                            }
                        }
                    }
                }

                var orderedCollection = new List<IDataItem>();
                if (context.ItemIdsOrdered != null && context.ItemIdsOrdered.Length > 0)
                {
                    foreach (var id in context.ItemIdsOrdered)
                    {
                        IDataItem orderedItem;
                        if (typeof(ILifecycleDataItemGeneric).IsAssignableFrom(clrType))
                        {
                            orderedItem = collectionContextForAll.FirstOrDefault(x => (x as ILifecycleDataItemGeneric).OriginalContentId == Guid.Parse(id));
                        }
                        else
                        {
                            orderedItem = collectionContextForAll.FirstOrDefault(x => x.Id == Guid.Parse(id));
                        }


                        if (orderedItem != null)
                        {
                            orderedCollection.Add(orderedItem);
                        }
                    }

                    return orderedCollection;
                }
            }

            return collectionContextForAll;
        }
    }
}