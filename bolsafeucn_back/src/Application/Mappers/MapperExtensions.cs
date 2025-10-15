using Mapster;

namespace bolsafeucn_back.src.Application.Mappers
{
    public class MapperExtensions
    {
        public static void ConfigureMapster(IServiceProvider serviceProvider)
        {
            var studentMapper = serviceProvider.GetService<StudentMapper>();
            studentMapper?.ConfigureAllMappings();
            var individualMapper = serviceProvider.GetService<IndividualMapper>();
            individualMapper?.ConfigureAllMappings();
            var companyMapper = serviceProvider.GetService<CompanyMapper>();
            companyMapper?.ConfigureAllMappings();
            var adminMapper = serviceProvider.GetService<AdminMapper>();
            adminMapper?.ConfigureAllMappings();

            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
        }
    }
}
