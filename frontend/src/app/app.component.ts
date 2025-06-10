import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { LayoutService } from './core/services/layout.service';
import { DesktopSidebarComponent } from './layout/navigation/desktop-sidebar/desktop-sidebar.component';
import { MobileBottomNavComponent } from './layout/navigation/mobile-bottom-nav/mobile-bottom-nav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    MobileBottomNavComponent, 
    DesktopSidebarComponent
  ],
  template: `
    <div class="app-container">
      <!-- Desktop Sidebar -->
      <app-desktop-sidebar></app-desktop-sidebar>
      
      <!-- Main Content Area -->
      <main [class]="layoutService.mainContentClasses()">
        <router-outlet></router-outlet>
      </main>
      
      <!-- Mobile Bottom Navigation -->
      <app-mobile-bottom-nav></app-mobile-bottom-nav>
    </div>
  `,
  styles: [`
    .app-container {
      display: flex;
      min-height: 100vh;
    }

    .main-content {
      flex: 1;
      padding: 20px;
      transition: margin-left 0.3s ease;
    }

    .main-content--with-sidebar {
      margin-left: 280px; /* Sidebar width */
    }

    .main-content--with-bottom-nav {
      padding-bottom: 80px; /* Bottom nav height + safe area */
    }

    @media (max-width: 1023px) {
      .main-content--with-sidebar {
        margin-left: 0; /* Reset on mobile */
      }
    }
  `]
})
export class AppComponent {
  layoutService = inject(LayoutService);
}