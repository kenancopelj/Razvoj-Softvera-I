import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { MojConfig } from '../../moj-config';
import {HttpClient} from "@angular/common/http";

declare function porukaSuccess(s: string): any;

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css'],
})
export class StudentEditComponent implements OnInit {

  @Output() ucitaj = new EventEmitter<void>();
  @Input() urediStudent : any;
  opstine: any;

  constructor(private httpKlijent : HttpClient) {}
  ngOnInit(): void {
    this.getOpstine();
  }

  Spasi() {
    this.httpKlijent.post(MojConfig.adresa_servera+"/Student/Snimi/",this.urediStudent,MojConfig.http_opcije()).subscribe((x:any)=>{
        this.urediStudent=null;
        this.ucitaj.emit();
    },(err)=>porukaSuccess(err.error));
  }

  getOpstine() {
    this.httpKlijent.get(MojConfig.adresa_servera+"/Opstina/GetByAll/",MojConfig.http_opcije()).subscribe((x:any)=>{
      this.opstine=x;
    },(err)=>porukaSuccess(err.error));
  }
}
