using AutoMapper;

namespace Portfolio.Application.Mapping;

public abstract class MappingProfile : Profile
{
    protected MappingProfile()
    {
        // Derived classes should configure mappings in their constructor
    }
}