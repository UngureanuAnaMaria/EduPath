import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentDetailComponent } from './student-detail.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { StudentService } from '../../services/student.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { StudentDTO } from '../../models/student.model';
import { By } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';

describe('StudentDetailComponent', () => {
  let component: StudentDetailComponent;
  let fixture: ComponentFixture<StudentDetailComponent>;
  let studentServiceSpy: jasmine.SpyObj<StudentService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const studentServiceMock = jasmine.createSpyObj('StudentService', ['getStudentById', 'deleteStudent']);
    const routerMock = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule],
      declarations: [StudentDetailComponent],
      providers: [
        { provide: StudentService, useValue: studentServiceMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: { paramMap: of({ get: () => '1' }) } },
        FormBuilder
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(StudentDetailComponent);
    component = fixture.componentInstance;
    studentServiceSpy = TestBed.inject(StudentService) as jasmine.SpyObj<StudentService>;
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set studentIdControl value from route parameters on init', () => {
    fixture.detectChanges();
    expect(component.studentIdControl.value).toBe('1');
  });

  it('should handle error if student not found', () => {
    studentServiceSpy.getStudentById.and.returnValue(throwError(() => new Error('Student not found')));
    component.searchStudent();

    fixture.detectChanges();
    expect(component.errorMessage).toBe('Error fetching student.');
  });

  it('should validate GUID on invalid studentId input', () => {
    const invalidGuid = 'invalid-guid';
    component.studentIdControl.setValue(invalidGuid);
    component.searchStudent();

    expect(component.studentIdControl.hasError('invalidGuid')).toBeTrue();
  });

  it('should delete student on delete button click', () => {
    const mockStudent: StudentDTO = {
      id: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date(),
      lastLogin: new Date()
    };
    component.student = mockStudent;
  
    studentServiceSpy.deleteStudent.and.returnValue(of(undefined));
    spyOn(window, 'confirm').and.returnValue(true);
  
    component.deleteStudent();
  
    expect(studentServiceSpy.deleteStudent).toHaveBeenCalledWith('1');
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/student-list']);
    expect(component.student).toBeNull();
    expect(component.studentIdControl.value).toBeNull();
    expect(component.errorMessage).toBeNull();
  });
  
  it('should show error if delete student fails', () => {
    const mockStudent: StudentDTO = {
      id: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date(),
      lastLogin: new Date()
    };
    component.student = mockStudent;
  
    studentServiceSpy.deleteStudent.and.returnValue(throwError(() => new Error('Delete failed')));
    spyOn(window, 'confirm').and.returnValue(true);
  
    component.deleteStudent();
  
    expect(studentServiceSpy.deleteStudent).toHaveBeenCalledWith('1');
    expect(component.errorMessage).toBe('Error deleting student.');
  });
  
  it('should call searchStudent on search button click and display student data', () => {
    const mockStudent: StudentDTO = {
      id: '1',
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'password123',
      status: true,
      createdAt: new Date(),
      lastLogin: new Date()
    };
  
    studentServiceSpy.getStudentById.and.returnValue(of(mockStudent));
    component.searchStudent();
  
    fixture.detectChanges();
  
    const nameElement = fixture.debugElement.query(By.css('p')).nativeElement;
    expect(nameElement.textContent).toBe('John Doe');
    expect(studentServiceSpy.getStudentById).toHaveBeenCalledWith('1');
  });
});