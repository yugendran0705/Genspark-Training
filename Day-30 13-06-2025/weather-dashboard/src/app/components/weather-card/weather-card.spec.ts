import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherCardComponent } from './weather-card';

describe('WeatherCardComponent', () => {
  let component: WeatherCardComponent;
  let fixture: ComponentFixture<WeatherCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WeatherCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WeatherCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
