using AutoMapper;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Mapper;

public class MapperProfile : Profile {
    public MapperProfile() {

        // map for user
        CreateMap<RegisterDto, User>();
        
        // map for bookRes
        CreateMap<Book, BookRes>();
        
        // map bookDto for book
        CreateMap<BookDto, Book>();
        
        // map for list of books
        CreateMap<List<Book>, List<BookRes>>().ConvertUsing((src, dest, context) =>
        {
            var mapper = context.Mapper;

            return src.Select(book => mapper.Map<BookRes>(book)).ToList();
        });
        
    }
}