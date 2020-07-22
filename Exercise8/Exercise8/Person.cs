//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exercise8
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> GenderID { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        
        public virtual Gender Gender { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {MiddleName} {LastName}"; }
        }
        public int Age
        {
            get
            {
                if (DateOfBirth != null)
                {
                    DateTime bday = DateOfBirth.Value;
                    TimeSpan A = DateTime.Now - bday;
                    return (int)A.TotalHours / 8766;
                }
                else
                    return 0;
            }
        }
    }
}
