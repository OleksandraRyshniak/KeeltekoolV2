using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Data;
using Keeltekool_2.Core.ServiceInterface;
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
            //kontrollitakse kas dto omab mingeid andmeid
            if (dto == null)
            {
                return null;
            }

            //tekitab uue andmebaasis istuva objekti vastava mudeli jörgi
            LanguageCourse domain = new LanguageCourse();

            //omistab andmed andmeedasikandeobjektist domeenimudelile
            //lahendades ära küsimused mis ei lahendatud kontrolleris, nagu näiteks id, createdat, modifiedat
            domain.Id = Guid.NewGuid();
            domain.Nimetus = dto.Nimetus;
            domain.Keel = dto.Keel;
            domain.Kirjeldus = dto.Kirjeldus;
            domain.Tase = dto.Tase;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;
            //later, require user id to be attached to "ModifiedBy" parameter, to know who modified last.

            //Todo: check if db addition succeeded, if yes, return object, if not, null
            //teostatakse andmebaasi lisamise tegevus
            await _context.LanguageCourses.AddAsync(domain);
            await _context.SaveChangesAsync();
            //tagastatakse domeenile kuuluv objekt
            return domain;
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

        public Task<LanguageCourse> DetailsAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}