import { Component, OnInit } from '@angular/core';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-usuario',
  standalone: false,
  templateUrl: './usuario.component.html',
})
export class UsuarioComponent  {

  isAdministrador = true;


}
