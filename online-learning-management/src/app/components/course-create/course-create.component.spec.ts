import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CourseCreateComponent } from './course-create.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CourseService } from '../../services/course.service';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';

describe('CourseCreateComponent', () => {
  let component: CourseCreateComponent;
  let fixture: ComponentFixture<CourseCreateComponent>;
  let courseService: jasmine.SpyObj<CourseService>;
  let router: Router;

  beforeEach(async () => {
    const courseServiceSpy = jasmine.createSpyObj('CourseService', ['createCourse']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule],
      declarations: [CourseCreateComponent],
      providers: [
        { provide: CourseService, useValue: courseServiceSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CourseCreateComponent);
    component = fixture.componentInstance;
    courseService = TestBed.inject(CourseService) as jasmine.SpyObj<CourseService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    const form = component.courseForm;
    expect(form).toBeTruthy();
    expect(form.get('name')?.value).toBe('');
    expect(form.get('description')?.value).toBe('');
  });

  it('should invalidate the form if required fields are empty', () => {
    component.courseForm.patchValue({
      name: '',
      description: '',
    });
    expect(component.courseForm.valid).toBeFalse();
  });

  it('should validate the name field correctly', () => {
    const nameControl = component.courseForm.get('name');
    nameControl?.setValue('');
    expect(nameControl?.hasError('required')).toBeTrue();

    nameControl?.setValue('A'.repeat(201));
    expect(nameControl?.hasError('maxlength')).toBeTrue();

    nameControl?.setValue('Valid Course Name');
    expect(nameControl?.valid).toBeTrue();
  });

  it('should validate the description field correctly', () => {
    const descriptionControl = component.courseForm.get('description');
    descriptionControl?.setValue('');
    expect(descriptionControl?.hasError('required')).toBeTrue();

    descriptionControl?.setValue('A'.repeat(1001));
    expect(descriptionControl?.hasError('maxlength')).toBeTrue();

    descriptionControl?.setValue('Valid Course Description');
    expect(descriptionControl?.valid).toBeTrue();
  });

  it('should call the service to create a course when the form is valid', () => {
    const mockCourse = {
      id: '1',
      name: 'Course 1',
      description: 'Description 1',
      professorId: '1'
    };

    courseService.createCourse.and.returnValue(of(mockCourse));

    component.courseForm.patchValue({
      name: 'Course 1',
      description: 'Description 1',
    });
    component.onSubmit();

    expect(courseService.createCourse).toHaveBeenCalledWith(jasmine.objectContaining({
      name: 'Course 1',
      description: 'Description 1',
    }));
  });

  it('should handle service errors when creating a course', () => {
    spyOn(window, 'alert');
    courseService.createCourse.and.returnValue(throwError(() => new Error('Error creating course')));

    component.courseForm.patchValue({
      name: 'Course 1',
      description: 'Description 1',
    });
    component.onSubmit();

    expect(courseService.createCourse).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('An error occurred while creating the course. Please try again.');
  });

  it('should show an alert if the form is invalid', () => {
    spyOn(window, 'alert');

    component.courseForm.patchValue({
      name: '',
      description: '',
    });
    component.onSubmit();

    expect(window.alert).toHaveBeenCalledWith('Please fill out the form correctly.');
  });

  it('should navigate to course list page on successful course creation', () => {
    const mockCourse = {
      id: '1',
      name: 'Course 1',
      description: 'Description 1',
      professorId: '1'
    };

    courseService.createCourse.and.returnValue(of(mockCourse));
    spyOn(router, 'navigate');

    component.courseForm.patchValue({
      name: 'Course 1',
      description: 'Description 1',
    });
    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/course-list']);
  });

  it('should remove token and navigate to welcome page on onLogout', () => {
    spyOn(localStorage, 'removeItem');
    spyOn(router, 'navigate');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});