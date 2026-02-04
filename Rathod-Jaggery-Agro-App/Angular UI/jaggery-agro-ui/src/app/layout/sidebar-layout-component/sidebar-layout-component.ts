import { Component } from '@angular/core';
import { SidebarComponent } from '../sidebar-component/sidebar-component';
import { HeaderComponent } from '../../shared/header.component/header.component';

import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar-layout',
  standalone: true,
  imports: [
    CommonModule,
    SidebarComponent,
    HeaderComponent,
    RouterOutlet
  ],
  template: `
    <div class="layout-wrapper">
      <app-sidebar></app-sidebar>

      <div class="main-section">
        <app-header></app-header>
        <router-outlet></router-outlet>
      </div>
    </div>
  `,
  styles: [`
    .layout-wrapper {
      display: flex;
      height: 100vh;
    }

    .main-section {
      flex: 1;
      display: flex;
      flex-direction: column;
    }
  `]
})
export class SidebarLayoutComponent {}