import { TestBed } from '@angular/core/testing';

import { LaborPayment } from './labor-payment';

describe('LaborPayment', () => {
  let service: LaborPayment;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LaborPayment);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
