import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentListComponent } from './student-list.component';
import { StudentService } from '../../services/student.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('StudentListComponent', () => {
  let component: StudentListComponent;
  let fixture: ComponentFixture<StudentListComponent>;
  let mockStudentService: any;
  let router: Router;

  beforeEach(async () => {
    mockStudentService = {
      getStudents: jasmine.createSpy('getStudents').and.returnValue(of({
        students: [
          { id: '1', name: 'John Doe', email: 'john@example.com', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [{ id: '1', studentId: '1', courseId: 'course1' }] },
          { id: '2', name: 'Jane Doe', email: 'jane@example.com', status: false, createdAt: new Date(), lastLogin: new Date(), studentCourses: [{ id: '2', studentId: '2', courseId: 'course2' }] }
        ],
        totalCount: 2
      })),
      deleteStudent: jasmine.createSpy('deleteStudent').and.returnValue(of({}))
    };

    await TestBed.configureTestingModule({
      imports: [CommonModule, FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [StudentListComponent],
      providers: [{ provide: StudentService, useValue: mockStudentService }]
    }).compileComponents();

    fixture = TestBed.createComponent(StudentListComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and display students', () => {
    expect(component.students.length).toBe(2);
    expect(mockStudentService.getStudents).toHaveBeenCalled();
  });

  it('should disable the Previous button on the first page', () => {
    component.pageNumber = 1;
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    const prevButton = compiled.querySelector('.pagination button:first-child');
    expect(prevButton.disabled).toBeTrue();
  });

  it('should calculate total pages correctly', () => {
    component.totalCount = 10;
    component.pageSize = 2;
    expect(component.getTotalPages()).toBe(5);
  });

  it('should apply filters correctly', () => {
    component.filters.name = 'John';
    component.applyFilters();
    expect(component.filteredStudents.length).toBe(1);
    expect(component.filteredStudents[0].name).toBe('John Doe');
  });

  it('should navigate to update student page on onUpdate', () => {
    spyOn(router, 'navigate');
    const student = { id: '1', name: 'John Doe', email: 'john@example.com', password: 'password123', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [{ id: '1', studentId: '1', courseId: 'course1' }] };
    component.onUpdate(student);
    expect(router.navigate).toHaveBeenCalledWith(['/student-update/1']);
  });

  it('should navigate to student predictions extern page on onGetPredictions', () => {
    spyOn(router, 'navigate');
    component.onGetPredictions();
    expect(router.navigate).toHaveBeenCalledWith(['/student-predictions-extern']);
  });

  it('should remove token and navigate to welcome page on onLogout', () => {
    spyOn(router, 'navigate');
    spyOn(localStorage, 'removeItem');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});
