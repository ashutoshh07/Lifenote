import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HabitCalendarComponent } from './habit-calendar.component';

describe('HabitCalendarComponent', () => {
  let component: HabitCalendarComponent;
  let fixture: ComponentFixture<HabitCalendarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HabitCalendarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HabitCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
