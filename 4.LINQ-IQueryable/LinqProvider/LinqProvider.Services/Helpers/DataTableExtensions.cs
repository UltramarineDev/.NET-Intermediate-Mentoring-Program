using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LinqProvider.Services.Helpers
{
    public static class DataTableExtensions
    {
        public static IList ToList(this DataTable table, Type type)
        {
            var genericListType = typeof(List<>).MakeGenericType(type);
            var objectsList =  (IList)Activator.CreateInstance(genericListType);

            try
            {
                foreach (var row in table.AsEnumerable())
                {
                    var obj = Activator.CreateInstance(type);
                    var propertyInfos = obj.GetType().GetProperties();
                    
                    SetPropertyValues(propertyInfos, obj, row);
                    
                    objectsList.Add(obj);
                }

                return objectsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return objectsList;
            }
        }
        
        private static void SetPropertyValues(IEnumerable<PropertyInfo> propertyInfos, object obj, DataRow row)
        {
            foreach (var prop in propertyInfos)
            {
                try
                {
                    var columnName = prop.Name;
                    var propertyInfo = obj.GetType().GetProperty(columnName);
                    propertyInfo?.SetValue(obj, Convert.ChangeType(row[columnName], propertyInfo.PropertyType), null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
