import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  constructor(private http: HttpClient) { }

  login(modal: any): Observable<object> {
    return this.http.post(this.baseUrl + 'account/login', modal);
  }
}
