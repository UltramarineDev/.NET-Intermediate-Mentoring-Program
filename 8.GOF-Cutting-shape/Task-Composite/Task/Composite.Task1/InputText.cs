using System;
using System.Xml.Linq;

namespace Composite.Task1
{
    public class InputText
    {
        private readonly string _name;
        private readonly string _value;

        private const string Label = "inputText";
        private const string AttributeNameLabel = "name";
        private const string AttributeValueLabel = "value";

        public InputText(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            var element = new XElement(Label,
                new XAttribute(AttributeNameLabel, _name),
                new XAttribute(AttributeValueLabel, _value));

            return element.ToString();
        }
    }
}
