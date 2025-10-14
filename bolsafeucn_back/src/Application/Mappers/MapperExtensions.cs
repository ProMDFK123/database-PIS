using Mapster;

namespace bolsafeucn_back.src.Application.Mappers
{
    public class MapperExtensions
    {
        public static void ConfigureMappings(IServiceProvider serviceProvider)
        {
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
        }
    }
}
