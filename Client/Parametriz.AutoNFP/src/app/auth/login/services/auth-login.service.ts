import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthLoginService {
  private apiUrl = environment.apiUrl

  constructor(
    // private http: HttpClient
  ) { }

  // autenticarUsuario(email: string, senha: string) {
  //   return this.bt.http<any>(`${this.apiUrl}/`, {email, senha});
  // }
}
