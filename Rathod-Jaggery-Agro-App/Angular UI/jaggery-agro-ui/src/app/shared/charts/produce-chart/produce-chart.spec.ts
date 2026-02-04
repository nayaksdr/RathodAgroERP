import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProduceChart } from './produce-chart';

describe('ProduceChart', () => {
  let component: ProduceChart;
  let fixture: ComponentFixture<ProduceChart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProduceChart]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProduceChart);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
