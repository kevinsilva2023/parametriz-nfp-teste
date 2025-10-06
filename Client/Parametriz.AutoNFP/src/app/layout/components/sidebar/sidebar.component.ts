import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MENU_ITEMS, MenuItem } from '../../models/menu-item';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  isCollapsed = false;

  menuItems: MenuItem[] = MENU_ITEMS;
  
  constructor(private router: Router,
              private route: ActivatedRoute
  ) {}
    toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  onMenuItemClick(item: MenuItem): void {
    this.menuItems.forEach(i => i.active = i.id == item.id)
    this.router.navigate([item.route], { relativeTo: this.route });
  }
}
