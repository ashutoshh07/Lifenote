import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="settings-page">
      <header class="page-header">
        <h1>⚙️ Settings</h1>
        <p>Customize your productivity experience</p>
      </header>
      
      <div class="settings-content">
        <div class="placeholder-card">
          <h3>🎛️ App Configuration</h3>
          <p>Your settings panel will go here</p>
          <div class="settings-preview">
            <div class="setting-group">
              <h4>🍅 Pomodoro Settings</h4>
              <div class="setting-item">Focus Duration: 25 minutes</div>
              <div class="setting-item">Short Break: 5 minutes</div>
              <div class="setting-item">Long Break: 15 minutes</div>
            </div>
            <div class="setting-group">
              <h4>🎨 Appearance</h4>
              <div class="setting-item">Theme: Auto</div>
              <div class="setting-item">Notifications: Enabled</div>
            </div>
          </div>
          <ul>
            <li>Timer customization</li>
            <li>Theme preferences</li>
            <li>Notification settings</li>
            <li>Data export/import</li>
          </ul>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .settings-page {
      max-width: 1200px;
      margin: 0 auto;
    }

    .page-header {
      margin-bottom: 2rem;
      text-align: center;
    }

    .page-header h1 {
      font-size: 2.5rem;
      color: #7c3aed;
      margin-bottom: 0.5rem;
    }

    .page-header p {
      color: #6b7280;
      font-size: 1.1rem;
    }

    .placeholder-card {
      background: white;
      border-radius: 12px;
      padding: 2rem;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      text-align: center;
    }

    .settings-preview {
      margin: 1.5rem 0;
      text-align: left;
      max-width: 400px;
      margin-left: auto;
      margin-right: auto;
    }

    .setting-group {
      margin-bottom: 1.5rem;
      padding: 1rem;
      background: #f9fafb;
      border-radius: 8px;
    }

    .setting-group h4 {
      margin: 0 0 0.5rem 0;
      color: #374151;
      font-size: 1rem;
    }

    .setting-item {
      padding: 0.25rem 0;
      color: #6b7280;
      font-size: 0.9rem;
    }

    .placeholder-card h3 {
      font-size: 1.5rem;
      margin-bottom: 1rem;
      color: #374151;
    }

    .placeholder-card ul {
      text-align: left;
      max-width: 300px;
      margin: 1rem auto;
    }

    .placeholder-card li {
      margin: 0.5rem 0;
      color: #6b7280;
    }
  `]
})
export class SettingsPageComponent {}