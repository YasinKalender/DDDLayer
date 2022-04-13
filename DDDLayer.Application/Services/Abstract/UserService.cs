using DDDLayer.Application.DTOs;
using DDDLayer.Application.Services.Interfaces;
using DDDLayer.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.Services.Abstract
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user =new User { Email=createUserDto.Email,UserName=createUserDto.UserName,FirstName=createUserDto.FirstName,LastName=createUserDto.LastName,BirthDate=createUserDto.BirthDate,ImageUrl=createUserDto.ImageUrl};

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            var newUser = user.Adapt<UserDto>();
            return newUser;

            


        }

        public async Task<UserDto> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user==null) return null;

            var userDto = user.Adapt<UserDto>();

            return userDto;
            

           
        }
    }
}
