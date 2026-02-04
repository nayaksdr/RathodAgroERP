import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JaggerySharePayment } from './jaggery-share-payment';

describe('JaggerySharePayment', () => {
  let component: JaggerySharePayment;
  let fixture: ComponentFixture<JaggerySharePayment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JaggerySharePayment]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JaggerySharePayment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
