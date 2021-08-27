namespace Adapter.Task1
{
    //can not change
    public class Printer
    {
        private readonly IWriter writer;

        public Printer(IWriter writer)
        {
            this.writer = writer;
        }

        public void Print<T>(IContainer<T> container)
        {
            foreach (var item in container.Items)
            {
                this.writer.Write(item.ToString());
            }
        }
    }
}
