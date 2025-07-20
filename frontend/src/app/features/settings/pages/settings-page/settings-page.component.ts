import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Theme, ThemeService } from '../../../../core/services/theme.service';
import { LayoutService } from '../../../../core/services/layout.service';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss'],
})
export class SettingsPageComponent implements OnInit {
  themeService = inject(ThemeService);
  fb = inject(FormBuilder);
  settingsForm = this.fb.group({
    theme: [this.themeService.getTheme()],
  });
  themes = Object.values(Theme);
  theme = Theme;
  currentTheme: Theme = this.themeService.getTheme();
  private layoutService = inject(LayoutService);

  constructor(
    // private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.settingsForm.get('theme')?.valueChanges.subscribe((theme) => {
      if (theme) {
        this.setTheme(theme);
      }
    });
  }

  setTheme(theme: Theme): void {
    this.themeService.setTheme(theme);
    this.currentTheme = theme;
    this.settingsForm.get('theme')?.setValue(theme, { emitEvent: false });
  }

  onClicko(event: any) {
    console.log(event.target.value);    
  }
}
