import { Component, OnInit } from '@angular/core';
import { Voluntario } from '../../models/voluntario';
import { debounceTime, Subject } from 'rxjs';
import { VoluntarioService } from '../../services/voluntario.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IdentidadeService } from 'src/app/identidades/services/identidade.service';
import { ToastrService } from 'ngx-toastr';
import { CadastrarVoluntarioComponent } from '../cadastrar-voluntario/cadastrar-voluntario.component';
import { DesativarVoluntarioComponent } from '../desativar-voluntario/desativar-voluntario.component';
import { AtivarVoluntarioComponent } from '../ativar-voluntario/ativar-voluntario.component';
import { InputUtils } from 'src/app/shared/utils/input-utils';

@Component({
  selector: 'app-listar-voluntario',
  standalone: false,
  templateUrl: './listar-voluntario.component.html',
  styles: [`
    .sticky-top {
      z-index: 999 !important;
    }
    ::ng-deep .mat-mdc-form-field-subscript-wrapper {
      display: none !important;
    }
    .img-perfil {
      width: 40px;
      height: 40px;
      object-fit: cover;
      border-radius: 50%;
      vertical-align: middle;
    }
    td {
      height: 48px;
      vertical-align: middle !important;
    }
  `]
})
export class ListarVoluntarioComponent implements OnInit {

  //verificar se email ta ativo

  inputUtils = InputUtils;

  voluntariosCadastrados!: Voluntario[];

  filtroNomeVoluntario = '';
  filtroEmailVoluntario = '';
  filtroAcesso = 2;
  filtroDesativado = 0;

  errors: [] = [];

  debounceNomeVoluntario = new Subject<string>();
  debounceEmailVoluntario = new Subject<string>();

  constructor(
    private voluntarioService: VoluntarioService,
    private modalService: NgbModal,
    private indentidadeService: IdentidadeService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.debounceNomeVoluntario
      .pipe(debounceTime(500))
      .subscribe({
        next: (nomeVoluntario: string) => {
          this.filtroNomeVoluntario = nomeVoluntario;
          this.obterPorFiltro();
        }
      });

    this.debounceEmailVoluntario
      .pipe(debounceTime(500))
      .subscribe({
        next: (emailVoluntario: string) => {
          this.filtroEmailVoluntario = emailVoluntario;
          this.obterPorFiltro();
        }
      })
    this.obterPorFiltro();
  }

  alterarFiltroAcesso(event: number) {
    this.filtroAcesso = event;
    this.obterPorFiltro();
  }

  alterarFiltroDesativado(event: number) {
    this.filtroDesativado = event;
    this.obterPorFiltro();
  }

  obterPorFiltro() {
    this.voluntarioService.obterPorFiltro(this.filtroNomeVoluntario, this.filtroEmailVoluntario,
      this.filtroAcesso, this.filtroDesativado)
      .subscribe({
        next: (voluntariosCadastrados: Voluntario[]) => {
          this.voluntariosCadastrados = voluntariosCadastrados
        }
      })
  }

  cadastrar() {
    let modalRef = this.modalService.open(CadastrarVoluntarioComponent, { size: 'lg', centered: false });

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  desativar(voluntario: Voluntario) {
    let modalRef = this.modalService.open(DesativarVoluntarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.voluntario = voluntario;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  ativar(voluntario: Voluntario) {
    let modalRef = this.modalService.open(AtivarVoluntarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.voluntario = voluntario;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  habilitarVoluntarioAdm(voluntario: Voluntario, event: any) {
    voluntario.administrador = event.checked;

    this.voluntarioService.editar(voluntario)
  }

  enviarConfirmarEmail(voluntario: Voluntario) {
    this.indentidadeService.enviarConfirmarEmail({
      voluntarioId: voluntario.id,
      definirSenha: false,
    })
      .subscribe({
        next: () => { this.processarSucesso(); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  enviarDefinirSenha(voluntario: Voluntario) {
    this.indentidadeService.enviarDefinirSenha({ email: voluntario.email })
      .subscribe({
        next: () => { this.processarSucesso(); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso() {
    this.limparErros()
    this.toastr.success('Email enviado.', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível enviar o email.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }
}

