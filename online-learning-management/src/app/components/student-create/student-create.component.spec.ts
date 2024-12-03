import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { StudentCreateComponent } from './student-create.component';
import { StudentService } from '../../services/student.service';
import { of, throwError } from 'rxjs';
import { ComponentFixture } from '@angular/core/testing';

describe('StudentCreateComponent', () => {
  let component: StudentCreateComponent;
  let fixture: ComponentFixture<StudentCreateComponent>;
  let studentService: jasmine.SpyObj<StudentService>;

  beforeEach(async () => {
    const studentServiceSpy = jasmine.createSpyObj('StudentService', ['createStudent']);

    await TestBed.configureTestingModule({
      declarations: [],
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
      ],
      providers: [
        { provide: StudentService, useValue: studentServiceSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentCreateComponent);
    component = fixture.componentInstance;
    studentService = TestBed.inject(StudentService) as jasmine.SpyObj<StudentService>;

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const form = component.studentForm;
    expect(form).toBeTruthy();
    expect(form.get('name')?.value).toBe('');
    expect(form.get('email')?.value).toBe('');
    expect(form.get('password')?.value).toBe('');
    expect(form.get('status')?.value).toBeTrue();
    expect(form.get('createdAt')?.value).toBeInstanceOf(Date);
    expect(form.get('lastLogin')?.value).toBeNull();
  });

  it('should invalidate the form if required fields are empty', () => {
    component.studentForm.patchValue({
      name: '',
      email: '',
      password: '',
    });
    expect(component.studentForm.valid).toBeFalse();
  });

  it('should validate the email field correctly', () => {
    const emailControl = component.studentForm.get('email');
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTrue();

    emailControl?.setValue('valid.email@example.com');
    expect(emailControl?.valid).toBeTrue();
  });

  /*it('should validate the password field correctly', () => {
    const passwordControl = component.studentForm.get('password');
    passwordControl?.setValue('');
    expect(passwordControl?.hasError('required')).toBeTrue();

    passwordControl?.setValue('short');
    expect(passwordControl?.hasError('minlength')).toBeTrue();

    passwordControl?.setValue('validPassword123');
    expect(passwordControl?.valid).toBeTrue();
  });*/

  it('should validate the lastLogin field correctly', () => {
    component.studentForm.patchValue({
      createdAt: new Date('2023-01-01'),
      lastLogin: new Date('2022-12-31'),
    });
    const lastLoginControl = component.studentForm.get('lastLogin');
    expect(lastLoginControl?.hasError('invalidLastLogin')).toBeTrue();
  });

  it('should call the service to create a student when the form is valid', () => {
    const mockStudent = {
      id: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date(),
      lastLogin: new Date(),
    };

    studentService.createStudent.and.returnValue(of(mockStudent));

    component.studentForm.patchValue(mockStudent);
    component.onSubmit();

    expect(studentService.createStudent).toHaveBeenCalledWith(jasmine.objectContaining({
      name: 'John Doe',
      email: 'john.doe@example.com',
    }));
  });

  it('should handle service errors when creating a student', () => {
    spyOn(window, 'alert');
    studentService.createStudent.and.returnValue(throwError(() => new Error('Error creating student')));

    const mockStudent = {
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date(),
      lastLogin: null,
    };

    component.studentForm.patchValue(mockStudent);
    component.onSubmit();

    expect(studentService.createStudent).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('An error occurred while creating the student. Please try again.');
  });

  it('should show an alert if the form is invalid', () => {
    spyOn(window, 'alert');

    component.studentForm.patchValue({
      name: '',
      email: '',
      password: '',
    });
    component.onSubmit();

    expect(window.alert).toHaveBeenCalledWith('Please fill out the form correctly.');
  });
});
