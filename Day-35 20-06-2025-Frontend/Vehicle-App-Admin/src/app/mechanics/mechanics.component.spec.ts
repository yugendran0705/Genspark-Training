import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MechanicsComponent } from './mechanics.component';

describe('MechanicsComponent', () => {
  let component: MechanicsComponent;
  let fixture: ComponentFixture<MechanicsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MechanicsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MechanicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
