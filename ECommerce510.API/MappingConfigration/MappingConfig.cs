namespace ECommerce510.API.MappingConfigration
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryRequest, Category>()
            //    .Map(des => des.Name, src => src.CategoryName)
            //    .Map(des => des.Description, src => src.Note);
        }
    }
}
