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
      label: 'Usuários',
      icon: 'fa-users',
      item: 'voluntario2',
      route: '/configuracoes/voluntario2'
    }
  ]