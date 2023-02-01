import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";

declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css'],
})
export class StudentMaticnaknjigaComponent implements OnInit {
  studentId: any;
  odabraniStudent: any = [];

  noviUpis : any = null;
  podaciUpis: any=[];
  podaciAkademskeGodine: any;
  ovjera: boolean = false;
  odabraniOvjera: any = null;


  constructor(private httpKlijent : HttpClient, private route : ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe((x:any)=>{
      this.studentId = +x['id'];
    });
    console.log(this.studentId);
    this.fetchStudent();
    this.fetchUpise();
    this.fetchAkademske();
  }

  ovjeriLjetni() {}

  upisLjetni() {}

  ovjeriZimski(x: any) {
    this.odabraniOvjera=x;
  }

   fetchStudent() {
    this.httpKlijent.get(MojConfig.adresa_servera+"/Student/Get/"+this.studentId,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.odabraniStudent = x;
    },(err)=>porukaError(err.error));
  }

  NapraviNovi() {
    this.noviUpis={
      godinaStudija:1,
      akademska_godina_id:1,
      DatumUpisZimski : new Date(),
      cijenaSkolarine:0,
      obnova:false,
      student_id:this.studentId
    }
  }

  Upisi() {
    this.httpKlijent.post(MojConfig.adresa_servera+"/UpisGodine/Add",this.noviUpis,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.noviUpis=null;
      this.fetchUpise();
    },(err)=>porukaError(err.error));
  }

   fetchUpise() {
     this.httpKlijent.get(MojConfig.adresa_servera+"/UpisGodine/GetAll/"+this.studentId,MojConfig.http_opcije()).subscribe((x:any)=>{
       this.podaciUpis = x;
     },(err)=>porukaError(err.error));
  }

   fetchAkademske() {
     this.httpKlijent.get(MojConfig.adresa_servera+"/AkademskeGodine/GetAll_ForCmb/",MojConfig.http_opcije()).subscribe((x:any)=>{
       this.podaciAkademskeGodine = x;
     },(err)=>porukaError(err.error));
  }

  Ovjeri() {
    this.httpKlijent.put(MojConfig.adresa_servera+"/UpisGodine/Update/",this.odabraniOvjera,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.odabraniOvjera=null;
      this.fetchUpise();
    },(err)=>porukaError(err.error));
  }
}

/*public int id { get; set; }
public int godinaStudija { get; set; }
public int akademska_godina_id { get; set; }
public DateTime DatumUpisZimski { get; set; }
public float cijenaSkolarine { get; set; }
public bool obnova { get; set; }
public DateTime? datumOvjereZimski { get; set; }
public string? napomena { get; set; }
public int student_id { get; set; }
public int evidentirao_korisnik_id { get; set; }*/
