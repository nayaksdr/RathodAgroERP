import { TestBed } from '@angular/core/testing';

import { JaggeryReportService } from './jaggery-report.service';

describe('JaggeryReportService', () => {
  let service: JaggeryReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JaggeryReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
