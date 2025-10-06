export interface MenuItem {
  id: string;
  label: string;
  icon: string;
  route: string;
  active?: boolean
}

export const MENU_ITEMS: MenuItem[] = [
  {
    id: 'dashbaord',
    label: 'Dashboard',
    icon: 'fa-square-poll-vertical',
    route: `dashboard`,
    active: true
  },
  {
    id: 'lanca-nfp',
    label: 'Lançar Nota Fiscal',
    icon: 'fa-robot',
    route: `lanca-nfp`,
  },
]