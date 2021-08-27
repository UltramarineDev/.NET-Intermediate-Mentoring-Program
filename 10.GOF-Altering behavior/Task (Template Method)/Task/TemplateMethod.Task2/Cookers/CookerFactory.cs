namespace TemplateMethod.Task2.Cookers
{
    public class CookerFactory
    {
        public CookerBase CreateUkraineCooker() => new UkraineCooker();
        public CookerBase CreateIndianCooker() => new IndiaCooker();
    }
}
