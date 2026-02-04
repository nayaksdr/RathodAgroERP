import { TestBed } from '@angular/core/testing';

import { Produce } from './produce';

describe('Produce', () => {
  let service: Produce;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Produce);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
