import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaneAdvance } from './cane-advance';

describe('CaneAdvance', () => {
  let component: CaneAdvance;
  let fixture: ComponentFixture<CaneAdvance>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaneAdvance]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaneAdvance);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
