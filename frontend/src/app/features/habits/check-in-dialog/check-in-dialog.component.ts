import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { Habit, CheckInDto } from '../models/habit.model';
import { HabitService } from '../services/habit.service';

@Component({
  selector: 'app-check-in-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  templateUrl: './check-in-dialog.component.html',
  styleUrl: './check-in-dialog.component.scss'
})
export class CheckInDialogComponent {
  checkInForm: FormGroup;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private habitService: HabitService,
    public dialogRef: MatDialogRef<CheckInDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { habit: Habit }
  ) {
    this.checkInForm = this.fb.group({
      notes: ['', [Validators.maxLength(500)]]
    });
  }

  get habit(): Habit {
    return this.data.habit;
  }

  get newStreak(): number {
    return this.habit.currentStreak + 1;
  }

  onSubmit(): void {
    if (this.checkInForm.invalid || this.submitting) return;

    this.submitting = true;

    const checkInDto: CheckInDto = {
      habitId: this.habit.id,
      notes: this.checkInForm.value.notes?.trim() || undefined
    };

    this.habitService.checkIn(checkInDto).subscribe({
      next: (log) => {
        this.dialogRef.close({ success: true, log });
      },
      error: (error) => {
        console.error('Check-in failed:', error);
        this.submitting = false;
        // TODO: Show error message
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close({ success: false });
  }

  getCurrentDate(): string {
    const today = new Date();
    return today.toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  getCurrentTime(): string {
    const now = new Date();
    return now.toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

}
