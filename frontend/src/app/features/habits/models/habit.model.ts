export interface Habit {
  id: number;
  name: string;
  description?: string;
  color: string;
  iconName: string;
  frequencyType: 'Daily' | 'Weekly' | 'Custom';
  frequencyValue?: string; // JSON string like '["Monday","Wednesday"]'
  targetCount: number;
  isActive: boolean;
  startDate: string;
  endDate?: string;
  currentStreak: number;
  longestStreak: number;
  totalCompletions: number;
  completedToday: boolean;
  completedCountToday: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateHabitDto {
  name: string;
  description?: string;
  color: string;
  iconName: string;
  frequencyType: 'Daily' | 'Weekly' | 'Custom';
  frequencyValue?: string;
  targetCount: number;
  startDate?: string;
  endDate?: string;
}

export interface CheckInDto {
  habitId: number;
  notes?: string;
}

export interface HabitLog {
  id: number;
  habitId: number;
  habitName: string;
  completedAt: string;
  completedDate: string;
  notes?: string;
  currentStreak: number;
}

export interface HabitStatistics {
  habitId: number;
  habitName: string;
  currentStreak: number;
  longestStreak: number;
  totalCompletions: number;
  completionRate: number;
  bestDayOfWeek?: string;
  worstDayOfWeek?: string;
  last7Days: DailyActivity[];
}

export interface DailyActivity {
  date: string;
  completed: boolean;
  notes?: string;
}

export interface WeeklyCalendar {
  weekStart: string;
  weekEnd: string;
  totalHabits: number;
  overallCompletionRate: number;
  habits: HabitWeek[];
}

export interface HabitWeek {
  habitId: number;
  name: string;
  color: string;
  iconName: string;
  frequencyType: string;
  completedDates: string[];
  completedCount: number;
  expectedCount: number;
  completionRate: number;
}
