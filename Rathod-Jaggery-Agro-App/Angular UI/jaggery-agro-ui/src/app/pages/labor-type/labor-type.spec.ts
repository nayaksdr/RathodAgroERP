import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LaborType } from './labor-type';

describe('LaborType', () => {
  let component: LaborType;
  let fixture: ComponentFixture<LaborType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaborType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LaborType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
