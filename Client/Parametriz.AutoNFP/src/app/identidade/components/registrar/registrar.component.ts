import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { _MatCheckboxRequiredValidatorModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrl: './registrar.component.scss',
  standalone: false
})
export class RegistrarComponent implements OnInit{
  registroForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder
  ) {}
  ngOnInit(): void {
    this.registroForm = this.formBuilder.group({
      razaoSocial: [null, Validators.required],
      cnpj: [null, Validators.required],
      voluntarioNome: [null, Validators.required],
      email: [null, [
          Validators.required,
          Validators.email
        ]],
      senha: [null, [
        Validators.required,
        Validators.minLength(8),
      ]],
      senhaConfirmacao: [null, [
        Validators.required,
        Validators.minLength(8)
      ]]
    })
  }



}
