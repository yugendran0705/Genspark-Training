import { TestBed } from '@angular/core/testing';

import { EventpageService } from './eventpage-service';

describe('EventpageService', () => {
  let service: EventpageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EventpageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
