import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as api from '../../apiUrl.json';

@Injectable({
  providedIn: 'root'
})
export class ValuesService {
  url: any;

  constructor(private http: HttpClient) {
    //ef
    //this.url = api.efUrl;
    //ado
    this.url = api.adoUrl;
  }

  GetAll() {
    return this.http.get(this.url + "values");
  }
}
