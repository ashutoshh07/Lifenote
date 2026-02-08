import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialog } from '@angular/material/dialog';
import { Habit } from '../models/habit.model';
import { HabitService } from '../services/habit.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CheckInDialogComponent } from '../check-in-dialog/check-in-dialog.component';
import { HabitFormComponent } from '../habit-form/habit-form.component';

@Component({
  selector: 'app-habit-list',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule  // Add this
  ],
  templateUrl: './habit-list.component.html',
  styleUrl: './habit-list.component.scss'
})
export class HabitListComponent implements OnInit {
  habits: Habit[] = [];
  loading = true;
  completedToday = 0;
  totalHabits = 0;
  completionPercentage = 0;

  constructor(
    private habitService: HabitService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar  // Add this
  ) { }

  ngOnInit(): void {
    this.loadHabits();
  }

  loadHabits(): void {
    this.loading = true;
    this.habitService.getHabits().subscribe({
      next: (habits) => {
        this.habits = habits;
        this.calculateProgress();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading habits:', error);
        this.loading = false;
      }
    });
  }

  calculateProgress(): void {
    this.totalHabits = this.habits.length;
    this.completedToday = this.habits.filter(h => h.completedToday).length;
    this.completionPercentage = this.totalHabits > 0
      ? (this.completedToday / this.totalHabits) * 100
      : 0;
  }

  // Update onCheckIn method
  onCheckIn(habit: Habit): void {
    const dialogRef = this.dialog.open(CheckInDialogComponent, {
      width: '500px',
      data: { habit },
      disableClose: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.success) {
        this.snackBar.open(
          `🎉 Great job! Your streak is now ${result.log.currentStreak} days!`,
          'Close',
          { duration: 5000 }
        );
        this.loadHabits(); // Refresh the list
      }
    });
  }

  onViewStats(habit: Habit): void {
    // TODO: Navigate to stats page
    console.log('View stats:', habit.name);
  }

  onCreateHabit(): void {
    const dialogRef = this.dialog.open(HabitFormComponent, {
      width: '600px',
      maxHeight: '90vh',
      data: {}, // No habit = create mode
      disableClose: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.success) {
        this.snackBar.open(
          `✅ Habit "${result.habit.name}" created successfully!`,
          'Close',
          { duration: 4000 }
        );
        this.loadHabits(); // Refresh the list
      }
    });
  }

  // Add method to edit habit
  onEditHabit(habit: Habit): void {
    const dialogRef = this.dialog.open(HabitFormComponent, {
      width: '600px',
      maxHeight: '90vh',
      data: { habit }, // Pass habit = edit mode
      disableClose: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.success) {
        this.snackBar.open(
          `✅ Habit "${result.habit.name}" updated successfully!`,
          'Close',
          { duration: 4000 }
        );
        this.loadHabits(); // Refresh the list
      }
    });
  }
}
