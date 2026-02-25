import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LucideAngularModule } from 'lucide-angular';
import { PomodoroService, PomodoroTimer, PomodoroType } from '../../services/pomodoro.service';

@Component({
  selector: 'app-pomodoro-page',
  standalone: true,
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './pomodoro-page.component.html',
  styleUrls: ['./pomodoro-page.component.scss'],
})
export class PomodoroPageComponent {
  pomodoroService = inject(PomodoroService);
  timers$ = this.pomodoroService.getTimers$();

  addTimer(): void {
    this.pomodoroService.addTimer();
  }

  removeTimer(id: string): void {
    this.pomodoroService.removeTimer(id);
  }

  setType(id: string, type: PomodoroType): void {
    this.pomodoroService.setType(id, type);
  }

  start(id: string): void {
    this.pomodoroService.start(id);
  }

  stop(id: string): void {
    this.pomodoroService.stop(id);
  }

  reset(id: string): void {
    this.pomodoroService.reset(id);
  }

  formatTime(timer: PomodoroTimer): string {
    return this.pomodoroService.formatTime(timer);
  }
}