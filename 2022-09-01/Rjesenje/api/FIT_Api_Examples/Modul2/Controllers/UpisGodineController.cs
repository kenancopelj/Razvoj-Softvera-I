using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;

namespace FIT_Api_Examples.Modul2.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UpisGodineController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public UpisGodineController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpPost]
        public ActionResult Add([FromBody] UpisGodineVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");


            var noviUpis = new UpisGodine
            {
                akademska_godina_id = x.akademska_godina_id,
                student_id = x.student_id,
                cijenaSkolarine = x.cijenaSkolarine,
                GodinaStudija = x.godinaStudija,
                DatumUpisZimski = x.DatumUpisZimski,
                evidentirao_korisnik_id = HttpContext.GetLoginInfo().korisnickiNalog.id,
                obnova = x.obnova,
            };

            if (_dbContext.UpisGodine.Any(u => u.GodinaStudija == noviUpis.GodinaStudija && u.student_id == noviUpis.student_id) && !noviUpis.obnova)
                return BadRequest("Ne moze");

            _dbContext.Add(noviUpis);
            _dbContext.SaveChanges();

            return Ok(noviUpis);
        }


        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpGet("{id}")]
        public ActionResult GetAll(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");

            var student = _dbContext.Student.Where(x => x.id == id).FirstOrDefault();
                var upisaneGodine = _dbContext.UpisGodine.Where(x => x.student_id == id)
                .Select(u => new
                {
                    id = u.id,
                    akGodina = u.AkademskaGodina.opis,
                    godinaStudija = u.GodinaStudija,
                    obnova = u.obnova,
                    datumOvjereZimski = u.datumOvjereZimski,
                    datumUpisZimski = u.DatumUpisZimski,
                    evidentiraoKorisnik = u.evidentiraoKorisnik.korisnickoIme
                });
                var objekat =  new
                {
                    studentId= student.id,
                    studentIme= student.ime,
                    studentPrezime = student.prezime,
                    upisaneAkGodine = upisaneGodine.ToList()
                };

            return Ok(objekat);
        }


        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpPut]
        public ActionResult Update([FromBody] UpisGodineUpdateVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");

            var upis = _dbContext.UpisGodine.Find(x.id);
            upis.datumOvjereZimski = x.datumOvjereZimski;
            upis.napomena = x.napomena;

            _dbContext.SaveChanges();

            return Ok(upis);
        }



    }
}
