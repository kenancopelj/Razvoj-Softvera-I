﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return Forbid();

            return Ok(_dbContext.Student.Include(s => s.opstina_rodjenja.drzava).FirstOrDefault(s => s.id == id)); ;
        }

        [HttpPost ("{id}")]
        public ActionResult Update(int id, [FromBody] StudentUpdateVM x)
        {
            //if (!HttpContext.GetLoginInfo().isLogiran)
            //    return BadRequest("Niste logirani.");

            Student student;

            if(id == 0)
            {
                student = new Student()
                {
                    slika_korisnika = Config.SlikeURL + "empty.png",
                    created_time = DateTime.Now
                };
                _dbContext.Add(student);
            }
            else
            {
                student = _dbContext.Student.Find(id);
                if (student == null)
                    return BadRequest("Ne postoji student sa ovim IDom!");
            }

            student.ime = x.ime.RemoveTags();
            student.prezime = x.prezime.RemoveTags();
            student.broj_indeksa = x.broj_indeksa;
            student.datum_rodjenja = x.datum_rodjenja;
            student.opstina_rodjenja_id = x.opstina_rodjenja_id;

            _dbContext.SaveChanges();

            return Ok(student);
        }

        [HttpPost ("{id}")]
        public ActionResult Delete (int id)
        {
            //if (!HttpContext.GetLoginInfo().isLogiran)
            //    return BadRequest("Niste logirani.");

            Student student = _dbContext.Student.Find(id);

            if (student == null || id == 1)
                return BadRequest("Pogrešan ID");

            _dbContext.Remove(student);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(x => ime_prezime == null || (x.ime + " " + x.prezime).StartsWith(ime_prezime) || (x.prezime + " " + x.ime).StartsWith(ime_prezime))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }

    }
}
