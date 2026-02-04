import { TestBed } from '@angular/core/testing';

import { Splitwise } from './splitwise';

describe('Splitwise', () => {
  let service: Splitwise;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Splitwise);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
