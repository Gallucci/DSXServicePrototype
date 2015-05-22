using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    class DSXIdentifier
    {
        public int LocationGroupNumber { get; set;}
        public int UdfFieldNumber {get; set;}
        public string UdfFieldData {get; set;}

        public DSXIdentifier() {}
    
        public DSXIdentifier(int locationGroupNumber, int udfFieldNumber, string udfFieldData)
        {
            LocationGroupNumber = locationGroupNumber;
            UdfFieldNumber = udfFieldNumber;
            UdfFieldData = udfFieldData;
        }

        public override string ToString()
        {
            return string.Format("I L{0} U{1} ^{2}^^^{3}", LocationGroupNumber, UdfFieldNumber, UdfFieldData, Environment.NewLine);
        }
    }

    class DSXTable
    {
        public string Name { get; private set; }
        public IList<Tuple<string, object>> Entries {get; private set;}
        private StringBuilder Output { get; set; }

        public DSXTable(string name)
        {
            Name = name;
            Entries = new List<Tuple<string, object>>();
            Output = new StringBuilder();
        }

        public override string ToString()
        { 	 
            if(Entries.Any())
            {
                OpenTable(Name);
                foreach (var entry in Entries)
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

        private void WriteEntryLine(string name, string value) { Output.AppendLine(string.Format("F {0} ^{1}^^^", name, value)); }

        private void OpenTable(string tableName) { Output.AppendLine(string.Format("T {0}", tableName)); }

        private void AddField<T>(string name, T value, bool allowEmptyValue = false)
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

        private void CloseTableWithWrite() { Output.AppendLine("W"); }

        private void CloseTableWithDelete() { Output.AppendLine("D"); }

        private void CloseTableWithPrint() { Output.AppendLine("P"); }

        private void CloseTableWithUpdate() { Output.AppendLine("U"); }
    }

    static class DMLConvert
    {
        private static StringBuilder output;
        private static DSXIdentifier identifier;
        private static DSXTable names;
        private static DSXTable udf;
        private static DSXTable images;
        private static DSXTable cards;

        static DMLConvert()
        {
            // Initialize
            Initialize();
        }

        private static void Initialize()
        {
            output = new StringBuilder();
            identifier = new DSXIdentifier();
            names = new DSXTable("Names");
            udf = new DSXTable("UDF");
            images = new DSXTable("Images");
            cards = new DSXTable("Cards");
        }

        public static string SerializeObject(object obj)
        {
            // Re-initialize since the class is static and collections/builders will not be cleared after the first initialization in the constructor
            Initialize();
            
            // Parse the object for properties with DML Identifier decorators
            GetDMLIdentifiersComponentsFromObject(obj);

            // Parse the object for properties with DML Entry decorators
            GetDMLEntriesFromObject(obj);

            // Begin serialization
            output.Append(identifier.ToString());
            output.Append(names.ToString());
            output.Append(udf.ToString());
            output.Append(images.ToString());
            output.Append(cards.ToString());
            
            // Return the serialized object
            return output.ToString();
        }

        private static void GetDMLIdentifiersComponentsFromObject(object obj)
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
                            identifier.LocationGroupNumber = (int)componentValue;
                            break;
                        case Component.UdfFieldNumber:
                            identifier.UdfFieldNumber = (int)componentValue;
                            break;
                        case Component.UdfFieldData:
                            identifier.UdfFieldData = componentValue.ToString();
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

            // Once we get through traversing we need to get the object's properties
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Iterate through the object's properties
            foreach (var property in properties)
            {
                // Get only the DML Entry attributes from the property in question
                var attributes = property.GetCustomAttributes(false);
                var dmlEntryAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLEntryAttribute));
                
                // If we actually have a property with a DML Entry attribute we need to examine that property
                if (dmlEntryAttribute != null)
                {
                    // Get information from the property and attribute
                    var value = property.GetValue(obj, null);
                    var attribute = dmlEntryAttribute as DMLEntryAttribute;
                    var sectionName = attribute.SectionName;
                    var entryName = attribute.EntryName;

                    // If the value of the property is an enumerable object we need to continue travering and calling the routine until get get a non-enumerable object
                    if (value is IEnumerable)
                    {
                        var list = value as IEnumerable;
                        foreach (var item in list)
                        {
                            GetDMLEntriesFromObject(item);
                        }
                    }

                    // Once we have a property value that isn't an enumerable object and it has a DML Entry attribute we need to store its information in the right list
                    if (entryName != null)
                    {
                        switch (sectionName)
                        {
                            case Section.Names:
                                names.Entries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.UDF:
                                udf.Entries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.Images:
                                images.Entries.Add(new Tuple<string, object>(entryName, value));
                                break;
                            case Section.Cards:
                                cards.Entries.Add(new Tuple<string, object>(entryName, value));
                                break;
                        }
                    }
                }
            }
        }                
    }
}
