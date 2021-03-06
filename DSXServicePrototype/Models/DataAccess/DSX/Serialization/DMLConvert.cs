﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.DataAccess.DSX.Serialization
{
    /// <summary>
    /// Static class that attempts to serialize an object into the DSX Markup Language (DML) using DML decorator attributes
    /// </summary>
    static class DMLConvert
    {
        private static StringBuilder output;
        private static DSXIdentifier identifier;
        private static DSXTable names;
        private static DSXTable udf;
        private static DSXTable images;
        private static DSXTable cards;
        private static IList<string> namesSortOrder;
        private static IList<string> cardsSortOrder;

        /// <summary>
        /// Static class constructor.
        /// </summary>
        static DMLConvert()
        {
            // Initialize
            Initialize();
        }

        /// <summary>
        /// Initializes the identifier, tables, and output
        /// </summary>
        private static void Initialize()
        {
            output = new StringBuilder();
            identifier = new DSXIdentifier();
            names = new DSXTable("Names");
            udf = new DSXTable("UDF");
            images = new DSXTable("Images");
            cards = new DSXTable("Cards");

            // Defines the field order for the Names table (Fields not in this list come last)
            // Case sensitive!
            namesSortOrder = new List<string>()
            {
                "FName",
                "LName",
                "Company",
                "Visitor",
                "Trace",
                "Notes"
            };
            
            // Defines the field order for the Cards table (Fields not in this list come last)
            // The "Code" field is the only field that *must* come first in the Cards table, the rest
            // are for aesthetics/convenience.
            // Case sensitive!
            cardsSortOrder = new List<string>() 
            {
                "Code",
                "ReplaceCode",
                "Copy2Code",
                "PIN",
                "StartDate",
                "StopDate",
                "CardNum",
                "NumUses",
                "GTour",
                "APB",
                "ClearAcl",
                "ClearTempAcl",
                "AddAcl",
                "AddTempAcl",
                "AclStartDate",
                "AclStopDate",
                "DelAcl",
                "DelTempAcl",
                "Loc",
                "OLL",
                "Notes"
            };
        }

        /// <summary>
        /// Serializes an object into DML format.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>A string output in DML format.</returns>
        public static string SerializeObject(object obj)
        {
            // Re-initialize since the class is static and collections/builders will not be cleared after the first initialization in the constructor
            Initialize();
            
            // Parse the object for properties with DML Identifier decorators
            GetDMLIdentifiersFromObject(obj);

            // Parse the object for properties with DML Field decorators
            GetDMLFieldsFromObject(obj);

            // Sort tables that require sorting
            names.Sort(namesSortOrder);
            cards.Sort(cardsSortOrder);

            // Begin serialization
            output.Append(identifier.ToString());                        
            output.Append(names.ToString());            
            output.Append(udf.ToString());
            output.Append(images.ToString());                        
            output.Append(cards.ToString());
            
            // Return the serialized object
            return output.ToString();
        }

        /// <summary>
        /// Examines an object for DML Identifier attributes and places their data into a DSX Identifier
        /// </summary>
        /// <param name="obj">The object from which to pull DML Identifier information.</param>
        private static void GetDMLIdentifiersFromObject(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var dmlIdentifier = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLIdentifierAttribute));

                if (dmlIdentifier != null)
                {
                    var attribute = dmlIdentifier as DMLIdentifierAttribute;
                    var componentName = attribute.ComponentName;
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

        /// <summary>
        /// Examines an object for DML Field attributes and places their data into the appropriate DSX Table
        /// </summary>
        /// <param name="obj">The object from which to pull DML Field information.</param>
        private static void GetDMLFieldsFromObject(object obj)
        {
            // If the object for serialization is enumerable we need to continue traversing and calling the routine until we get a non-enumerable object
            if (obj is IEnumerable)
            {
                var list = obj as IEnumerable;
                foreach (var item in list)
                {
                    GetDMLFieldsFromObject(item);
                }
            }

            // Once we get through traversing we need to get the object's properties
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Iterate through the object's properties
            foreach (var property in properties)
            {
                // Get only the DML Field attributes from the property in question
                var attributes = property.GetCustomAttributes(false);
                var dmlFieldAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(DMLFieldAttribute));
                
                // If we actually have a property with a DML Entry attribute we need to examine that property
                if (dmlFieldAttribute != null)
                {
                    // Get information from the property and attribute
                    var value = property.GetValue(obj, null);
                    var attribute = dmlFieldAttribute as DMLFieldAttribute;
                    var tableName = attribute.TableName;
                    var fieldName = attribute.FieldName;

                    // If the value of the property is an enumerable object we need to continue travering and calling the routine until get get a non-enumerable object
                    if (value is IEnumerable)
                    {
                        var list = value as IEnumerable;
                        foreach (var item in list)
                        {
                            // Recursively call the Get
                            GetDMLFieldsFromObject(item);
                        }
                    }

                    // Once we have a property value that isn't an enumerable object and it has a DML Entry attribute we need to store its information in the right list
                    if (fieldName != null)
                    {
                        switch (tableName)
                        {
                            case TableName.Names:                                
                                names.Entries.Add(new Entry(fieldName, value));
                                break;
                            case TableName.UDF:                                
                                udf.Entries.Add(new Entry(fieldName, value));
                                break;
                            case TableName.Images:                                
                                images.Entries.Add(new Entry(fieldName, value));
                                break;
                            case TableName.Cards:                                
                                cards.Entries.Add(new Entry(fieldName, value));
                                break;
                        }
                    }
                }
            }            
        }                
    }

    /// <summary>
    /// Helper class that represents the Identifier portion at the top of every DSX request.
    /// </summary>
    class DSXIdentifier
    {
        public int LocationGroupNumber { get; set; }
        public int UdfFieldNumber { get; set; }
        public string UdfFieldData { get; set; }

        public DSXIdentifier() { }

        /// <summary>
        /// Constructs a representation of a DSX Identifier for DML export.
        /// </summary>
        /// <param name="locationGroupNumber">The group location number used to identify a card holder in DSX.</param>
        /// <param name="udfFieldNumber">The number of the user-defined field used to identify a card holder in DSX.</param>
        /// <param name="udfFieldData">The data contained within the user-defined field used to identify a card holder in DSX.</param>
        public DSXIdentifier(int locationGroupNumber, int udfFieldNumber, string udfFieldData)
        {
            LocationGroupNumber = locationGroupNumber;
            UdfFieldNumber = udfFieldNumber;
            UdfFieldData = udfFieldData;
        }

        /// <summary>
        /// Gets the DML output of the DSX Identifier.
        /// </summary>
        /// <returns>The string output of the DSX Identifier in DML format.</returns>
        public override string ToString()
        {
            return string.Format("I L{0} U{1} ^{2}^^^{3}", LocationGroupNumber, UdfFieldNumber, UdfFieldData, Environment.NewLine);
        }
    }

    /// <summary>
    /// Helper class that represents a table in a DSX request.
    /// </summary>
    class DSXTable
    {
        public string Name { get; private set; }        
        public IList<Entry> Entries { get; private set; }
        private StringBuilder Output { get; set; }        

        /// <summary>
        /// Constructs a representation of a DSX Table for DML export.
        /// </summary>
        /// <param name="name"></param>
        public DSXTable(string name)
        {
            Name = name;            
            Entries = new List<Entry>();
            Output = new StringBuilder();
        }

        /// <summary>
        /// Gets the DML output of the DSX Table.
        /// </summary>
        /// <returns>The string output of the DSX Table in DML format.</returns>
        public override string ToString()
        {
            if (Entries.Any())
            {                                                                
                OpenTable(Name);
                foreach (var entry in Entries)
                {
                    AddField(entry.Name, entry.Value);
                }
                CloseTableWithWrite();
            }

            return Output.ToString();
        }

        /// <summary>
        /// Sorts the Entries by field name (case sensitive) as prescribed by an order list.
        /// </summary>
        /// <param name="sortOrder">The list defining the order in which the table entries should be displayed.</param>
        public void Sort(IList<string> sortOrder)
        {
            if (Entries.Any())
            {                
                (Entries as List<Entry>).Sort(new EntryComparer(sortOrder));
            }
        }

        /// <summary>
        /// Formats a DateTime into a value acceptable by DSX.
        /// </summary>
        /// <param name="value">The date/time to be formatted.</param>
        /// <returns>The string output of the date/time in DML format.</returns>
        private string FormatDSXDate(DateTime value)
        {
            var pattern = "M/d/yyyy HH:mm";
            return value.ToString(pattern);
        }

        /// <summary>
        /// Formats a boolean into a value acceptable by DSX.
        /// </summary>
        /// <param name="value">The boolean to be formatted.</param>
        /// <returns>The string output of the boolean in DML format.</returns>
        private string FormatDSXBoolean(bool value)
        {
            if (value)
                return "1";
            else
                return "0";
        }

        /// <summary>
        /// Adds a DSX field entry in DML format to the output.
        /// </summary>
        /// <param name="name">The field name of the entry.</param>
        /// <param name="value">The field value of the entry.</param>
        private void WriteField(string name, string value) { Output.AppendLine(string.Format("F {0} ^{1}^^^", name, value)); }

        /// <summary>
        /// Adds a DSX open table entry in DML format to the format.
        /// </summary>
        /// <param name="tableName">The name of the table to open.</param>
        private void OpenTable(string tableName) { Output.AppendLine(string.Format("T {0}", tableName)); }

        /// <summary>
        /// Handles the field entry values so they are properly added to the DML output.
        /// </summary>
        /// <typeparam name="T">The type of the value to be added to the DML output.</typeparam>
        /// <param name="name">The name of the field to be added to the DML output.</param>
        /// <param name="value">The value of the field to be added to the DML output.</param>
        /// <param name="isAllowedToNoValue">If true, the field will still be added to the DML output even if the value is null.</param>
        private void AddField<T>(string name, T value, bool isAllowedToNoValue = false)
        {
            if (value is DateTime)
            {
                WriteField(name, FormatDSXDate((DateTime)(object)value));
            }
            else if (value is DateTime?)
            {
                if ((value as DateTime?).HasValue)
                    WriteField(name, FormatDSXDate((DateTime)(object)value));
            }
            else if (value is bool)
            {
                WriteField(name, FormatDSXBoolean((bool)(object)value));
            }
            else if (value is bool?)
            {
                if ((value as bool?).HasValue)
                    WriteField(name, FormatDSXBoolean((bool)(object)value));
            }
            else if (value is ICollection)
            {
                var list = value as ICollection;
                foreach (var item in list)
                {
                    WriteField(name, item.ToString().Trim());
                }
            }
            else
            {
                if (value != null)
                    WriteField(name, value.ToString().Trim());
            }
        }

        /// <summary>
        /// Adds a DSX Close Table and Write Data to DSX entry in DML format to the format.
        /// </summary>
        private void CloseTableWithWrite() { Output.AppendLine("W"); }

        /// <summary>
        /// Adds a DSX Close Table and Delete Data from DSX entry in DML format to the format.
        /// </summary>
        private void CloseTableWithDelete() { Output.AppendLine("D"); }

        /// <summary>
        /// Adds a DSX Close Table and Print Data in DSX entry in DML format to the format.
        /// </summary>
        private void CloseTableWithPrint() { Output.AppendLine("P"); }

        /// <summary>
        /// Adds a DSX Close Table and Update Data in DSX entry in DML format to the format.
        /// </summary>
        private void CloseTableWithUpdate() { Output.AppendLine("U"); }
    }

    /// <summary>
    /// Helper class that represents an entry in a DSX Table
    /// </summary>
    class Entry
    {
        public string Name {get; private set;}
        public object Value { get; private set; }

        /// <summary>
        /// Constructs an Entry for a DSX Table
        /// </summary>
        /// <param name="name">The of the entry as it will appear in the DSX request.</param>
        /// <param name="value">The value of the entry.</param>
        public Entry(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Helper comparer class that compares a list of Entry objects to each other via name, using their placement in another list to determine order.
    /// </summary>
    class EntryComparer : IComparer<Entry>
    {
        private IList<string> OrderList { get; set; }

        /// <summary>
        /// Creates the comparer for comparing the Entry source list to the order list.
        /// </summary>
        /// <param name="otherList">The list whose order will be mimicked by the order list.</param>
        public EntryComparer(IList<string> orderList)
        {
            OrderList = orderList;
        }

        /// <summary>
        /// Compares two entries' index positions within the order list.
        /// </summary>
        /// <param name="a">The first entry.</param>
        /// <param name="b">The second entry.</param>
        /// <returns>The order in which the two entries should be placed within the Entry list in relation to each other.</returns>
        public int Compare(Entry a, Entry b)
        {
            if (OrderList.Contains(a.Name) && !OrderList.Contains(b.Name))
                return -1;

            if (!OrderList.Contains(a.Name) && OrderList.Contains(b.Name))
                return 1;

            if (!OrderList.Contains(a.Name) && !OrderList.Contains(b.Name))
                return 0;

            return OrderList.IndexOf(a.Name) - OrderList.IndexOf(b.Name);
        }
    }
}
