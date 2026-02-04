import { TestBed } from '@angular/core/testing';

import { JaggerySaleService } from './jaggery-sale.service';

describe('JaggerySaleService', () => {
  let service: JaggerySaleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JaggerySaleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
