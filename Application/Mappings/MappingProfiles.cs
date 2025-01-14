using Application.UserCases.Products.Command;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            ProductMappingRules();
        }

        private void ProductMappingRules()
        {
            CreateMap<CreateProductCommand, Product>();
            CreateMap<Product, CreateProductCommandHandlerResult>();
        }
    }
}
