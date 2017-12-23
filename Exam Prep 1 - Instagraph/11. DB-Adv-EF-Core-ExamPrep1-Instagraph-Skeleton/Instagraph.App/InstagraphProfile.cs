using AutoMapper;
using Instagraph.DataProcessor.DtoModels;
using Instagraph.Models;

namespace Instagraph.App
{
    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            CreateMap<UserDto, User>()
                //  на 2-рия кое проп ще мапваме, как да го мапне и ако не може да сложи null
                .ForMember(u => u.ProfilePicture, pp => pp.UseValue<Picture>(null));
        }
    }
}
