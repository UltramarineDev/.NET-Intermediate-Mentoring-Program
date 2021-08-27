namespace Adapter.Task1
{
    public class MyPrinterFactory
    {
        public IMyPrinter CreateMyPrinter(IWriter writer)
        {
            return new PrinterAdapter(new Printer(writer));
        }
    }
}
