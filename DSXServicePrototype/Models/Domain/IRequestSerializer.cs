using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public enum SerializerFormat
    {
        DML,
        JSON,
        XML
    }

    public interface IRequestSerializer
    {
        string Serialize();
    }
}
