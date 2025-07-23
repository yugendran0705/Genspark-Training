import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignalrEventComponent } from './signalr-event.component';

describe('SignalrEventComponent', () => {
  let component: SignalrEventComponent;
  let fixture: ComponentFixture<SignalrEventComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignalrEventComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignalrEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
