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

  constructor(private activatedRoute: ActivatedRoute,
              private identidadeService: IdentidadeService,
              private router: Router,
              private toastr: ToastrService) {}

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

  limparErros() {
    this.errors = [];
  }

  processarSucesso(response: any, confirmarEmail: ConfirmarEmail) {
    if (confirmarEmail.definirSenha) {
      let toast = this.toastr.success('Email confirmado! Você será redirecionado.', 'Confirmação');

      toast.onHidden
        .subscribe({
          next: () => this.router.navigate(['/definir-senha-enviado'])
        });
    } else {
      this.emailConfirmado = true;
      this.nomeUsuario = response?.nome;
      this.emailUsuario = confirmarEmail.email;

      let toast = this.toastr.success('Aguarde, clique no botao para acessar o AutoNFP', 'Email confirmado!');
      
      toast.onHidden
        .subscribe({
          next: () => this.router.navigate(['/'])
        });
    }
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens ?? ['Falha desconhecida'];
    this.confirmacaoFalhou = true;

    this.toastr.error('Não foi possível confirmar o email.', 'Erro')
  }
}
