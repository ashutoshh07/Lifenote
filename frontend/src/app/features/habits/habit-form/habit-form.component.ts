import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatChipsModule } from '@angular/material/chips';
import { Habit, CreateHabitDto } from '../models/habit.model';
import { HabitService } from '../services/habit.service';

export interface HabitFormDialogData {
  habit?: Habit; // If provided, we're editing. Otherwise, creating.
}

@Component({
  selector: 'app-habit-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatRadioModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule
  ],
  templateUrl: './habit-form.component.html',
  styleUrl: './habit-form.component.scss'
})
export class HabitFormComponent implements OnInit {
  habitForm: FormGroup;
  submitting = false;
  isEditMode = false;

  // Icon options
  availableIcons = [
    { name: '💪', label: 'Strength' },
    { name: '🏃', label: 'Running' },
    { name: '🧘', label: 'Yoga' },
    { name: '📚', label: 'Reading' },
    { name: '💻', label: 'Coding' },
    { name: '🎨', label: 'Art' },
    { name: '🎵', label: 'Music' },
    { name: '🍎', label: 'Healthy' },
    { name: '💧', label: 'Water' },
    { name: '😴', label: 'Sleep' },
    { name: '🚴', label: 'Cycling' },
    { name: '🏋️', label: 'Gym' },
    { name: '⚽', label: 'Sports' },
    { name: '🎯', label: 'Goal' },
    { name: '✅', label: 'Task' },
    { name: '🔥', label: 'Fire' },
    { name: '⭐', label: 'Star' },
    { name: '🌟', label: 'Sparkle' },
    { name: '💡', label: 'Idea' },
    { name: '📝', label: 'Note' }
  ];

  // Color options
  availableColors = [
    { value: '#4CAF50', label: 'Green' },
    { value: '#2196F3', label: 'Blue' },
    { value: '#F44336', label: 'Red' },
    { value: '#FFC107', label: 'Yellow' },
    { value: '#9C27B0', label: 'Purple' },
    { value: '#FF9800', label: 'Orange' },
    { value: '#607D8B', label: 'Gray' },
    { value: '#E91E63', label: 'Pink' }
  ];

  // Days of week for custom frequency
  daysOfWeek = [
    { value: 'Monday', label: 'Mon', selected: false },
    { value: 'Tuesday', label: 'Tue', selected: false },
    { value: 'Wednesday', label: 'Wed', selected: false },
    { value: 'Thursday', label: 'Thu', selected: false },
    { value: 'Friday', label: 'Fri', selected: false },
    { value: 'Saturday', label: 'Sat', selected: false },
    { value: 'Sunday', label: 'Sun', selected: false }
  ];

  constructor(
    private fb: FormBuilder,
    private habitService: HabitService,
    public dialogRef: MatDialogRef<HabitFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: HabitFormDialogData
  ) {
    this.isEditMode = !!data.habit;

    this.habitForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(500)]],
      iconName: ['💪', Validators.required],
      color: ['#4CAF50', Validators.required],
      frequencyType: ['Daily', Validators.required],
      targetCount: [1, [Validators.required, Validators.min(1), Validators.max(10)]],
      startDate: [new Date()],
      endDate: [null]
    });
  }

  ngOnInit(): void {
    if (this.isEditMode && this.data.habit) {
      this.populateForm(this.data.habit);
    }
  }

  populateForm(habit: Habit): void {
    this.habitForm.patchValue({
      name: habit.name,
      description: habit.description,
      iconName: habit.iconName,
      color: habit.color,
      frequencyType: habit.frequencyType,
      targetCount: habit.targetCount,
      startDate: new Date(habit.startDate),
      endDate: habit.endDate ? new Date(habit.endDate) : null
    });

    // Parse custom days if frequencyType is Custom
    if (habit.frequencyType === 'Custom' && habit.frequencyValue) {
      try {
        const selectedDays = JSON.parse(habit.frequencyValue);
        this.daysOfWeek.forEach(day => {
          day.selected = selectedDays.includes(day.value);
        });
      } catch (e) {
        console.error('Error parsing frequency value:', e);
      }
    }
  }

  get frequencyType(): string {
    return this.habitForm.get('frequencyType')?.value;
  }

  get isCustomFrequency(): boolean {
    return this.frequencyType === 'Custom';
  }

  selectIcon(icon: string): void {
    this.habitForm.patchValue({ iconName: icon });
  }

  selectColor(color: string): void {
    this.habitForm.patchValue({ color });
  }

  toggleDay(day: any): void {
    day.selected = !day.selected;
  }

  incrementTarget(): void {
    const current = this.habitForm.get('targetCount')?.value || 1;
    if (current < 10) {
      this.habitForm.patchValue({ targetCount: current + 1 });
    }
  }

  decrementTarget(): void {
    const current = this.habitForm.get('targetCount')?.value || 1;
    if (current > 1) {
      this.habitForm.patchValue({ targetCount: current - 1 });
    }
  }

  getSelectedDaysJson(): string | undefined {
    const selected = this.daysOfWeek
      .filter(day => day.selected)
      .map(day => day.value);
    
    return selected.length > 0 ? JSON.stringify(selected) : undefined;
  }

  onSubmit(): void {
    if (this.habitForm.invalid || this.submitting) return;

    // Validate custom frequency
    if (this.isCustomFrequency) {
      const selectedDays = this.daysOfWeek.filter(d => d.selected);
      if (selectedDays.length === 0) {
        alert('Please select at least one day for custom frequency');
        return;
      }
    }

    this.submitting = true;

    const formValue = this.habitForm.value;
    const dto: CreateHabitDto = {
      name: formValue.name.trim(),
      description: formValue.description?.trim() || undefined,
      color: formValue.color,
      iconName: formValue.iconName,
      frequencyType: formValue.frequencyType,
      frequencyValue: this.isCustomFrequency ? this.getSelectedDaysJson() : undefined,
      targetCount: formValue.targetCount,
      startDate: formValue.startDate?.toISOString(),
      endDate: formValue.endDate?.toISOString() || undefined
    };

    const request = this.isEditMode && this.data.habit
      ? this.habitService.updateHabit(this.data.habit.id, dto)
      : this.habitService.createHabit(dto);

    request.subscribe({
      next: (habit) => {
        this.dialogRef.close({ success: true, habit });
      },
      error: (error) => {
        console.error('Save failed:', error);
        this.submitting = false;
        // TODO: Show error message
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close({ success: false });
  }
}
