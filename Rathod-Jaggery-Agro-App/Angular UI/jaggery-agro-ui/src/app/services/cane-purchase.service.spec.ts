import { TestBed } from '@angular/core/testing';

import { CanePurchaseService } from './cane-purchase.service';

describe('CanePurchaseService', () => {
  let service: CanePurchaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CanePurchaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
