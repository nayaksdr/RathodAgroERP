import { ComponentFixture, TestBed } from '@angular/core/testing';
import { JaggeryShareDashboardComponent } from './jaggery-share-dashboard';


describe('JaggeryShareDashboard', () => {
  let component: JaggeryShareDashboardComponent;
  let fixture: ComponentFixture<JaggeryShareDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JaggeryShareDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JaggeryShareDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
