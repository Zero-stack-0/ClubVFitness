import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiServiceService {
  constructor(private http: HttpClient) { }

  createUser(user: any): Observable<any> {
    return this.http.post('http://localhost:5146/create', user);
  }
}
