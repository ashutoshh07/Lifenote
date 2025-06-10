import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { LayoutService } from '../../../core/services/layout.service';
import { BreakpointService } from '../../../core/services/breakpoint.service';

@Component({
  selector: 'app-desktop-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <aside 
      class="sidebar"
      [class.sidebar--open]="layoutService.showSidebar()"
    >
      <div class="sidebar__header">
        <h1 class="sidebar__title">Focus Flow</h1>
        <button 
          class="sidebar__close-btn"
          (click)="layoutService.closeSidebar()"
        >
          <lucide-icon name="x"></lucide-icon>
        </button>
      </div>
      
      <nav class="sidebar__nav">
        <a 
          *ngFor="let item of layoutService.navItems()"
          [routerLink]="item.route"
          routerLinkActive="sidebar__item--active"
          class="sidebar__item"
        >
          <lucide-icon [name]="item.icon"></lucide-icon>
          <span class="sidebar__label">{{ item.label }}</span>
          <span 
            *ngIf="item.badge && item.badge > 0"
            class="sidebar__badge"
          >
            {{ item.badge }}
          </span>
        </a>
      </nav>
    </aside>
    
    <!-- Overlay for mobile sidebar -->
    <div 
      *ngIf="layoutService.sidebarOpen() && !breakpointService.isDesktop()"
      class="sidebar__overlay"
      (click)="layoutService.closeSidebar()"
    ></div>
  `,
  styles: [`
    .sidebar {
      position: fixed;
      top: 0;
      left: -280px; /* Hidden by default */
      width: 280px;
      height: 100vh;
      background: white;
      border-right: 1px solid #e5e7eb;
      transition: left 0.3s ease;
      z-index: 1100;
      overflow-y: auto;
    }

    .sidebar--open {
      left: 0;
    }

    .sidebar__header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20px;
      border-bottom: 1px solid #e5e7eb;
    }

    .sidebar__title {
      font-size: 20px;
      font-weight: 700;
      color: #1f2937;
      margin: 0;
    }

    .sidebar__nav {
      padding: 20px 0;
    }

    .sidebar__item {
      display: flex;
      align-items: center;
      padding: 12px 20px;
      text-decoration: none;
      color: #6b7280;
      transition: all 0.2s ease;
      border-left: 3px solid transparent;
    }

    .sidebar__item:hover {
      background: #f9fafb;
      color: #374151;
    }

    .sidebar__item--active {
      background: #eff6ff;
      color: #3b82f6;
      border-left-color: #3b82f6;
    }

    .sidebar__label {
      margin-left: 12px;
      font-weight: 500;
    }

    .sidebar__badge {
      margin-left: auto;
      background: #ef4444;
      color: white;
      border-radius: 12px;
      padding: 2px 8px;
      font-size: 12px;
      font-weight: 600;
    }

    .sidebar__overlay {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(0, 0, 0, 0.5);
      z-index: 1050;
    }

    /* Desktop styles */
    @media (min-width: 1024px) {
      .sidebar {
        position: relative;
        left: 0 !important;
        height: 100vh;
      }
      
      .sidebar__close-btn {
        display: none;
      }
    }
  `]
})
export class DesktopSidebarComponent {
  layoutService = inject(LayoutService);
  breakpointService = inject(BreakpointService);
}