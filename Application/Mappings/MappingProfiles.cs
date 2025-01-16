using Application.DTOs.Products;
using Application.DTOs.Users;
using Application.UseCases.Accounts.Command;
using Application.UseCases.Categories.Command;
using Application.UseCases.Products.Command;
using Application.UseCases.Users.Command;
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
            UserMappingRules();
            CategoryMappingRules();
        }

        private void CategoryMappingRules()
        {
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<Category, CreateCategoryCommandHandlerResult>();
        }

        private void UserMappingRules()
        {
            CreateMap<RegisterUserCommand, User>();
            CreateMap<User, UserGetDto>();
            CreateMap<ModifyUserCommand, User>();
        }

        private void ProductMappingRules()
        {
            CreateMap<CreateProductCommand, Product>();
            CreateMap<Product, CreateProductCommandHandlerResult>();

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(destination => destination.ProductThumbnails,
                options => options.MapFrom(src => src.ThumbnailsId
                .Select(x => new ProductThumbnail() { Id = x })));

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(destination => destination.ProductThumbnails, option => option.Ignore());

            CreateMap<Product, UpdateProductDTO>()
                .ForMember(destination => destination.ThumbnailsId,
                options => options.MapFrom(src => src.ProductThumbnails.Select(p => p.Id)))
                .ForMember(destination => destination.CategoryId,
                options => options.MapFrom(src => src.Category.Id)).ReverseMap();

            CreateMap<Product, GetProductDto>()
               .ForMember(destination => destination.ThumbnailsId,
                options => options.MapFrom(src => src.ProductThumbnails.Select(p => p.Id)))
                .ForMember(destination => destination.CategoryId,
                options => options.MapFrom(src => src.Category.Id));

        }
    }
}

