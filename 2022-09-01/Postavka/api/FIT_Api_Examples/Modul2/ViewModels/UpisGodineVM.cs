using System;

namespace FIT_Api_Examples.Modul2.ViewModels
{
    public class UpisGodineVM
    {
        public int id { get; set; }
        public int godinaStudija { get; set; }
        public int akademska_godina_id { get; set; }
        public DateTime DatumUpisZimski { get; set; }
        public float cijenaSkolarine { get; set; }
        public bool obnova { get; set; }
        public DateTime? datumOvjereZimski { get; set; }
        public string? napomena { get; set; }
        public int student_id { get; set; }
        public int evidentirao_korisnik_id { get; set; }

    }
}
