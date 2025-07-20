import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { DesktopSidebarComponent } from './layout/navigation/desktop-sidebar/desktop-sidebar.component';
import { MobileBottomNavComponent } from './layout/navigation/mobile-bottom-nav/mobile-bottom-nav.component';
import { LayoutService } from './core/services/layout.service';
import { ThemeService } from './core/services/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    DesktopSidebarComponent,
    MobileBottomNavComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  layoutService = inject(LayoutService);

  constructor(private themeService: ThemeService) {
    this.themeService.initializeTheme();
  }
}
