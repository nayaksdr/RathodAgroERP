import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Dealer } from './dealer';

describe('Dealer', () => {
  let component: Dealer;
  let fixture: ComponentFixture<Dealer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dealer]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Dealer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
