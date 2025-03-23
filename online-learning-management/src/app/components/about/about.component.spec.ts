import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AboutComponent } from './about.component';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('AboutComponent', () => {
  let component: AboutComponent;
  let fixture: ComponentFixture<AboutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AboutComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AboutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the navbar', () => {
    const navbar = fixture.debugElement.query(By.css('.navbar'));
    expect(navbar).toBeTruthy();
  });

  it('should display the hero section', () => {
    const heroSection = fixture.debugElement.query(By.css('.about-hero'));
    expect(heroSection).toBeTruthy();
  });

  it('should display the about content section', () => {
    const aboutContentSection = fixture.debugElement.query(By.css('.about-content'));
    expect(aboutContentSection).toBeTruthy();
  });

  it('should display the mission section', () => {
    const missionSection = fixture.debugElement.query(By.css('.about-mission'));
    expect(missionSection).toBeTruthy();
  });

  it('should display the footer', () => {
    const footer = fixture.debugElement.query(By.css('footer'));
    expect(footer).toBeTruthy();
  });

  it('should display the correct title in the hero section', () => {
    const heroTitle = fixture.debugElement.query(By.css('.about-hero h1')).nativeElement;
    expect(heroTitle.textContent).toContain('About EduPath');
  });

  it('should display the correct mission statement', () => {
    const missionStatement = fixture.debugElement.query(By.css('.about-mission p')).nativeElement;
    expect(missionStatement.textContent).toContain('To empower individuals through accessible, high-quality education that inspires a lifelong love of learning.');
  });
});