using System;
using System.Collections.Generic;
using System.Text;

namespace LinqProvider.Services
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SqlColName : Attribute
    {
        private string _name = "";
        public string Name { get => _name; set => _name = value; }

        public SqlColName(string name)
        {
            _name = name;
        }
    }
}
