import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthLoginService } from './services/auth-login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authLogin: AuthLoginService,
    private router: Router
  ) {}
  
  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: [null, [
        Validators.required,
        Validators.email
      ]],
      senha: [null, [
        Validators.required,
        Validators.minLength(8)
      ]]
    });
  }

  fazerLogin() {
      const email = this.loginForm.value.email;
      const senha = this.loginForm.value.senha;

      // this.authLogin.autenticarUsuario(email, senha)
      //   .subscribe({
      //     next: (value) => {
      //       this.router.navigateByUrl('/');
      //     },
      //     error: (err) => console.error(err)
      //   })
  }
}
