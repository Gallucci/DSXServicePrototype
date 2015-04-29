using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class DSXData
    {
        private StringBuilder output;

        private DSXData(DSXDataBuilder builder)
        {
            output = builder.Output;
        }

        public string WriteData()
        {
            return output.ToString();
        }

        public class DSXDataBuilder
        {
            // Properties
            internal StringBuilder Output { get; private set; }

            // Constructors
            public DSXDataBuilder(int locGroupNum, int udfFieldNum, string udfFieldData)
            {
                Output = new StringBuilder();
                Output.AppendLine(string.Format("I L{0} U{1} ^{2}^^^", locGroupNum.ToString(), udfFieldNum.ToString(), udfFieldData));
            }

            public DSXDataBuilder OpenTable(string tableName)
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
            private string FormatDSXBoolean(Boolean value)
            {
                if(value)
                  return "1";
                else
                  return "0";
            }

            public DSXDataBuilder AddField<T>(string fieldName, T fieldValue, bool allowEmptyValue = false)
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

            public DSXDataBuilder WriteToTable()
            {
                Output.AppendLine("W");
                return (this);
            }

            public DSXDataBuilder DeleteFromTable()
            {
                Output.AppendLine("D");
                return (this);
            }

            public DSXDataBuilder PrintTable()
            {
                Output.AppendLine("P");
                return (this);
            }

            public DSXDataBuilder UpdateTable()
            {
                Output.AppendLine("U");
                return (this);
            }

            public DSXData Build()
            {
                return new DSXData(this);
            }
        }
    }
}
