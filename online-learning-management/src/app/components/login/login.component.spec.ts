import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UserService } from '../../services/user.service';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let router: Router;

  beforeEach(async () => {
    const userServiceSpy = jasmine.createSpyObj('UserService', ['login']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule],
      declarations: [LoginComponent],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const form = component.loginForm;
    expect(form).toBeTruthy();
    expect(form.get('email')?.value).toBe('');
    expect(form.get('password')?.value).toBe('');
  });

  it('should invalidate the form if required fields are empty', () => {
    component.loginForm.patchValue({
      email: '',
      password: '',
    });
    expect(component.loginForm.valid).toBeFalse();
  });

  it('should validate the email field correctly', () => {
    const emailControl = component.loginForm.get('email');
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTrue();

    emailControl?.setValue('valid.email@example.com');
    expect(emailControl?.valid).toBeTrue();
  });

  it('should validate the password field correctly', () => {
    const passwordControl = component.loginForm.get('password');
    passwordControl?.setValue('');
    expect(passwordControl?.hasError('required')).toBeTrue();

    passwordControl?.setValue('short');
    expect(passwordControl?.hasError('minlength')).toBeTrue();

    passwordControl?.setValue('validPassword123');
    expect(passwordControl?.valid).toBeTrue();
  });

  it('should call the service to login a user when the form is valid', () => {
    const mockResponse = {

      token: 'dummy-token',

      admin: false,

    };

    userService.login.and.returnValue(of(mockResponse));

    component.loginForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
    });
    component.onLogin();

    expect(userService.login).toHaveBeenCalledWith('john.doe@example.com', 'password123', '');
  });

  it('should handle service errors when logging in a user', () => {
    spyOn(window, 'alert');
    userService.login.and.returnValue(throwError(() => new Error('Error logging in')));

    component.loginForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
    });
    component.onLogin();

    expect(userService.login).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Login failed. Please check your credentials and try again.');
  });

  it('should show an alert if the form is invalid', () => {
    spyOn(window, 'alert');

    component.loginForm.patchValue({
      email: '',
      password: '',
    });
    component.onLogin();

    expect(window.alert).toHaveBeenCalledWith('Please fill out the form correctly.');
  });

  it('should navigate to student list page on successful login for non-admin user', () => {
    const mockResponse = {

      token: 'dummy-token',

      admin: false,

    };

    userService.login.and.returnValue(of(mockResponse));
    spyOn(router, 'navigate');

    component.loginForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
    });
    component.onLogin();

    expect(router.navigate).toHaveBeenCalledWith(['/student-list']);
  });
});