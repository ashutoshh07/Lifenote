import { Injectable } from '@angular/core';
import { BehaviorSubject, interval, Subscription } from 'rxjs';

export type PomodoroType = 'pomodoro' | 'short-break' | 'long-break';

@Injectable({
  providedIn: 'root'
})
export class PomodoroService {
  private timer$: Subscription | null = null;
  
  type: PomodoroType = 'pomodoro';
  private running = false;

  private minutes = 25;
  private seconds = 0;

  time$ = new BehaviorSubject<string>(this.formatTime());
  running$ = new BehaviorSubject<boolean>(this.running);
  type$ = new BehaviorSubject<PomodoroType>(this.type);

  constructor() { }

  start() {
    if (this.running) return;

    this.running = true;
    this.running$.next(this.running);
    this.timer$ = interval(1000).subscribe(() => {
      if (this.seconds > 0) {
        this.seconds--;
      } else if (this.minutes > 0) {
        this.minutes--;
        this.seconds = 59;
      } else {
        this.stop();
        // Handle completion
      }
      this.time$.next(this.formatTime());
    });
  }

  stop() {
    if (!this.running) return;

    this.running = false;
    this.running$.next(this.running);
    if (this.timer$) {
      this.timer$.unsubscribe();
      this.timer$ = null;
    }
  }

  reset() {
    this.stop();
    this.setType(this.type);
  }

  setType(type: PomodoroType) {
    this.type = type;
    this.type$.next(this.type);
    switch (type) {
      case 'pomodoro':
        this.minutes = 25;
        break;
      case 'short-break':
        this.minutes = 5;
        break;
      case 'long-break':
        this.minutes = 15;
        break;
    }
    this.seconds = 0;
    this.time$.next(this.formatTime());
  }

  private formatTime(): string {
    return `${this.minutes.toString().padStart(2, '0')}:${this.seconds.toString().padStart(2, '0')}`;
  }
}
