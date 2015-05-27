using DSXServicePrototype.Models.DataAccess.DSX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype
{
    static class IOHelper
    {
        public static void WriteObjectDMLProperties<T>(T obj)
        {
            var type = obj.GetType();
            var typeName = type.Name;
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var dmlIdentifier = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLIdentifierAttribute));
                var dmlField = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLFieldAttribute));
                var propertyName = property.Name;

                if (dmlIdentifier != null)
                {                    
                    var attribute = dmlIdentifier as DMLIdentifierAttribute;
                    var componentName = attribute.ComponentName;                                        
                    var propertyValue = property.GetValue(obj, null);

                    Console.WriteLine(string.Format("{0} : DMLIdentifier => [{1}] : Property => [{2}] [{3}]", typeName, componentName, propertyName, propertyValue));
                }

                if (dmlField != null)
                {
                    var attribute = dmlField as DMLFieldAttribute;                    
                    var tableName = attribute.TableName;
                    var fieldName = attribute.FieldName;                    
                    var propertyValue = property.GetValue(obj, null);

                    Console.WriteLine(string.Format("{0} : DMLEntry => [{1}] [{2}] : Property => [{3}] [{4}]", typeName, tableName, fieldName, propertyName, propertyValue));
                }
            }
        }
    }
}
