import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseType } from './expense-type';

describe('ExpenseType', () => {
  let component: ExpenseType;
  let fixture: ComponentFixture<ExpenseType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpenseType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpenseType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
