import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-confirmar-email-enviado',
  standalone: false,
  templateUrl: './confirmar-email-enviado.component.html',
})
export class ConfirmarEmailEnviadoComponent implements OnInit {
  email!: string;
  usuario!: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.email = this.route.snapshot.queryParamMap.get('email')!;
    this.usuario = this.route.snapshot.queryParamMap.get('usuario')!;
  }
}
