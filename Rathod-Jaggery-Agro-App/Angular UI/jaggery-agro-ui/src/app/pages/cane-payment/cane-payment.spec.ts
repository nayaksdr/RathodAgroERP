import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CanePaymentComponent } from './cane-payment';

describe('CanePaymentComponent ', () => {
  let component: CanePaymentComponent ;
  let fixture: ComponentFixture<CanePaymentComponent >;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CanePaymentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CanePaymentComponent );
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
