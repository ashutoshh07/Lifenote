import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PomodoroService } from '../../services/pomodoro.service';
import { LayoutService } from '../../../../core/services/layout.service';

@Component({
  selector: 'app-pomodoro-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pomodoro-page.component.html',
  styleUrls: ['./pomodoro-page.component.scss'],
})
export class PomodoroPageComponent implements OnInit {
  private layoutService = inject(LayoutService);
  constructor(public pomodoroService: PomodoroService) {}

  ngOnInit(): void {
  }
}