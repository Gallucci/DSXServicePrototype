using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    static class DMLConvert
    {
        private static StringBuilder Output;
        private static string locationGroupNumber;
        private static string udfFieldNumber;
        private static string udfFieldData;
        private static IList<Tuple<string, object>> NamesTableEntries;
        private static IList<Tuple<string, object>> CardsTableEntries;
        private static IList<Tuple<string, object>> UdfTableEntries;

        static DMLConvert()
        {
            // Initialize
            Initialize();
        }

        private static void Initialize()
        {
            Output = new StringBuilder();
            NamesTableEntries = new List<Tuple<string, object>>();
            CardsTableEntries = new List<Tuple<string, object>>();
            UdfTableEntries = new List<Tuple<string, object>>();
        }

        public static string SerializeObject(object obj)
        {
            // Re-initialize since the class is static and collections/builders will not be cleared after the first initialization in the constructor
            Initialize();

            var type = obj.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            foreach (var property in properties)
            {                
                var attributes = property.GetCustomAttributes(false);
                var dmlIdentifier = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLIdentifierComponentAttribute));                                                
                var dmlEntry = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLEntryAttribute));

                // Check if the property attribute is a DML identifier component
                if (dmlIdentifier != null)
                {
                    var toDml = dmlIdentifier as DMLIdentifierComponentAttribute;
                    var componentName = toDml.Name;
                    var componentValue = property.GetValue(obj, null);

                    switch(componentName)
                    {
                        case Component.LocationGroupNumber:
                            locationGroupNumber = componentValue.ToString();
                            break;
                        case Component.UdfFieldNumber:
                            udfFieldNumber = componentValue.ToString();
                            break;
                        case Component.UdfFieldData:
                            udfFieldData = componentValue.ToString();
                            break;
                    }
                }

                // Check if the property attribute is a DML entry
                if (dmlEntry != null)
                {
                    var toDml = dmlEntry as DMLEntryAttribute;
                    var sectionName = toDml.SectionName;
                    var entryName = toDml.EntryName;
                    var entryValue = property.GetValue(obj, null);

                    switch(sectionName)
                    {
                        case Section.Names:
                            NamesTableEntries.Add(new Tuple<string,object>(toDml.EntryName, entryValue));
                            break;
                        case Section.Cards:
                            CardsTableEntries.Add(new Tuple<string, object>(toDml.EntryName, entryValue));
                            break;
                        case Section.UDF:

                            // If the property is a type that implements a list we might have to map the properties of the objects contained in the list
                            if (entryValue is IList)
                            {
                                // Treat the property as a list and assume no DML attributes
                                var valueList = entryValue as IList;
                                var hasDMLAttributes = false;

                                // Cycle through the items in the property's list
                                foreach (var item in valueList)
                                {
                                    // Get the properties of each object in the list
                                    var childType = item.GetType();
                                    var childProperties = childType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                                    // Determine of any of properties for each object in the list have DML attrubutes associated with them
                                    foreach (var childProperty in childProperties)
                                    {
                                        var childAttributes = childProperty.GetCustomAttributes(false);
                                        var childDmlEntry = childAttributes.FirstOrDefault(a => a.GetType() == typeof(DMLEntryAttribute));
                                        
                                        // If there are DML attributes on the properties of the list object they must be mapped
                                        if (childDmlEntry != null)
                                        {
                                            hasDMLAttributes = true;
                                            var childToDml = childDmlEntry as DMLEntryAttribute;
                                            var childSectionName = childToDml.SectionName;
                                            var childEntryName = childToDml.EntryName;
                                            var childEntryValue = childProperty.GetValue(item, null);

                                            if (childSectionName == Section.UDF)
                                                UdfTableEntries.Add(new Tuple<string, object>(childToDml.EntryName, childEntryValue));
                                        }
                                    }

                                    // If the property was a list type, but its objects had no DML attributes then map the property in the standard way
                                    if(!hasDMLAttributes)
                                        UdfTableEntries.Add(new Tuple<string, object>(toDml.EntryName, entryValue));
                                }
                            }
                            else
                            {
                                // The property has DML attributes, but isn't a list so just map it in the standard way
                                UdfTableEntries.Add(new Tuple<string, object>(toDml.EntryName, entryValue));
                            }
                            break;
                    }
                }
            }

            // Begin serialization
            AddIdentifier();
            
            // Check for Names table entries
            if(NamesTableEntries.Any())
            {
                OpenTable("Names");
                foreach(var entry in NamesTableEntries)
                {
                    AddField(entry.Item1, entry.Item2);
                }
                CloseTableWithWrite();
            }

            // Check for UDF table entries
            if(UdfTableEntries.Any())
            {
                OpenTable("UDF");
                foreach (var entry in UdfTableEntries)
                {
                    AddField(entry.Item1, entry.Item2);
                }
                CloseTableWithWrite();
            }

            // Check for Cards table entries
            if (CardsTableEntries.Any())
            {
                OpenTable("Cards");
                foreach (var entry in CardsTableEntries)
                {
                    AddField(entry.Item1, entry.Item2);
                }
                CloseTableWithWrite();
            }

            return Output.ToString();
        }
        
        private static string FormatDSXDate(DateTime value)
        {
            var pattern = "M/d/yyyy HH:mm";
            return value.ToString(pattern);
        }
        private static string FormatDSXBoolean(bool value)
        {
            if (value)
                return "1";
            else
                return "0";
        }

        private static void WriteEntryLine(string name, string value)
        {
            Output.AppendLine(string.Format("F {0} ^{1}^^^", name, value));
        }

        private static void AddIdentifier()
        {
            Output.AppendLine(string.Format("I L{0} U{1} ^{2}^^^", locationGroupNumber, udfFieldNumber, udfFieldData));
        }

        private static void OpenTable(string tableName)
        {
            Output.AppendLine(string.Format("T {0}", tableName));
        }

        private static void AddField<T>(string name, T value, bool allowEmptyValue = false)
        {                        
            if (value is DateTime)
            {
                WriteEntryLine(name, FormatDSXDate((DateTime)(object)value));
            }
            else if (value is DateTime?)
            {
                if ((value as DateTime?).HasValue)
                    WriteEntryLine(name, FormatDSXDate((DateTime)(object)value));
            }
            else if (value is bool)
            {
                WriteEntryLine(name, FormatDSXBoolean((bool)(object)value));
            }
            else if (value is bool?)
            {
                if ((value as bool?).HasValue)
                    WriteEntryLine(name, FormatDSXBoolean((bool)(object)value));
            }
            else if (value is ICollection)
            {
                var list = value as ICollection;
                foreach (var item in list)
                {
                    WriteEntryLine(name, item.ToString().Trim());
                }
            }
            else
            {
                if (value != null)
                    WriteEntryLine(name, value.ToString().Trim());
            }
        }

        private static void CloseTableWithWrite()
        {
            Output.AppendLine("W");            
        }

        private static void CloseTableWithDelete()
        {
            Output.AppendLine("D");            
        }

        private static void CloseTableWithPrint()
        {
            Output.AppendLine("P");            
        }

        private static void CloseTableWithUpdate()
        {
            Output.AppendLine("U");            
        }
    }
}
