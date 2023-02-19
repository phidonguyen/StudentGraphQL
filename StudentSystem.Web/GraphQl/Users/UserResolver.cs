using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;
using SystemTech.Core.Entities;

namespace StudentSystem.Web.GraphQl.Users
{
    public class UserResolver
    {
        public async Task<User> GetCreator<T>([Parent] T model,
            GraphQlDataLoader<User> dataLoader,
            CancellationToken cancellationToken)
        {
            if (!(model is IAuditFields auditFields)) return null;
            
            if (string.IsNullOrEmpty(auditFields.CreatedBy)) return null;
            
            return await dataLoader.LoadAsync(auditFields.CreatedBy, cancellationToken);
        }

        public async Task<User> GetUpdater<T>([Parent] T model,
            GraphQlDataLoader<User> dataLoader,
            CancellationToken cancellationToken)
        {
            if (!(model is IAuditFields auditFields)) return null;
            
            if (string.IsNullOrEmpty(auditFields.UpdatedBy)) return null;
            
            return await dataLoader.LoadAsync(auditFields.UpdatedBy, cancellationToken);
        }
    }   
}