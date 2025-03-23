import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UserService } from '../../services/user.service';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let userService: jasmine.SpyObj<UserService>;
  let router: Router;

  beforeEach(async () => {
    const userServiceSpy = jasmine.createSpyObj('UserService', {
      register: of({})
    });

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule],
      declarations: [RegisterComponent],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const form = component.registerForm;
    expect(form).toBeTruthy();
    expect(form.get('email')?.value).toBe('');
    expect(form.get('password')?.value).toBe('');
    expect(form.get('confirmPassword')?.value).toBe('');
  });

  it('should invalidate the form if required fields are empty', () => {
    component.registerForm.patchValue({
      email: '',
      password: '',
      confirmPassword: '',
    });
    expect(component.registerForm.valid).toBeFalse();
  });

  it('should validate the email field correctly', () => {
    const emailControl = component.registerForm.get('email');
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTrue();

    emailControl?.setValue('valid.email@example.com');
    expect(emailControl?.valid).toBeTrue();
  });

  it('should validate the password field correctly', () => {
    const passwordControl = component.registerForm.get('password');
    passwordControl?.setValue('');
    expect(passwordControl?.hasError('required')).toBeTrue();

    passwordControl?.setValue('short');
    expect(passwordControl?.hasError('minlength')).toBeTrue();

    passwordControl?.setValue('validPassword123');
    expect(passwordControl?.valid).toBeTrue();
  });

  it('should validate the confirmPassword field correctly', () => {
    component.registerForm.patchValue({
      password: 'validPassword123',
      confirmPassword: 'differentPassword',
    });
    const confirmPasswordControl = component.registerForm.get('confirmPassword');
    expect(component.passwordsMatch()).toBeFalse();

    component.registerForm.patchValue({
      confirmPassword: 'validPassword123',
    });
    expect(component.passwordsMatch()).toBeTrue();
  });

  it('should call the service to register a user when the form is valid', () => {
    const mockUser = {
      id: '1',
      email: 'john.doe@example.com',
      password: 'password123',
      admin: false,
      message: 'User registered successfully',
    };

    userService.register.and.returnValue(of(mockUser));

    component.registerForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
    });
    component.onRegister();

    expect(userService.register).toHaveBeenCalledWith(jasmine.objectContaining({
      email: 'john.doe@example.com',
      password: 'password123',
    }));
  });

  it('should handle service errors when registering a user', () => {
    spyOn(window, 'alert');
    userService.register.and.returnValue(throwError(() => new Error('Error registering user')));

    component.registerForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
    });
    component.onRegister();

    expect(userService.register).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Registration failed. Please try again.');
  });

  it('should show an alert if the form is invalid', () => {
    spyOn(window, 'alert');

    component.registerForm.patchValue({
      email: '',
      password: '',
      confirmPassword: '',
    });
    component.onRegister();

    expect(window.alert).toHaveBeenCalledWith('Please fill out the form correctly.');
  });

  it('should navigate to login page on successful registration', () => {
    const mockUser = {
      id: '1',
      email: 'john.doe@example.com',
      password: 'password123',
      admin: false,
      message: 'User registered successfully',
    };

    userService.register.and.returnValue(of(mockUser));
    spyOn(router, 'navigate');

    component.registerForm.patchValue({
      email: 'john.doe@example.com',
      password: 'password123',
      confirmPassword: 'password123',
    });
    component.onRegister();

    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});