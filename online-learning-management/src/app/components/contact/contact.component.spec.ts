import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContactComponent } from './contact.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';

describe('ContactComponent', () => {
  let component: ContactComponent;
  let fixture: ComponentFixture<ContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule],
      declarations: [ContactComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ContactComponent);
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

  it('should display the contact form', () => {
    const form = fixture.debugElement.query(By.css('form'));
    expect(form).toBeTruthy();
  });

  it('should display the contact details', () => {
    const contactDetails = fixture.debugElement.query(By.css('.col-md-6'));
    expect(contactDetails).toBeTruthy();
  });

  it('should validate the name field correctly', () => {
    const nameInput = fixture.debugElement.query(By.css('#name')).nativeElement;
    nameInput.value = '';
    nameInput.dispatchEvent(new Event('input'));
    fixture.detectChanges();

    const nameErrors = fixture.debugElement.query(By.css('#name + div small'));
    expect(nameErrors).toBeTruthy();
  });

  it('should validate the email field correctly', () => {
    const emailInput = fixture.debugElement.query(By.css('#email')).nativeElement;
    emailInput.value = 'invalid-email';
    emailInput.dispatchEvent(new Event('input'));
    fixture.detectChanges();

    const emailErrors = fixture.debugElement.query(By.css('#email + div small'));
    expect(emailErrors).toBeTruthy();
  });

  it('should validate the message field correctly', () => {
    const messageInput = fixture.debugElement.query(By.css('#message')).nativeElement;
    messageInput.value = '';
    messageInput.dispatchEvent(new Event('input'));
    fixture.detectChanges();

    const messageErrors = fixture.debugElement.query(By.css('#message + div small'));
    expect(messageErrors).toBeTruthy();
  });

  it('should display the footer', () => {
    const footer = fixture.debugElement.query(By.css('footer'));
    expect(footer).toBeTruthy();
  });
});