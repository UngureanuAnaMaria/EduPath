import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { StudentPredictionsExternComponent } from './student-predictions-extern.component';
import { StudentService } from '../../services/student.service';
import { of, throwError } from 'rxjs';

describe('StudentPredictionsExternComponent', () => {
  let component: StudentPredictionsExternComponent;
  let fixture: ComponentFixture<StudentPredictionsExternComponent>;
  let studentService: StudentService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        StudentPredictionsExternComponent
      ],
      providers: [StudentService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentPredictionsExternComponent);
    component = fixture.componentInstance;
    studentService = TestBed.inject(StudentService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have an invalid form when empty', () => {
    expect(component.studentForm.valid).toBeFalsy();
  });

  it('should validate the form fields', () => {
    const name = component.studentForm.controls['name'];
    name.setValue('');
    expect(name.valid).toBeFalsy();
    name.setValue('John Doe');
    expect(name.valid).toBeTruthy();

    const gender = component.studentForm.controls['gender'];
    gender.setValue('');
    expect(gender.valid).toBeFalsy();
    gender.setValue('Male');
    expect(gender.valid).toBeTruthy();

    const age = component.studentForm.controls['age'];
    age.setValue('');
    expect(age.valid).toBeFalsy();
    age.setValue(25);
    expect(age.valid).toBeTruthy();

    const gpa = component.studentForm.controls['gpa'];
    gpa.setValue('');
    expect(gpa.valid).toBeFalsy();
    gpa.setValue(3.5);
    expect(gpa.valid).toBeTruthy();
  });

  it('should call getStudentPredictionsExtern on submitForm', () => {
    spyOn(studentService, 'getStudentPredictionsExtern').and.returnValue(of({} as any));
    component.studentForm.controls['name'].setValue('John Doe');
    component.studentForm.controls['gender'].setValue('Male');
    component.studentForm.controls['age'].setValue(25);
    component.studentForm.controls['gpa'].setValue(3.5);
    component.studentForm.controls['major'].setValue('Computer Science');
    component.studentForm.controls['interestedDomain'].setValue('AI');
    component.studentForm.controls['projects'].setValue('Project 1');
    component.studentForm.controls['python'].setValue('Strong');
    component.studentForm.controls['sql'].setValue('Strong');
    component.studentForm.controls['java'].setValue('Strong');

    component.submitForm();
    expect(studentService.getStudentPredictionsExtern).toHaveBeenCalled();
  });

  it('should handle error on submitForm', () => {
    spyOn(studentService, 'getStudentPredictionsExtern').and.returnValue(throwError('Error'));
    component.studentForm.controls['name'].setValue('John Doe');
    component.studentForm.controls['gender'].setValue('Male');
    component.studentForm.controls['age'].setValue(25);
    component.studentForm.controls['gpa'].setValue(3.5);
    component.studentForm.controls['major'].setValue('Computer Science');
    component.studentForm.controls['interestedDomain'].setValue('AI');
    component.studentForm.controls['projects'].setValue('Project 1');
    component.studentForm.controls['python'].setValue('Strong');
    component.studentForm.controls['sql'].setValue('Strong');
    component.studentForm.controls['java'].setValue('Strong');

    component.submitForm();
    expect(component.errorMessage).toBe('Error fetching predictions.');
  });

  it('should navigate to /student-predictions-extern on onGetPredictions', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    component.onGetPredictions();
    expect(router.navigate).toHaveBeenCalledWith(['/student-predictions-extern']);
  });

  it('should remove token and navigate to /welcome-page on onLogout', () => {
    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    spyOn(localStorage, 'removeItem');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});