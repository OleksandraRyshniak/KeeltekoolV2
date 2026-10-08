using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.Core.Domain
{
    public class Lecturer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Fullname TBA
        public string Qualification { get; set; }
        public string UserID { get; set; }
       // public ICollection<LanguageSubject> LanguageSubject { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
