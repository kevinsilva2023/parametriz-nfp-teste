import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-definir-senha-enviado',
  standalone: false,
  templateUrl: './definir-senha-enviado.component.html',
})
export class DefinirSenhaEnviadoComponent {
  email!: string;
  usuario!: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.email = this.route.snapshot.queryParamMap.get('email')!;
    this.usuario = this.route.snapshot.queryParamMap.get('usuario')!;
  }
}
