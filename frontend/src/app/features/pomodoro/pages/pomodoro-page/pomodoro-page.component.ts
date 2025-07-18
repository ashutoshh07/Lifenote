import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PomodoroService } from '../../services/pomodoro.service';

@Component({
  selector: 'app-pomodoro-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pomodoro-page.component.html',
  styleUrls: ['./pomodoro-page.component.scss'],
  providers: [PomodoroService]
})
export class PomodoroPageComponent {
  constructor(public pomodoroService: PomodoroService) {}
}

