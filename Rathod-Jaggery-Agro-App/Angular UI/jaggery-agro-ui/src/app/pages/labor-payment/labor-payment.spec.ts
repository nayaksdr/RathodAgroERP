import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LaborPaymentComponent } from './labor-payment';
describe('LaborPaymentComponent', () => {
  let component: LaborPaymentComponent;
  let fixture: ComponentFixture<LaborPaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LaborPaymentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LaborPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
