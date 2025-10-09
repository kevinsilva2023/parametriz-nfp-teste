import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { MENU_ITEMS, MenuItem } from '../../models/menu-item';
import { filter } from 'rxjs';

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
              private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.itemAtivo(this.router.url);

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.itemAtivo(event.urlAfterRedirects);
      });
  }

  itemAtivo(currentUrl: string): void {
    const urlLimpa = currentUrl.replace(/^\//, '');

    this.menuItems.forEach(item => {
      item.active = urlLimpa.includes(item.route)
    });
  }
 
  onMenuItemClick(item: MenuItem): void {
    this.menuItems.forEach(i => i.active = i.id == item.id)
    this.router.navigate([item.route], { relativeTo: this.route });
  }
}
