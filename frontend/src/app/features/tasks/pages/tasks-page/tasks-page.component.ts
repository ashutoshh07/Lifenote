import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Task {
  id: number;
  title: string;
  completed: boolean;
}

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tasks-page.component.html',
  styleUrls: ['./tasks-page.component.scss']
})
export class TasksPageComponent {
  tasks: Task[] = [
    { id: 1, title: 'Finish the report for Q2', completed: false },
    { id: 2, title: 'Schedule a meeting with the design team', completed: true },
    { id: 3, title: 'Buy groceries for the week', completed: false },
    { id: 4, title: 'Call the plumber to fix the sink', completed: false },
  ];
}
