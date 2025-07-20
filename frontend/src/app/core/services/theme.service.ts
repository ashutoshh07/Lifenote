import { Injectable } from '@angular/core';

export enum Theme {
  Light = 'light',
  Dark = 'dark',
  System = 'system',
}

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly themeKey = 'theme';

  constructor() {}

  getTheme(): Theme {
    return (localStorage.getItem(this.themeKey) as Theme) || Theme.System;
  }

  setTheme(theme: Theme): void {
    localStorage.setItem(this.themeKey, theme);
    this.applyTheme();
  }

  initializeTheme(): void {
    this.applyTheme();
    window
      .matchMedia('(prefers-color-scheme: dark)')
      .addEventListener('change', () => {
        if (this.getTheme() === Theme.System) {
          this.applyTheme();
        }
      });
  }

  private applyTheme(): void {
    const theme = this.getTheme();
    const prefersDark = window.matchMedia(
      '(prefers-color-scheme: dark)'
    ).matches;

    if (theme === Theme.Dark || (theme === Theme.System && prefersDark)) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }
}