import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { LayoutService } from '../../../core/services/layout.service';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-mobile-bottom-nav',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <nav class="bottom-nav" *ngIf="layoutService.showBottomNav()">
      <div class="bottom-nav__container">
        <a 
          *ngFor="let item of layoutService.navItems()" 
          [routerLink]="item.route"
          routerLinkActive="bottom-nav__item--active"
          class="bottom-nav__item"
          (click)="onNavItemClick(item.route)"
        >
          <div class="bottom-nav__icon-wrapper">
            <lucide-icon [name]="item.icon"></lucide-icon>
            <span 
              *ngIf="item.badge && item.badge > 0" 
              class="bottom-nav__badge"
            >
              {{ item.badge }}
            </span>
          </div>
          <span class="bottom-nav__label">{{ item.label }}</span>
        </a>
      </div>
    </nav>
  `,
  styles: [`
    .bottom-nav {
      position: fixed;
      bottom: 0;
      left: 0;
      right: 0;
      background: white;
      border-top: 1px solid #e5e7eb;
      box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
      z-index: 1000;
      padding-bottom: env(safe-area-inset-bottom); /* iOS safe area */
    }

    .bottom-nav__container {
      display: flex;
      justify-content: space-around;
      align-items: center;
      padding: 8px 0;
      max-width: 100%;
    }

    .bottom-nav__item {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 8px 12px;
      text-decoration: none;
      color: #6b7280;
      transition: color 0.2s ease;
      min-width: 60px;
    }

    .bottom-nav__item--active {
      color: #3b82f6; /* Primary blue */
    }

    .bottom-nav__icon-wrapper {
      position: relative;
      margin-bottom: 4px;
    }

    .bottom-nav__badge {
      position: absolute;
      top: -8px;
      right: -8px;
      background: #ef4444;
      color: white;
      border-radius: 12px;
      padding: 2px 6px;
      font-size: 10px;
      font-weight: 600;
      min-width: 16px;
      text-align: center;
    }

    .bottom-nav__label {
      font-size: 12px;
      font-weight: 500;
    }

    /* Dark mode support */
    @media (prefers-color-scheme: dark) {
      .bottom-nav {
        background: #1f2937;
        border-top-color: #374151;
      }
      
      .bottom-nav__item {
        color: #9ca3af;
      }
      
      .bottom-nav__item--active {
        color: #60a5fa;
      }
    }
  `]
})
export class MobileBottomNavComponent {
  layoutService = inject(LayoutService);
  private router = inject(Router);

  onNavItemClick(route: string): void {
    // Add haptic feedback for mobile
    if ('vibrate' in navigator) {
      navigator.vibrate(50);
    }
    
    this.router.navigate([route]);
  }
}