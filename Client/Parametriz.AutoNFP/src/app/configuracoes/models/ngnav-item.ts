  export interface NgNavItem {
    label: string,
    icon: string,
    item: string,
    route: string,
  }

  export const NGNAV_ITEMS: NgNavItem[] = [
    {
      label: 'Voluntário',
      icon: 'fa-user',
      item: 'voluntario',
      route: '/configuracoes/voluntario'
    },
    {
      label: 'Usuário',
      icon: 'fa-users',
      item: 'usuario',
      route: '/configuracoes/usuario'
    }
  ]