using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [Autorizacija(studentskaSluzba:true,prodekan:true,dekan:true,studenti:false,nastavnici:true)]
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid();

            return Ok(_dbContext.Student.Include(s => s.opstina_rodjenja.drzava).FirstOrDefault(s => s.id == id)); ;
        }
        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");


            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(x => ime_prezime == null || (x.ime + " " + x.prezime).StartsWith(ime_prezime) || (x.prezime + " " + x.ime).StartsWith(ime_prezime))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }
        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpPost]
        public ActionResult Snimi([FromBody] StudentAddVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");
            
            Student student;
            if (x.id == 0)
            {
                student = new Student();
                student.created_time = DateTime.Now;
                _dbContext.Student.Add(student);
            }
            else
                student = _dbContext.Student.Find(x.id);

            student.ime = x.ime;
            student.prezime = x.prezime;
            student.broj_indeksa = x.broj_indeksa;
            student.opstina_rodjenja_id = x.opstina_rodjenja_id;
            student.datum_rodjenja = x.datum_rodjenja;

            _dbContext.SaveChanges();

            return Ok(student);
        }
        [Autorizacija(studentskaSluzba: true, prodekan: true, dekan: true, studenti: false, nastavnici: true)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid("Nije logiran!");

            var student = _dbContext.Student.Find(id);
            _dbContext.Remove(student);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
