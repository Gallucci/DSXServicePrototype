using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class DMLRequestData : IRequestData
    {
        private StringBuilder output;

        private DMLRequestData(DataBuilder builder)
        {
            output = builder.Output;
        }

        public string WriteData()
        {
            return output.ToString();
        }

        internal sealed class DataBuilder
        {
            // Properties
            public StringBuilder Output { get; private set; }

            // Constructors
            public DataBuilder(int locGroupNum, int udfFieldNum, string udfFieldData)
            {
                Output = new StringBuilder();
                Output.AppendLine(string.Format("I L{0} U{1} ^{2}^^^", locGroupNum.ToString(), udfFieldNum.ToString(), udfFieldData));
            }

            public DataBuilder OpenTable(string tableName)
            {
                Output.AppendLine(string.Format("T {0}", tableName));
                return (this);
            }

            // Methods
            private string FormatDSXDate(DateTime value)
            {
                var pattern = "M/d/yyyy HH:mm";
                return value.ToString(pattern);
            }
            private string FormatDSXBoolean(bool value)
            {
                if(value)
                  return "1";
                else
                  return "0";
            }

            public DataBuilder AddField<T>(string fieldName, T fieldValue, bool allowEmptyValue = false)
            {
                string value = string.Empty;

                if (fieldValue is DateTime)
                {                    
                    value = FormatDSXDate((DateTime)(object)fieldValue);
                }
                else if (fieldValue is DateTime?)
                {
                    if ((fieldValue as DateTime?).HasValue)
                        value = FormatDSXDate((DateTime)(object)fieldValue);
                }
                else if (fieldValue is bool)
                {
                    value = FormatDSXBoolean((bool)(object)fieldValue);
                }
                else if (fieldValue is bool?)
                {
                    if ((fieldValue as bool?).HasValue)
                        value = FormatDSXBoolean((bool)(object)fieldValue);
                }
                else
                {
                    if(fieldValue != null)
                        value = fieldValue.ToString().Trim();
                }

                if(!string.IsNullOrEmpty(value) || (allowEmptyValue && value != null))
                    Output.AppendLine(string.Format("F {0} ^{1}^^^", fieldName, value));

                return (this);
            }

            public DataBuilder AddField<T>(IDictionary<string, T> fieldSet, bool allowEmptyValues = false)
            {
                foreach(var field in fieldSet)
                {
                    AddField(field.Key, field.Value, allowEmptyValues);
                }
                return (this);
            }

            public DataBuilder CloseTableWithWrite()
            {
                Output.AppendLine("W");
                return (this);
            }

            public DataBuilder CloseTableWithDelete()
            {
                Output.AppendLine("D");
                return (this);
            }

            public DataBuilder CloseTableWithPrint()
            {
                Output.AppendLine("P");
                return (this);
            }

            public DataBuilder CloseTableWithUpdate()
            {
                Output.AppendLine("U");
                return (this);
            }

            public IRequestData Build()
            {
                return new DMLRequestData(this);
            }
        }
    }
}
