import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerBalance } from './dealer-balance';

describe('DealerBalance', () => {
  let component: DealerBalance;
  let fixture: ComponentFixture<DealerBalance>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DealerBalance]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DealerBalance);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
