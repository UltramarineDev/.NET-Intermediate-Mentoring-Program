using TemplateMethod.Task2.Cookers;

namespace TemplateMethod.Task2
{
    public class MasalaCooker
    {
        private readonly ICooker _cooker;
        private readonly CookerFactory _cookerFactory;

        public MasalaCooker(ICooker cooker)
        {
            _cooker = cooker;
            _cookerFactory = new CookerFactory();
        }

        public void CookMasala(Country country)
        {
            var localCooker = GetCooker(country);
            localCooker.CookMasala(_cooker);
        }

        private CookerBase GetCooker(Country country)
            => country == Country.Ukraine ? _cookerFactory.CreateUkraineCooker() : _cookerFactory.CreateIndianCooker();
    }
}
