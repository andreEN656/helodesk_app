using IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace helpdesk_backend.Mapper
{
    public static class Mapper
    {
        public static void Init()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            AutoMapper.Mapper.Initialize((config) =>
            {
                config.CreateMap<User, UserViewModel>();
                config.CreateMap<UserViewModel, User>();

                config.CreateMap<User, UserViewModel>()
                    .ForMember(vm => vm.Role, opt => opt.Ignore())
                    .ForMember(vm => vm.Password, opt => opt.MapFrom(m => m.PasswordHash))
                    .ForMember(vm => vm.PasswordConfirm, opt => opt.MapFrom(m => m.PasswordHash));
                config.CreateMap<UserViewModel, User>();

            });
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
