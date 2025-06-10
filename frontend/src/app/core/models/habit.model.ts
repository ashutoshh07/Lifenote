import { BaseEntity } from "./base-entity.model";

export enum HabitFrequency {
  DAILY = 'daily',
  WEEKLY = 'weekly',
  MONTHLY = 'monthly'
}

export enum HabitCategory {
  HEALTH = 'health',
  LEARNING = 'learning',
  PRODUCTIVITY = 'productivity',
  PERSONAL = 'personal',
  SOCIAL = 'social'
}

export interface Habit extends BaseEntity {
  name: string;
  description?: string;
  category: HabitCategory;
  frequency: HabitFrequency;
  targetCount: number; // How many times per frequency period
  icon: string; // Icon name/emoji
  color: string; // Hex color for theming
}

// Track individual habit completions
export interface HabitEntry extends BaseEntity {
  habitId: string;
  date: string; // YYYY-MM-DD format
  completed: boolean;
  count: number; // For habits that can be done multiple times
  notes?: string;
}

// Calculated statistics
export interface HabitStreak {
  habitId: string;
  currentStreak: number;
  longestStreak: number;
  completionRate: number; // percentage
}