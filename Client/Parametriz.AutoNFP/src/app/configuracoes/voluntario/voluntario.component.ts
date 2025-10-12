import { Component } from '@angular/core';

@Component({
  selector: 'app-voluntario',
  standalone: false,
  templateUrl: './voluntario.component.html',
})

export class VoluntarioComponent {
  certificadoCadastrado = false;

  onCertificadoStatusChange(status: boolean) {
    this.certificadoCadastrado = status;
  }

}
