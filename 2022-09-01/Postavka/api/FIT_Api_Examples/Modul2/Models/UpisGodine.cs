using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul4_MaticnaKnjiga.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul2.Models
{
    public class UpisGodine
    {
        [Key]
        public int id { get; set; }
        public DateTime DatumUpisZimski { get; set; }
        public int GodinaStudija { get; set; }
        [ForeignKey(nameof(AkademskaGodina))]
        public int akademska_godina_id { get; set; }
        public AkademskaGodina  AkademskaGodina { get; set; }
        public float cijenaSkolarine{ get; set; }
        public bool obnova { get; set; }
        public DateTime? datumOvjereZimski { get; set; }
        public string? napomena { get; set; }
        [ForeignKey(nameof(student))]
        public int student_id { get; set; }
        public Student student { get; set; }
        [ForeignKey(nameof(evidentiraoKorisnik))]
        public int evidentirao_korisnik_id { get; set; }
        public KorisnickiNalog  evidentiraoKorisnik { get; set; }

    }
}
