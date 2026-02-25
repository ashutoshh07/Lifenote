import { Injectable } from '@angular/core';
import { BehaviorSubject, interval, Subscription } from 'rxjs';

export type PomodoroType = 'pomodoro' | 'short-break' | 'long-break';

export interface PomodoroTimer {
  id: string;
  label: string;
  type: PomodoroType;
  minutes: number;
  seconds: number;
  running: boolean;
}

const DEFAULT_DURATIONS: Record<PomodoroType, number> = {
  'pomodoro': 25,
  'short-break': 5,
  'long-break': 15
};

function generateId(): string {
  return 'pomo-' + Date.now() + '-' + Math.random().toString(36).slice(2, 9);
}

@Injectable({
  providedIn: 'root'
})
export class PomodoroService {
  private timers$ = new BehaviorSubject<PomodoroTimer[]>([]);
  private tickSub: Subscription | null = null;

  /** All timers; subscribe in components. */
  getTimers$() {
    return this.timers$.asObservable();
  }

  get timers(): PomodoroTimer[] {
    return this.timers$.value;
  }

  constructor() {
    this.ensureAtLeastOne();
  }

  private ensureAtLeastOne(): void {
    if (this.timers$.value.length === 0) {
      this.addTimer();
    }
  }

  addTimer(): PomodoroTimer {
    const id = generateId();
    const type: PomodoroType = 'pomodoro';
    const timer: PomodoroTimer = {
      id,
      label: `Focus ${this.timers$.value.length + 1}`,
      type,
      minutes: DEFAULT_DURATIONS[type],
      seconds: 0,
      running: false
    };
    this.timers$.next([...this.timers$.value, timer]);
    this.startTickIfNeeded();
    return timer;
  }

  removeTimer(id: string): void {
    const list = this.timers$.value.filter(t => t.id !== id);
    if (list.length === 0) {
      this.addTimer();
      return;
    }
    this.timers$.next(list);
    this.stopTickIfIdle();
  }

  setType(id: string, type: PomodoroType): void {
    this.updateTimer(id, t => ({
      ...t,
      type,
      minutes: DEFAULT_DURATIONS[type],
      seconds: 0,
      running: false
    }));
  }

  setLabel(id: string, label: string): void {
    this.updateTimer(id, t => ({ ...t, label: label.trim() || t.label }));
  }

  start(id: string): void {
    this.updateTimer(id, t => ({ ...t, running: true }));
    this.startTickIfNeeded();
  }

  stop(id: string): void {
    this.updateTimer(id, t => ({ ...t, running: false }));
    this.stopTickIfIdle();
  }

  reset(id: string): void {
    const type = this.timers$.value.find(t => t.id === id)?.type ?? 'pomodoro';
    this.updateTimer(id, t => ({
      ...t,
      running: false,
      minutes: DEFAULT_DURATIONS[type],
      seconds: 0
    }));
  }

  formatTime(timer: PomodoroTimer): string {
    const m = timer.minutes.toString().padStart(2, '0');
    const s = timer.seconds.toString().padStart(2, '0');
    return `${m}:${s}`;
  }

  private updateTimer(id: string, fn: (t: PomodoroTimer) => PomodoroTimer): void {
    this.timers$.next(
      this.timers$.value.map(t => t.id === id ? fn(t) : t)
    );
  }

  private startTickIfNeeded(): void {
    if (this.tickSub || !this.timers$.value.some(t => t.running)) return;
    this.tickSub = interval(1000).subscribe(() => this.tick());
  }

  private stopTickIfIdle(): void {
    if (!this.timers$.value.some(t => t.running) && this.tickSub) {
      this.tickSub.unsubscribe();
      this.tickSub = null;
    }
  }

  private tick(): void {
    const next = this.timers$.value.map(t => {
      if (!t.running) return t;
      let { minutes, seconds } = t;
      if (seconds > 0) seconds--;
      else if (minutes > 0) {
        minutes--;
        seconds = 59;
      } else {
        return { ...t, running: false };
      }
      return { ...t, minutes, seconds };
    });
    this.timers$.next(next);
    this.stopTickIfIdle();
  }

}
