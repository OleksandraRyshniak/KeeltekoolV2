using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class LanguageCoursesServices : ILanguageCoursesServices
    {
        private readonly Keeltekool_2Context _context;

        public LanguageCoursesServices(Keeltekool_2Context context)
        {
            _context = context;
        }

        public async Task<LanguageCourse> Create(LanguageCourseDTO dto)
        {
            return null;
        }
        public async Task<LanguageCourse> Update(LanguageCourseDTO dto)
        {
            return null;
        }
        public async Task<LanguageCourse> Update(Guid id)
        {
            return null;
        }
        public async Task<LanguageCourse> Delete(Guid id)
        {
            return null;
        }
    }
}