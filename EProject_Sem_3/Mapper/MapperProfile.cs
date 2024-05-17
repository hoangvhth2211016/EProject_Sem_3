using AutoMapper;
using EProject_Sem_3.Models.Feedbacks;
using EProject_Sem_3.Models.Plans;
using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Subscriptions;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {

        // map for user
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserRes>();
        CreateMap<UserUpdateDto, User>();

        // map for Subscription
        CreateMap<Subscription, SubscriptionRes>();

        // map for Plan
        CreateMap<Plan, PlanRes>();

        // map for Feedback
        CreateMap<FeedbackCreateDto, Feedback>();

        // map for recipe
        CreateMap<RecipeCreateDto, Recipe>();
        CreateMap<Recipe, RecipeRes>();
        CreateMap<RecipeUpdateDto, Recipe>();

        // map for recipe image
        CreateMap<RecipeImage, RecipeImageRes>();
        CreateMap<RecipeImageCreateDto, RecipeImage>();

        // map for bookRes
        CreateMap<Book, BookRes>();

        // map bookDto for book
        CreateMap<BookDto, Book>();

        // map for list of booksRes
        CreateMap<List<Book>, List<BookRes>>().ConvertUsing((src, dest, context) =>
        {
            var mapper = context.Mapper;

            return src.Select(book => mapper.Map<BookRes>(book)).ToList();
        });


        // map OrderDetail for OrderDetailRes
        CreateMap<OrderDetail, OrderDetailRes>()
            .ForMember(dest => dest.Book,
                opt => opt.MapFrom(src => src.Book));


        // map for list of OrderDetailsRes
        CreateMap<List<OrderDetail>, List<OrderDetailRes>>().ConvertUsing((src, dest, context) =>
        {
            var mapper = context.Mapper;

            return src.Select(od => mapper.Map<OrderDetailRes>(od)).ToList();
        });

        // map OrderDetailDto for OrderDetail
        CreateMap<OrderDetailDto, OrderDetail>()
            .ForMember(dest => dest.Order,
                opt => opt.Ignore()) // Bỏ qua ánh xạ cho thuộc tính Order
            .ForMember(dest => dest.Book,
                opt => opt.Ignore()); // Bỏ qua ánh xạ cho thuộc tính Book

        // map OrderDto for Order
        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.OrderDetails,
                opt => opt.Ignore());

        // map Order for OrderRes
        CreateMap<Order, OrderRes>()
            .ForMember(dest => dest.OrderDetails,
                opt => opt.MapFrom(src => src.OrderDetails));

        // map for list of OrderRes
        CreateMap<List<Order>, List<OrderRes>>().ConvertUsing((src, dest, context) =>
        {
            var mapper = context.Mapper;

            return src.Select(od => mapper.Map<OrderRes>(od)).ToList();
        });

    }
}
