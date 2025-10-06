import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-definir-senha',
  standalone: false,
  templateUrl: './definir-senha.component.html',
})
export class DefinirSenhaComponent implements OnInit {
  definirSenhaForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.definirSenhaForm = this.formBuilder.group({
      senha: [null, Validators.required],
      senhaConfirmacao: [null, Validators.required]    
    })
  }
}
