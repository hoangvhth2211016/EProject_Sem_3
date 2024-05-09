using AutoMapper;
using EProject_Sem_3.Models.Feedbacks;
using EProject_Sem_3.Models.Plans;
using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Subscriptions;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Mapper;

public class MapperProfile : Profile {
    public MapperProfile() {

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

    }
}