using FluentValidation;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Common.Messages;
using StudentSystem.Web.Common.Validators;

namespace StudentSystem.Web.GraphQl.Students
{ 
    public class EditStudentInput
   {

        public string Id { get; set; } = null!;
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int? IdentityNumber { get; set; }
    }
    
    public class EditStudentInputValidator : AbstractValidator<EditStudentInput>
    {
        public EditStudentInputValidator(PredicateValidators predicateValidators)
        {
            Predicate<string> idStudentIsExist = predicateValidators.IsExist<Student>;

            RuleFor(input => input.Id)
                .Must(id => idStudentIsExist(id))
                .WithMessage(CommonMessages.PropertyNotExist(PropertyName.Student.Id));
        }
    }

    public class EditStudentInputType : InputObjectType<EditStudentInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<EditStudentInput> descriptor)
        {
            descriptor.Field(b => b.Id).Type<StringType>();
            descriptor.Field(b => b.Name).Type<StringType>();
            descriptor.Field(b => b.PhoneNumber).Type<StringType>();
            descriptor.Field(b => b.IdentityNumber).Type<StringType>();

            base.Configure(descriptor);
        }
    }   
}