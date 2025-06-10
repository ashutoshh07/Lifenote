import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pomodoro-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="pomodoro-page">
      <header class="page-header">
        <h1>🍅 Pomodoro Timer</h1>
        <p>Focus sessions made simple</p>
      </header>
      
      <div class="pomodoro-content">
        <div class="placeholder-card">
          <h3>⏰ Timer Interface</h3>
          <p>Your Pomodoro timer will go here</p>
          <div class="timer-preview">25:00</div>
          <ul>
            <li>25-minute focus sessions</li>
            <li>5-minute short breaks</li>
            <li>15-minute long breaks</li>
            <li>Session tracking & statistics</li>
          </ul>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .pomodoro-page {
      max-width: 1200px;
      margin: 0 auto;
    }

    .page-header {
      margin-bottom: 2rem;
      text-align: center;
    }

    .page-header h1 {
      font-size: 2.5rem;
      color: #dc2626;
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

    .timer-preview {
      font-size: 3rem;
      font-weight: 700;
      color: #dc2626;
      margin: 1rem 0;
      font-family: monospace;
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
export class PomodoroPageComponent {}