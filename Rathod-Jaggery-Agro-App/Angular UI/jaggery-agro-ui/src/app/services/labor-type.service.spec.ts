import { TestBed } from '@angular/core/testing';

import { LaborTypeService } from './labor-type.service';

describe('LaborTypeService', () => {
  let service: LaborTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LaborTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
