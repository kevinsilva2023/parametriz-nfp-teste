import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { NgxImageCompressService } from 'ngx-image-compress';
import { ToastrService } from 'ngx-toastr';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';

@Component({
  selector: 'app-editar-perfil',
  standalone: false,
  templateUrl: './editar-perfil.component.html',
  styles: ``
})
export class EditarPerfilComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  @ViewChild('inputFile') inputFile!: ElementRef;

  usuario!: any; // verificar tipo usuario

  perfilForm!: FormGroup;

  errors: [] = [];

  constructor(private formBuilder: FormBuilder,
    // private perfilService: PerfilService,
    private toastr: ToastrService,
    private imageCompresService: NgxImageCompressService
  ) {
    super();

    this.validationMessages = {
      nome: {
        required: 'Favor preencher o nome.'
      }
    }

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.perfilForm);
  }

  uploadImagem(event: any) {
    const arquivoImagem: File = event.target.files[0];
    if (!arquivoImagem) return;
    const tamanhoKb = arquivoImagem.size / 1024;
    const reader = new FileReader();
    reader.onload = (e: any) => {
      let imageDataUrl = e.target.result as string;
      if (tamanhoKb > 200) {
        this.imageCompresService.compressFile(imageDataUrl, -1, 50, 50)
          .then((imagemComprimida) => {
            this.usuario.fotoUpload = imagemComprimida;
          });
      } else {
        this.usuario.fotoUpload = imageDataUrl;
      }
    };
    reader.readAsDataURL(arquivoImagem);
  }

  ngOnInit(): void {
    this.perfilForm = this.formBuilder.group({
      nome: ['', [
        Validators.required
      ]],
    });
    this.obterPorId();
  }

  obterPorId() {
    // this.perfilService.obter()
    //   .subscribe({
    //     next: (response: Usuario) => {
    //       this.usuario = response;
    //       this.preencherForm();
    //     },
    //     error: (erro: any) => this.toastr.error('Erro ao obter usuário.', 'Erro')
    //   });
  }

  preencherForm() {
    this.perfilForm.patchValue({
      nome: this.usuario.nome,
      administrador: this.usuario.administrador
    });
  }

  efetuarEditarUsuario() {
    super.validarFormulario(this.perfilForm);

    this.usuario = Object.assign({}, this.usuario, this.perfilForm.value);

    // this.perfilService.salvar(this.usuario)
    //   .subscribe({
    //     next: (sucesso: any) => { this.processarSucesso(sucesso); },
    //     error: (falha: any) => { this.processarFalha(falha); }
    //   })
  }

  processarSucesso(response: any) {
    this.limparErros()
    this.toastr.success('Usuário editado com sucesso.', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível editar o usuário.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }
}

// confirmar nome dos campos do form