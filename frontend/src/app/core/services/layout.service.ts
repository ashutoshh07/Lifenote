import { Injectable, signal, computed, inject } from '@angular/core';
import { BreakpointService } from './breakpoint.service';

export interface NavigationItem {
  id: string;
  label: string;
  icon: string;
  route: string;
  badge?: number; // For notifications/counts
}

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  private breakpointService = inject(BreakpointService);
  
  // Navigation state
  private sidebarOpenSignal = signal<boolean>(false);
  private navigationItems = signal<NavigationItem[]>([
    {
      id: 'tasks',
      label: 'Tasks',
      icon: 'list-checks', // Lucide icon name
      route: '/tasks',
      badge: 0
    },
    {
      id: 'pomodoro', 
      label: 'Timer',
      icon: 'timer',
      route: '/pomodoro'
    },
    {
      id: 'habits',
      label: 'Habits', 
      icon: 'trending-up',
      route: '/habits'
    },
    {
      id: 'settings',
      label: 'Settings',
      icon: 'settings',
      route: '/settings'
    }
  ]);

  // Public readonly signals
  sidebarOpen = this.sidebarOpenSignal.asReadonly();
  navItems = this.navigationItems.asReadonly();
  
  // Computed layout decisions
  showSidebar = computed(() => 
    this.breakpointService.isDesktop() && this.sidebarOpen()
  );
  
  showBottomNav = computed(() => 
    this.breakpointService.isMobile()
  );
  
  showMobileHeader = computed(() => 
    this.breakpointService.isMobile()
  );

  // Layout classes for main content area
  mainContentClasses = computed(() => {
    const classes = ['main-content'];
    
    if (this.showSidebar()) {
      classes.push('main-content--with-sidebar');
    }
    
    if (this.showBottomNav()) {
      classes.push('main-content--with-bottom-nav');
    }
    
    return classes.join(' ');
  });

  // Actions
  toggleSidebar(): void {
    this.sidebarOpenSignal.update(open => !open);
  }

  closeSidebar(): void {
    this.sidebarOpenSignal.set(false);
  }

  openSidebar(): void {
    this.sidebarOpenSignal.set(true);
  }

  updateBadge(itemId: string, count: number): void {
    this.navigationItems.update(items => 
      items.map(item => 
        item.id === itemId ? { ...item, badge: count } : item
      )
    );
  }
}