import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  Habit,
  CreateHabitDto,
  CheckInDto,
  HabitLog,
  HabitStatistics,
  WeeklyCalendar
} from '../models/habit.model';

@Injectable({
  providedIn: 'root'
})
export class HabitService {
  private apiUrl = `${environment.apiHost}/habits`;

  constructor(private http: HttpClient) {}

  // CRUD
  getHabits(includeInactive: boolean = false): Observable<Habit[]> {
    const params = new HttpParams().set('includeInactive', includeInactive);
    return this.http.get<Habit[]>(this.apiUrl, { params });
  }

  getHabitById(id: number): Observable<Habit> {
    return this.http.get<Habit>(`${this.apiUrl}/${id}`);
  }

  createHabit(dto: CreateHabitDto): Observable<Habit> {
    return this.http.post<Habit>(this.apiUrl, dto);
  }

  updateHabit(id: number, dto: Partial<CreateHabitDto>): Observable<Habit> {
    return this.http.put<Habit>(`${this.apiUrl}/${id}`, dto);
  }

  deleteHabit(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  toggleHabitStatus(id: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/toggle`, {});
  }

  // Check-in
  checkIn(dto: CheckInDto): Observable<HabitLog> {
    return this.http.post<HabitLog>(`${this.apiUrl}/checkin`, dto);
  }

  undoCheckIn(habitId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${habitId}/checkin`);
  }

  // Analytics
  getHabitHistory(
    habitId: number,
    startDate?: string,
    endDate?: string
  ): Observable<HabitLog[]> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    
    return this.http.get<HabitLog[]>(`${this.apiUrl}/${habitId}/history`, { params });
  }

  getHabitStatistics(habitId: number): Observable<HabitStatistics> {
    return this.http.get<HabitStatistics>(`${this.apiUrl}/${habitId}/statistics`);
  }

  getWeeklyCalendar(weekStart?: string): Observable<WeeklyCalendar> {
    let params = new HttpParams();
    if (weekStart) params = params.set('weekStart', weekStart);
    
    return this.http.get<WeeklyCalendar>(`${this.apiUrl}/calendar/weekly`, { params });
  }
}
