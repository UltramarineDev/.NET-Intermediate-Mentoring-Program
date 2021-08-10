namespace Adapter.Task1
{
    public class PrinterAdapter : IMyPrinter
    {
        private readonly Printer _printer;

        public PrinterAdapter(Printer printer)
        {
            _printer = printer;
        }

        public void Print<T>(IElements<T> elements)
        {
            _printer.Print(new Container<T>(elements.GetElements()));
        }
    }
}
