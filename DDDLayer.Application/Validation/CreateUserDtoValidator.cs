using DDDLayer.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.Validation
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty().WithMessage("İsim alanı zorunludur");
            RuleFor(i => i.Email).EmailAddress().WithMessage("Lütfen geçerli bir email giriniz");
        }

    }
}
