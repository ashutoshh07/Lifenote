import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LucideAngularModule } from 'lucide-angular';
import { Habit } from '../models/habit.model';
import { HabitService } from '../services/habit.service';
import { CheckInDialogComponent } from '../check-in-dialog/check-in-dialog.component';
import { HabitFormComponent } from '../habit-form/habit-form.component';
import { Trophy, Flame, Target, CheckCircle, Circle, Edit3, BarChart2, Sparkles, Plus } from 'lucide-angular';

@Component({
  selector: 'app-habit-list',
  standalone: true,
  imports: [
    CommonModule,
    MatSnackBarModule,
    LucideAngularModule,
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

  TrophyIcon = Trophy;
  FlameIcon = Flame;
  TargetIcon = Target;
  CheckCircleIcon = CheckCircle;
  CircleIcon = Circle;
  EditIcon = Edit3;
  BarChartIcon = BarChart2;
  SparklesIcon = Sparkles;
  PlusIcon = Plus;

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
    const dialogConfig: MatDialogConfig<{ habit: Habit }> = {
      width: '100%',
      maxWidth: '440px',
      maxHeight: '90vh',
      data: { habit },
      disableClose: false,
      panelClass: 'check-in-dialog-panel',
    };
    if (typeof window !== 'undefined' && window.innerWidth < 576) {
      dialogConfig.position = { top: '0' };
    }
    const dialogRef = this.dialog.open(CheckInDialogComponent, dialogConfig);

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
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '100%';
    dialogConfig.maxWidth = '440px';
    if (window.innerWidth < 576) {
      dialogConfig.position = { top: '0px' };
    }
    dialogConfig.maxHeight = '90vh';
    dialogConfig.data = {}; // No habit = create mode
    dialogConfig.disableClose = false;
    dialogConfig.panelClass = 'habit-form-panel';
    const dialogRef = this.dialog.open(HabitFormComponent, dialogConfig);

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
  getCountArray(n: number): number[] {
    return Array.from({ length: Math.max(0, n) }, (_, i) => i);
  }

  onEditHabit(habit: Habit): void {
    const dialogRef = this.dialog.open(HabitFormComponent, {
      width: '100%',
      maxWidth: '440px',
      maxHeight: '90vh',
      data: { habit }, // Pass habit = edit mode
      disableClose: false,
      panelClass: 'habit-form-panel',
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
