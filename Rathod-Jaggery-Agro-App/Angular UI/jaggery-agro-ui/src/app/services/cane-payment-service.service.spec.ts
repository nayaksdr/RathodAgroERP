import { TestBed } from '@angular/core/testing';

import { CanePaymentServiceService } from './cane-payment-service.service';

describe('CanePaymentServiceService', () => {
  let service: CanePaymentServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CanePaymentServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
