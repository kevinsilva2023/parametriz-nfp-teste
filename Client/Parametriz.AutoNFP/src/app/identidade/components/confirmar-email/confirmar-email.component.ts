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
  returnUrl: string;

  emailConfirmado = false;
  confirmacaoFalhou = false;
  nomeUsuario?: string;
  emailUsuario?: string;

  constructor(private activatedRoute: ActivatedRoute,
    private identidadeService: IdentidadeService,
    private router: Router,
    private toastr: ToastrService) {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];
  }

  ngOnInit(): void {
    this.verificaConfirmacaoDeEmail();
  }

  verificaConfirmacaoDeEmail() {
    const email = this.activatedRoute.snapshot.queryParamMap.get('email');
    const code = this.activatedRoute.snapshot.queryParamMap.get('code');
    const definirSenha = this.activatedRoute.snapshot.queryParamMap.get('definirSenha');

    if (email && code && definirSenha !== null) {
      const confirmarEmail: ConfirmarEmail = {
        email: email,
        code: code,
        definirSenha: definirSenha === 'True'
      }

      this.identidadeService.confirmarEmail(confirmarEmail)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso, confirmarEmail); },
          error: (falha: any) => { this.processarFalha(falha); }
        });

    } else {
      this.confirmacaoFalhou = true
    }
  }

  processarSucesso(response: any, confirmarEmail: ConfirmarEmail) {
    if (confirmarEmail.definirSenha) {
      this.toastr.success('Email confirmado! Aguarde, você será redirecionado...', 'Confirmação', {
        timeOut: 5000,
        progressBar: true,
        progressAnimation: 'increasing',
      });

      LocalStorageUtils.salvarDadosLocaisUsuario(response);

      setTimeout(() => {
        this.returnUrl
          ? this.router.navigate([this.returnUrl])
          : this.router.navigate(['/definir-senha-enviado']);
      }, 5000);

    } else {
      this.emailConfirmado = true;
      this.nomeUsuario = response?.nome ?? 'Usuário';
      this.emailUsuario = confirmarEmail.email;

      this.toastr.success('Aguarde, você será redirecionado para a página inicial...', 'Email confirmado!', {
        timeOut: 5000,
        progressBar: true,
        progressAnimation: 'increasing',
      });

      setTimeout(() => {
        this.returnUrl
          ? this.router.navigate([this.returnUrl])
          : this.router.navigate(['/']);
      }, 5000);
    }
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens ?? ['Falha desconhecida'];
    this.confirmacaoFalhou = true;

    this.toastr.error('Não foi possível confirmar o email.', 'Erro', {
      timeOut: 5000,
      progressBar: true,
      progressAnimation: 'increasing',
    });
  }
}
