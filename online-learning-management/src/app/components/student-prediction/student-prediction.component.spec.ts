import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { StudentPredictionComponent } from './student-prediction.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StudentService } from '../../services/student.service';
import { Router, ActivatedRoute, convertToParamMap } from '@angular/router';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('StudentPredictionComponent', () => {
  let component: StudentPredictionComponent;
  let fixture: ComponentFixture<StudentPredictionComponent>;
  let studentService: jasmine.SpyObj<StudentService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const studentServiceSpy = jasmine.createSpyObj('StudentService', ['getStudentById', 'getStudentPredictions']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    
    await TestBed.configureTestingModule({
      imports: [ 
        ReactiveFormsModule, 
        HttpClientTestingModule, 
        StudentPredictionComponent
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

    fixture = TestBed.createComponent(StudentPredictionComponent);
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

  it('should call searchStudent() and get predictions when student is found', fakeAsync(() => {
    const mockStudent = {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date('2023-12-01T00:00:00'),
      lastLogin: new Date('2023-12-02T00:00:00')
    };

    const mockPredictions = {
      averageGrade: 90,
      percentageCompletedCourses: 80,
      learningPath: 'Data Science'
    };

    studentService.getStudentById.and.returnValue(of(mockStudent));
    studentService.getStudentPredictions.and.returnValue(of(mockPredictions));

    component.searchStudent();
    tick();

    expect(studentService.getStudentById).toHaveBeenCalledOnceWith('123');
    expect(component.student).toEqual(mockStudent);
    expect(studentService.getStudentPredictions).toHaveBeenCalledOnceWith('123');
    expect(component.predictions).toEqual(mockPredictions);
  }));

  it('should handle error if student is not found', fakeAsync(() => {
    studentService.getStudentById.and.returnValue(throwError(() => new Error('Error fetching student')));
  
    component.searchStudent();
    tick();
  
    expect(component.errorMessage).toBe('Error fetching student.');
    expect(component.student).toBeNull();
  }));

  it('should handle error when fetching predictions', fakeAsync(() => {
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
    studentService.getStudentPredictions.and.returnValue(throwError(() => new Error('Error fetching predictions')));

    component.searchStudent();
    tick();

    expect(studentService.getStudentById).toHaveBeenCalledOnceWith('123');
    expect(component.student).toEqual(mockStudent);
    expect(studentService.getStudentPredictions).toHaveBeenCalledOnceWith('123');
    expect(component.errorMessage).toBe('Error fetching predictions.');
    expect(component.predictions).toBeNull();
  }));

  it('should navigate to /student-predictions-extern on onGetPredictions', () => {
    component.onGetPredictions();
    expect(router.navigate).toHaveBeenCalledWith(['/student-predictions-extern']);
  });

  it('should remove token and navigate to /welcome-page on onLogout', () => {
    spyOn(localStorage, 'removeItem');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});