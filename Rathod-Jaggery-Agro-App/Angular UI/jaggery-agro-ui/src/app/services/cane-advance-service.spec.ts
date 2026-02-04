import { TestBed } from '@angular/core/testing';

import { CaneAdvanceService } from './cane-advance-service';

describe('CaneAdvanceService', () => {
  let service: CaneAdvanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CaneAdvanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
