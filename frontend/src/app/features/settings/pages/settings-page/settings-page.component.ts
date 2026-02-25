import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { LucideAngularModule, Sun, Moon, Monitor, Bell, User, Shield, Info } from 'lucide-angular';
import { Theme, ThemeService } from '../../../../core/services/theme.service';
import { LayoutService } from '../../../../core/services/layout.service';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LucideAngularModule],
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss'],
})
export class SettingsPageComponent implements OnInit {
  themeService = inject(ThemeService);
  fb = inject(FormBuilder);
  settingsForm = this.fb.group({
    theme: [this.themeService.getTheme()],
  });
  theme = Theme;
  currentTheme: Theme = this.themeService.getTheme();

  SunIcon = Sun;
  MoonIcon = Moon;
  MonitorIcon = Monitor;
  BellIcon = Bell;
  UserIcon = User;
  ShieldIcon = Shield;
  InfoIcon = Info;

  ngOnInit(): void {
    this.settingsForm.get('theme')?.valueChanges.subscribe((t) => {
      if (t) this.setTheme(t);
    });
  }

  setTheme(theme: Theme): void {
    this.themeService.setTheme(theme);
    this.currentTheme = theme;
    this.settingsForm.get('theme')?.setValue(theme, { emitEvent: false });
  }
}
