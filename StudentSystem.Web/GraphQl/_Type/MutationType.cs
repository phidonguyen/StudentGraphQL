namespace StudentSystem.Web.GraphQl._Type
{
    public class MutationType: ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            
            #region Auth

            descriptor.Field(_ => _.Login(default!, default!, default));
            descriptor.Field(_ => _.RefreshToken(default!, default!, default));

            #endregion
            
            #region Student
            
            descriptor.Field(_ => _.AddStudent(default!, default!, default))
                .Authorize();
            descriptor.Field(_ => _.EditStudent(default!, default!,default))
                .Authorize();
            descriptor.Field(_ => _.RemoveStudent(default!, default!,default))
                .Authorize();

            #endregion
            
            #region Subject
            
            descriptor.Field(_ => _.AddSubject(default!, default!, default))
                .Authorize();
            descriptor.Field(_ => _.EditSubject(default!, default!,default))
                .Authorize();
            descriptor.Field(_ => _.RemoveSubject(default!, default!,default))
                .Authorize();

            #endregion
            
            #region Result
            
            descriptor.Field(_ => _.AddResult(default!, default!, default))
                .Authorize();
            descriptor.Field(_ => _.EditResult(default!, default!,default))
                .Authorize();
  
            #endregion
        }
    }
}