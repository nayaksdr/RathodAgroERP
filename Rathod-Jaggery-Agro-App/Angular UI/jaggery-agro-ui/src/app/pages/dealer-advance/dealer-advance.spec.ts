import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerAdvance } from './dealer-advance';

describe('DealerAdvance', () => {
  let component: DealerAdvance;
  let fixture: ComponentFixture<DealerAdvance>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DealerAdvance]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DealerAdvance);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
