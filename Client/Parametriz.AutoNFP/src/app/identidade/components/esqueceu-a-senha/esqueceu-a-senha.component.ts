import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-esqueceu-a-senha',
  standalone: false,
  templateUrl: './esqueceu-a-senha.component.html',
})
export class EsqueceuASenhaComponent implements OnInit {
  esqueceuASenhaForm!: FormGroup

  constructor(
    private formBuilder: FormBuilder
  ) {}


  ngOnInit(): void {
    this.esqueceuASenhaForm = this.formBuilder.group({
      email: [null, [
        Validators.required,
        Validators.email
      ]]
    })
  }

}
