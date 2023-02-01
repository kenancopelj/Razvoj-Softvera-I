import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MojConfig } from '../moj-config';
import { Router } from '@angular/router';
declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css'],
})
export class StudentiComponent implements OnInit {
  title: string = 'angularFIT2';
  filter_imeprezime: string = '';
  filter_opstina: string = '';
  studentPodaci: any;
  checked_opstina: boolean;
  checked_imeprezime: boolean;
  odabraniStudent: any = null;
  constructor(private httpKlijent: HttpClient, private router: Router) {}

  testirajWebApi(): void {
    this.httpKlijent
      .get(
        MojConfig.adresa_servera + '/Student/GetAll',
        MojConfig.http_opcije()
      )
      .subscribe((x: any) => {
        this.studentPodaci = x;
      });
  }

  ngOnInit(): void {
    this.testirajWebApi();
  }

  GetPodaci(){
      if(this.studentPodaci==null)
        return [];

      return this.studentPodaci.filter((x:any)=>
        (!this.checked_imeprezime || ((x.ime.toLowerCase() + " " + x.prezime.toLowerCase()).startsWith(this.filter_imeprezime))
          ||(x.ime.toLowerCase() + " " + x.prezime.toLowerCase()).startsWith(this.filter_imeprezime))
      && (!this.checked_opstina || x.opstina_rodjenja.description.toLowerCase().startsWith(this.filter_opstina)))
  }

  Uredi(s: any) {
    this.odabraniStudent=s;
  }

  Obrisi(s: any) {
    this.httpKlijent.delete(MojConfig.adresa_servera+"/Student/Delete/"+s.id,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.testirajWebApi();
    });
  }

  napraviNovi() {
    this.odabraniStudent={
      id:0,
      ime:this.filter_imeprezime.slice(0,1).toUpperCase() + this.filter_imeprezime.slice(1,).toLowerCase(),
      prezime:"",
      broj_indeksa:"",
      opstina_rodjenja_id:2,
    }
  }

  maticnaKnijga(s: any) {
    this.router.navigate(['/student-maticnaknjiga/',s.id]);
  }
}
