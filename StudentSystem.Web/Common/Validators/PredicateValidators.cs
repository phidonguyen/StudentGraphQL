using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using SystemTech.Core.Entities;

namespace StudentSystem.Web.Common.Validators
{
    public class PredicateValidators
    {
        private readonly StudentSystemDbContextFactory _pooledFactory;
        
        public PredicateValidators(StudentSystemDbContextFactory pooledFactory)
        {
            _pooledFactory = pooledFactory;
        }
        public bool IsExist<T>(string id)
        {
            using var context = _pooledFactory.CreateDbContext();
            return context.Find(typeof(T), id) != null;
        }

        public string[] GetNotExistItems<T>(string[] ids) where T : class, IBaseEntities 
        {
            using var context = _pooledFactory.CreateDbContext();
            T[] entities = context.Set<T>().Where(_ => ids.Contains(_.Id)).ToArray();
            string[] guestNotExistsIds = ids.Where(_ => entities.All(__ => __.Id != _)).ToArray();
            return guestNotExistsIds;
        }
        
        public bool IsDuplicate<T>(Func<T, bool> filterExpression) where T : class 
        {
            using var context = _pooledFactory.CreateDbContext();
            return context.Set<T>().IgnoreQueryFilters().AsEnumerable().Where(filterExpression).Any();
        }
        
        public bool IsValidDateTime(string dateTime)
        {
            return DateTime.TryParseExact(dateTime, new string[] {"yyyy-MM-dd", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm:ss"},
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime _);
        }
        
        public bool IsGreaterThan(string dateTimFrom, string dateTimTo)
        {
            if (!DateTime.TryParseExact(dateTimFrom, new string[] {"yyyy-MM-dd", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm:ss"},
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime from))
            {
                throw new ValidationException("The DateTimeFrom Invalid format datetime.");
            }

            if (!DateTime.TryParseExact(dateTimTo, new string[] {"yyyy-MM-dd", "dd/MM/yyyy HH:mm", "yyyy-MM-dd HH:mm:ss"},
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime to))
            {
                throw new ValidationException("The DateTimeTo Invalid format datetime.");
            }

            return DateTime.Compare(from, to) < 0;
        }
    }
}