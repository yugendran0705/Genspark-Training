import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Videolist } from './videolist';

describe('Videolist', () => {
  let component: Videolist;
  let fixture: ComponentFixture<Videolist>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Videolist]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Videolist);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
