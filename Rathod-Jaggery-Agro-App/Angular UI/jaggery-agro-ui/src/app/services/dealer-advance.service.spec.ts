import { TestBed } from '@angular/core/testing';

import { DealerAdvanceService } from './dealer-advance.service';

describe('DealerAdvanceService', () => {
  let service: DealerAdvanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DealerAdvanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
