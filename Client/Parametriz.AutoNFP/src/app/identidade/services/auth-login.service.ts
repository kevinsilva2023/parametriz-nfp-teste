import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Usuario } from '../models/usuario';

@Injectable({
  providedIn: 'root'
})
export class AuthLoginService {
  private apiUrl = environment.apiUrl

  constructor(
    private http: HttpClient
  ) { }

  autenticarUsuario(email: string, senha: string): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.apiUrl}/`, {email, senha});
  }
}
