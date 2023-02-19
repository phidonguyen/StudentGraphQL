using FluentValidation;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Common.Messages;
using StudentSystem.Web.Common.Validators;

namespace StudentSystem.Web.GraphQl.Subjects
{ 
    public class EditSubjectInput
   {

        public string Id { get; set; } = null!;
        public string Name { get; set; }
   }
    
    public class EditSubjectInputValidator : AbstractValidator<EditSubjectInput>
    {
        public EditSubjectInputValidator(PredicateValidators predicateValidators)
        {
            Predicate<string> idSubjectIsExist = predicateValidators.IsExist<Subject>;

            RuleFor(input => input.Id)
                .Must(id => idSubjectIsExist(id))
                .WithMessage(CommonMessages.PropertyNotExist(PropertyName.Subject.Id));
        }
    }

    public class EditSubjectInputType : InputObjectType<EditSubjectInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<EditSubjectInput> descriptor)
        {
            descriptor.Field(b => b.Id).Type<StringType>();
            descriptor.Field(b => b.Name).Type<StringType>();

            base.Configure(descriptor);
        }
    }   
}