import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaborTypeRate } from './labor-type-rate';

describe('LaborTypeRate', () => {
  let component: LaborTypeRate;
  let fixture: ComponentFixture<LaborTypeRate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaborTypeRate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LaborTypeRate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
