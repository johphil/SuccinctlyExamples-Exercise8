using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Exercise8
{
    public static class Extensions
    {
        public static IQueryable<Person> IsEqual(this IQueryable<Person> q, int age)
        {
            return q.Where(p => (DbFunctions.DiffHours(p.DateOfBirth, DateTime.Today) / 8766) == age);
        }
        public static IQueryable<Person> IsGreaterThan(this IQueryable<Person> q, int age)
        {
            return q.Where(p => (DbFunctions.DiffHours(p.DateOfBirth, DateTime.Today) / 8766) > age);
        }
        public static IQueryable<Person> IsLessThan(this IQueryable<Person> q, int age)
        {
            return q.Where(p => (DbFunctions.DiffHours(p.DateOfBirth, DateTime.Today) / 8766) < age);
        }
        public static IQueryable<Person> IsGreaterThanOrEqual(this IQueryable<Person> q, int age)
        {
            return q.Where(p => (DbFunctions.DiffHours(p.DateOfBirth, DateTime.Today) / 8766) >= age);
        }
        public static IQueryable<Person> IsLessThanOrEqual(this IQueryable<Person> q, int age)
        {
            return q.Where(p => (DbFunctions.DiffHours(p.DateOfBirth, DateTime.Today) / 8766) <= age);
        }
    }
}
