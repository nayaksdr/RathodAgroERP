import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Splitwise } from './splitwise';

describe('Splitwise', () => {
  let component: Splitwise;
  let fixture: ComponentFixture<Splitwise>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Splitwise]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Splitwise);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
