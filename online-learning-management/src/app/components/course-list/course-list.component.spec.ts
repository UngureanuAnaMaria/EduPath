import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CourseListComponent } from './course-list.component';
import { CourseService } from '../../services/course.service';
import { of, throwError } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

describe('CourseListComponent', () => {
  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;
  let mockCourseService: jasmine.SpyObj<CourseService>;
  let router: Router;

  beforeEach(async () => {
    mockCourseService = jasmine.createSpyObj('CourseService', ['getCourses', 'deleteCourse']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, FormsModule, RouterTestingModule],
      declarations: [CourseListComponent],
      providers: [{ provide: CourseService, useValue: mockCourseService }]
    }).compileComponents();

    fixture = TestBed.createComponent(CourseListComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and display courses', () => {
    const mockCourses = {
      courses: [
        { id: '1', name: 'Course 1', description: 'Description 1', professorId: 'prof1' },
        { id: '2', name: 'Course 2', description: 'Description 2', professorId: 'prof2' }
      ],
      totalCount: 2
    };

    mockCourseService.getCourses.and.returnValue(of(mockCourses));
    component.loadCourses();
    fixture.detectChanges();

    expect(component.courses.length).toBe(2);
    expect(mockCourseService.getCourses).toHaveBeenCalled();
  });

  it('should apply filters correctly', () => {
    component.courses = [
      {
          id: '1', name: 'Course 1', description: 'Description 1',
          professorId: ''
      },
      {
          id: '2', name: 'Course 2', description: 'Description 2',
          professorId: ''
      }
    ];
    component.filters.name = 'Course 1';
    component.applyFilters();
    expect(component.filteredCourses.length).toBe(1);
    expect(component.filteredCourses[0].name).toBe('Course 1');
  });

  it('should navigate to create course page on onCreate', () => {
    spyOn(router, 'navigate');
    component.onCreate();
    expect(router.navigate).toHaveBeenCalledWith(['/course-create']);
  });

  it('should navigate to course detail page on onGetInfo', () => {
    spyOn(router, 'navigate');
    const course = { id: '1', name: 'Course 1', description: 'Description 1', professorId: 'prof1' };
    component.onGetInfo(course);
    expect(router.navigate).toHaveBeenCalledWith(['/course-detail/1']);
  });

  it('should delete course on onDelete', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    const course = { id: '1', name: 'Course 1', description: 'Description 1', professorId: 'prof1' };
    mockCourseService.deleteCourse.and.returnValue(of(void 0));
    component.onDelete(course);
    expect(mockCourseService.deleteCourse).toHaveBeenCalledWith('1');
  });

  it('should handle error when deleting course', () => {
    spyOn(window, 'alert');
    spyOn(window, 'confirm').and.returnValue(true);
    const course = { id: '1', name: 'Course 1', description: 'Description 1', professorId: 'prof1' };
    mockCourseService.deleteCourse.and.returnValue(throwError(() => new Error('Error deleting course')));
    component.onDelete(course);
    expect(mockCourseService.deleteCourse).toHaveBeenCalledWith('1');
    expect(window.alert).toHaveBeenCalledWith('An error occurred while deleting the course.');
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

  it('should navigate to the previous page on onPageChange', () => {
    component.pageNumber = 2;
    spyOn(component, 'loadCourses');
    component.onPageChange(1);
    expect(component.pageNumber).toBe(1);
    expect(component.loadCourses).toHaveBeenCalled();
  });

  it('should navigate to the next page on onPageChange', () => {
    component.pageNumber = 1;
    spyOn(component, 'loadCourses');
    component.onPageChange(2);
    expect(component.pageNumber).toBe(2);
    expect(component.loadCourses).toHaveBeenCalled();
  });

  it('should change page size and reset to first page on onPageSizeChange', () => {
    const event = { target: { value: '5' } } as unknown as Event;
    spyOn(component, 'loadCourses');
    component.onPageSizeChange(event);
    expect(component.pageSize).toBe(5);
    expect(component.pageNumber).toBe(1);
    expect(component.loadCourses).toHaveBeenCalled();
  });

  it('should remove token and navigate to welcome page on onLogout', () => {
    spyOn(localStorage, 'removeItem');
    spyOn(router, 'navigate');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});