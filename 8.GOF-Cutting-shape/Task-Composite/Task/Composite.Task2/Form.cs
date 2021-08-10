using System.Xml.Linq;

namespace Composite.Task2
{
    //Form element should have ability to add internal elements. 
    public class Form : IComponent
    {
        private readonly string _formName;
        private XElement _formElement;
        
        private const string Label = "form";
        private const string AttributeLabel = "name";

        public Form(string name)
        {
            _formName = name;
            CreateFormElement();
        }

        public void AddComponent(IComponent component)
        {
            var xDocument = XDocument.Parse(component.ConvertToString());

            _formElement.Add(xDocument.Nodes());
        }

        public string ConvertToString(int depth = 0)
        {
            return _formElement.ToString();
        }

        private void CreateFormElement()
        {
            _formElement = new XElement(Label, new XAttribute(AttributeLabel, _formName));
        }
    }
}