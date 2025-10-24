import { Component, OnInit } from '@angular/core';
import { ConfirmarEmail } from '../../models/confirmar-email';
import { ActivatedRoute, Router } from '@angular/router';
import { IdentidadeService } from '../../services/identidade.service';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-confirmar-email',
  standalone: false,
  templateUrl: './confirmar-email.component.html',
})
export class ConfirmarEmailComponent implements OnInit {
  errors: any[] = [];
  emailConfirmado = false;
  confirmacaoFalhou = false;
  nomeUsuario?: string;
  emailUsuario?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private identidadeService: IdentidadeService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.verificaConfirmacaoDeEmail();
  }

  verificaConfirmacaoDeEmail() {
    let email = this.activatedRoute.snapshot.queryParamMap.get('email');
    let code = this.activatedRoute.snapshot.queryParamMap.get('code');

    if (email && code !== null) {
      let confirmarEmail = new ConfirmarEmail();
      confirmarEmail.email = email;
      confirmarEmail.code = code;

      this.identidadeService.confirmarEmail(confirmarEmail)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarFalha(falha); }
        });

    } else {
      this.confirmacaoFalhou = true
    }
  }

  limparErros() {
    this.errors = [];
  }

  processarSucesso() {
    let toast = this.toastr.success('Email confirmado! Você será redirecionado.', 'Confirmação');

    toast.onHidden
      .subscribe({
        next: () => this.router.navigate(['/definir-senha-enviado'])
      });
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens ?? ['Falha desconhecida'];
    this.confirmacaoFalhou = true;

    this.toastr.error('Não foi possível confirmar o email.', 'Erro')
  }
}
