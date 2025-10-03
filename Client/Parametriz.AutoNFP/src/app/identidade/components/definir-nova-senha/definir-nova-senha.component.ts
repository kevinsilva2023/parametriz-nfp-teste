import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-definir-nova-senha',
  standalone: false,
  templateUrl: './definir-nova-senha.component.html',
  styleUrl: './definir-nova-senha.component.scss'
})
export class DefinirNovaSenhaComponent implements OnInit {
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
