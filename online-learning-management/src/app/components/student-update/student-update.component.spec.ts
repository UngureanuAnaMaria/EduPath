import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentUpdateComponent } from './student-update.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StudentService } from '../../services/student.service';
import { Router, ActivatedRoute, convertToParamMap } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { fakeAsync, tick } from '@angular/core/testing';

describe('StudentUpdateComponent', () => {
  let component: StudentUpdateComponent;
  let fixture: ComponentFixture<StudentUpdateComponent>;
  let studentService: jasmine.SpyObj<StudentService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const studentServiceSpy = jasmine.createSpyObj('StudentService', ['getStudentById', 'updateStudent']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    
    await TestBed.configureTestingModule({
      imports: [ 
        ReactiveFormsModule, 
        HttpClientTestingModule, 
        StudentUpdateComponent // Importing the standalone component here
      ],
      providers: [
        FormBuilder,
        { provide: StudentService, useValue: studentServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of(convertToParamMap({ id: '123' }))
          }
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentUpdateComponent);
    component = fixture.componentInstance;
    studentService = TestBed.inject(StudentService) as jasmine.SpyObj<StudentService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form correctly', () => {
    expect(component.studentForm).toBeDefined();
    expect(component.studentIdControl).toBeDefined();
  });

  /*it('should call searchStudent() and populate the form when student is found', () => {
    const mockStudent = {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date('2023-12-01T00:00:00'),
      lastLogin: new Date('2023-12-02T00:00:00')
    };

    studentService.getStudentById.and.returnValue(of(mockStudent));
    component.searchStudent();

    expect(studentService.getStudentById).toHaveBeenCalledOnceWith('123');
    expect(component.studentForm.value).toEqual({
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: '2023-12-01',
      lastLogin: '2023-12-02'
    });
  });*/

  /*it('should handle error if student is not found', () => {
    studentService.getStudentById.and.returnValue(throwError(() => new Error('Error fetching student')));
  
    component.searchStudent();
  
    expect(component.errorMessage).toBe('Error fetching student.');
  });*/
  

  /*it('should call updateStudent() and navigate on success', fakeAsync(() => {
    const formValue = {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: '2023-12-01',
      lastLogin: '2023-12-02'
    };
  
    component.studentForm.setValue(formValue);
  
    const formValueWithDates = {
      ...formValue,
      createdAt: new Date(formValue.createdAt),
      lastLogin: new Date(formValue.lastLogin)
    };
  
    studentService.updateStudent.and.returnValue(of(formValueWithDates));
  
    // Call updateStudent() manually
    component.updateStudent();
  
    tick(); // Simulate the passage of time for async operations
  
    expect(studentService.updateStudent).toHaveBeenCalledOnceWith({
      ...formValue,
      createdAt: new Date(formValue.createdAt),
      lastLogin: new Date(formValue.lastLogin)
    });
    expect(router.navigate).toHaveBeenCalledOnceWith(['/student-list']);
    expect(component.errorMessage).toBeNull();
  }));*/

  it('should handle error when updating student', () => {
    const formValue = {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: '2023-12-01',
      lastLogin: '2023-12-02'
    };
    component.studentForm.setValue(formValue);

    studentService.updateStudent.and.returnValue(throwError(() => new Error('An error occurred')));
    component.updateStudent();

    expect(component.errorMessage).toBe(null);
  });
});
