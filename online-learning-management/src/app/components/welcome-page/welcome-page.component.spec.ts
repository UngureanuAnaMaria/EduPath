import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { WelcomePageComponent } from './welcome-page.component';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';

describe('WelcomePageComponent', () => {
  let component: WelcomePageComponent;
  let fixture: ComponentFixture<WelcomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, WelcomePageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WelcomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the navbar', () => {
    const navbar = fixture.debugElement.query(By.css('.navbar'));
    expect(navbar).toBeTruthy();
  });

  it('should render the welcome section', () => {
    const welcomeSection = fixture.debugElement.query(By.css('.hero-section'));
    expect(welcomeSection).toBeTruthy();
  });

  it('should render the features section', () => {
    const featuresSection = fixture.debugElement.query(By.css('.features-section'));
    expect(featuresSection).toBeTruthy();
  });

  it('should render the footer', () => {
    const footer = fixture.debugElement.query(By.css('.footer'));
    expect(footer).toBeTruthy();
  });

  it('should navigate to /select-role when Get Started button is clicked', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');

    const getStartedButton = fixture.debugElement.query(By.css('.btn-success'));
    getStartedButton.nativeElement.click();

    expect(router.navigate).toHaveBeenCalledWith(['/select-role']);
  });

  it('should navigate to /login when Login button is clicked', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');

    const loginButton = fixture.debugElement.query(By.css('.btn-primary'));
    loginButton.nativeElement.click();

    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});