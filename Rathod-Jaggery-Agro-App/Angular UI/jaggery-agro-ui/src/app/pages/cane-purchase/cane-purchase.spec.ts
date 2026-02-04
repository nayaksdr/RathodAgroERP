import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CanePurchase } from './cane-purchase';

describe('CanePurchase', () => {
  let component: CanePurchase;
  let fixture: ComponentFixture<CanePurchase>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CanePurchase]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CanePurchase);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
