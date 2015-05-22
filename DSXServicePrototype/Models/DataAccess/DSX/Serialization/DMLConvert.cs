using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static IList<Tuple<string, object>> UdfTableEntries;
        private static IList<Tuple<string, object>> ImagesTableEntries;
        private static IList<Tuple<string, object>> CardsTableEntries;        

        static DMLConvert()
        {
            // Initialize
            Initialize();
        }

        private static void Initialize()
        {
            Output = new StringBuilder();
            NamesTableEntries = new List<Tuple<string, object>>();            
            UdfTableEntries = new List<Tuple<string, object>>();
            ImagesTableEntries = new List<Tuple<string, object>>();
            CardsTableEntries = new List<Tuple<string, object>>();
        }

        public static string SerializeObject(object obj)
        {
            // Re-initialize since the class is static and collections/builders will not be cleared after the first initialization in the constructor
            Initialize();
            
            // Parse the object for properties with DML Identifier decorators
            GetDMLIdentifiersFromObject(obj);

            // Parse the object for properties with DML Entry decorators
            GetDMLEntriesFromObject(obj);

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

            // Check for Images table entries
            if (ImagesTableEntries.Any())
            {
                OpenTable("Images");
                foreach (var entry in ImagesTableEntries)
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

            // Return the serialized object
            return Output.ToString();
        }

        private static void GetDMLIdentifiersFromObject(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var dmlIdentifier = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLIdentifierComponentAttribute));

                if (dmlIdentifier != null)
                {
                    var toDml = dmlIdentifier as DMLIdentifierComponentAttribute;
                    var componentName = toDml.Name;
                    var componentValue = property.GetValue(obj, null);

                    switch (componentName)
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
            }
        }

        private static void GetDMLEntriesFromObject(object obj)
        {
            // If the object for serialization is enumerable we need to continue traversing and calling the routine until we get a non-enumerable object
            if (obj is IEnumerable)
            {
                var list = obj as IEnumerable;
                foreach (var item in list)
                {
                    GetDMLEntriesFromObject(item);
                }
            }

            // Onve we get through traversing we need to get the object's properties
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Iterate through the object's properties
            foreach (var property in properties)
            {
                // Get only the DML Entry attributes from the property in question
                var attributes = property.GetCustomAttributes(false);
                var dmlEntry = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLEntryAttribute));
                
                // If we actually have a property with a DML Entry attribute we need to examine that property
                if (dmlEntry != null)
                {
                    // Get information from the property and attribute
                    var value = property.GetValue(obj, null);
                    var attribute = dmlEntry as DMLEntryAttribute;
                    var sectionName = attribute.SectionName;
                    var entryName = attribute.EntryName;

                    // If the value of the property is an enumerable object we need to continue travering and calling the routine until get get a non-enumerable object
                    if (value is IEnumerable)
                    {
                        var valueList = value as IEnumerable;
                        foreach (var itemValue in valueList)
                        {
                            GetDMLEntriesFromObject(itemValue);
                        }
                    }

                    // Once we have a property value that isn't an enumerable object and it has a DML Entry attribute we need to store its information in the right list
                    if (entryName != null)
                    {
                        switch (sectionName)
                        {
                            case Section.Names:
                                NamesTableEntries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.UDF:
                                UdfTableEntries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.Images:
                                ImagesTableEntries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.Cards:
                                CardsTableEntries.Add(new Tuple<string, object>(entryName, value));
                                break;
                        }
                    }
                }
            }
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
