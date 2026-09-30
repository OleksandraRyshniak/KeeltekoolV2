using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keeltekool_2.Core.ServiceInterface
{
    public interface ILanguageCoursesServices
    {
        Task<LanguageCourse> Create(LanguageCourseDTO dto);
        Task<LanguageCourse> Update(LanguageCourseDTO dto);
        Task<LanguageCourse> Update(Guid id);
        Task<LanguageCourse> Delete(Guid id);
    }
}