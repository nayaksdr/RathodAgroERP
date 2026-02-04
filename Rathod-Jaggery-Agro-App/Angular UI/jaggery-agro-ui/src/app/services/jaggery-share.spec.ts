import { TestBed } from '@angular/core/testing';

import { JaggeryShare } from './jaggery-share';

describe('JaggeryShare', () => {
  let service: JaggeryShare;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JaggeryShare);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
