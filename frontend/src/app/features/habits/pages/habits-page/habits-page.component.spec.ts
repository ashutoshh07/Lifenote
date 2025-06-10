import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HabitsPageComponent } from './habits-page.component';

describe('HabitsPageComponent', () => {
  let component: HabitsPageComponent;
  let fixture: ComponentFixture<HabitsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HabitsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HabitsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
