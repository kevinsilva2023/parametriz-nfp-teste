import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './registrar.component.html',
  styleUrl: './registrar.component.scss',
  standalone: false
})
export class RegistrarComponent {
  cadastroForm!: FormGroup;

  constructor(
  ) {}
}
