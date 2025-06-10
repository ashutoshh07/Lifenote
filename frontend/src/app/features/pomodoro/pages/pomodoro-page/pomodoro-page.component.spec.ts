import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PomodoroPageComponent } from './pomodoro-page.component';

describe('PomodoroPageComponent', () => {
  let component: PomodoroPageComponent;
  let fixture: ComponentFixture<PomodoroPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PomodoroPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PomodoroPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
