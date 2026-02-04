import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JaggerySale } from './jaggery-sale';

describe('JaggerySale', () => {
  let component: JaggerySale;
  let fixture: ComponentFixture<JaggerySale>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JaggerySale]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JaggerySale);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
