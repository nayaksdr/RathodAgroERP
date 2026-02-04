import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JaggeryReportComponent } from './jaggery-report.component';

describe('JaggeryReportComponent', () => {
  let component: JaggeryReportComponent;
  let fixture: ComponentFixture<JaggeryReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JaggeryReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JaggeryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
