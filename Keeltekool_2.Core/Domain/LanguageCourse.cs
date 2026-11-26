using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Keeltekool_2.Core.Domain
{
    //Domain kaustas olev mudel näitab ära täpse andmekuju millena kursuse andmed andmebaasis ka
    //hoitakse, siin võib samuti olla valikulisi andmeid, kuid tüüpiliselt on neid vähem
    //sest andmebaas hoiab tihtipeale ainult vajalikke andmeid.
    public class LanguageCourse
    {
        public Guid Id { get; set; }
        /// <example>Nimetus</example>
        [Required]
        public string Nimetus { get; set; }
        /// <example>Keel</example>
        public string Keel { get; set; }
        /// <example>Tase</example>
        public string Tase { get; set; }
        /// <example>Kirjeldus</example>
        public string Kirjeldus { get; set; }

        //Vajalikud andmeväljad, mida muudavad ainult kontroller ja/või service
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        /// <example>null</example>
        public string? ModifiedBy { get; set; }
    }
}