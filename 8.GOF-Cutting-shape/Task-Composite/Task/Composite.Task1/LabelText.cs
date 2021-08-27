using System.Xml.Linq;

namespace Composite.Task1
{
    public class LabelText
    {
        private readonly string _value;

        private const string Label = "label";
        private const string AttributeLabel = "value";

        public LabelText(string value)
        {
            _value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            var element = new XElement(Label, new XAttribute(AttributeLabel, _value));
            return element.ToString();
        }
    }
}
