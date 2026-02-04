import { TestBed } from '@angular/core/testing';

import { LaborTypeRateService } from './labor-type-rate.service';

describe('LaborTypeRateService', () => {
  let service: LaborTypeRateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LaborTypeRateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
