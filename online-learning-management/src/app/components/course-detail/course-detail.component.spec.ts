import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CourseDetailComponent } from './course-detail.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CourseService } from '../../services/course.service';
import { of, throwError } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseDTO } from '../../models/course.model';
import { By } from '@angular/platform-browser';

describe('CourseDetailComponent', () => {
  let component: CourseDetailComponent;
  let fixture: ComponentFixture<CourseDetailComponent>;
  let courseService: jasmine.SpyObj<CourseService>;
  let router: Router;

  beforeEach(async () => {
    const courseServiceSpy = jasmine.createSpyObj('CourseService', ['getCourseById', 'deleteCourse']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule],
      declarations: [CourseDetailComponent],
      providers: [
        { provide: CourseService, useValue: courseServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({ get: () => '1' })
          }
        }
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CourseDetailComponent);
    component = fixture.componentInstance;
    courseService = TestBed.inject(CourseService) as jasmine.SpyObj<CourseService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set courseIdControl value from route parameters on init', () => {
    fixture.detectChanges();
    expect(component.courseIdControl.value).toBe('1');
  });

  it('should handle error if course not found', () => {
    courseService.getCourseById.and.returnValue(throwError(() => new Error('Course not found')));
    component.searchCourse();

    fixture.detectChanges();
    expect(component.errorMessage).toBe('Error fetching course.');
  });

  it('should validate GUID on invalid courseId input', () => {
    const invalidGuid = 'invalid-guid';
    component.courseIdControl.setValue(invalidGuid);
    component.searchCourse();

    expect(component.courseIdControl.hasError('invalidGuid')).toBeTrue();
  });

  it('should delete course on delete button click', () => {
    const mockCourse: CourseDTO = {
      id: '1',
      name: 'Course 1',
      description: 'Description 1',
      professorId: '1'
    };
    component.course = mockCourse;

    courseService.deleteCourse.and.returnValue(of(undefined));
    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteCourse();

    expect(courseService.deleteCourse).toHaveBeenCalledWith('1');
    expect(router.navigate).toHaveBeenCalledWith(['/course-list']);
    expect(component.course).toBeNull();
    expect(component.courseIdControl.value).toBeNull();
    expect(component.errorMessage).toBeNull();
  });

  it('should show error if delete course fails', () => {
    const mockCourse: CourseDTO = {
      id: '1',
      name: 'Course 1',
      description: 'Description 1',
      professorId: '1'
    };
    component.course = mockCourse;

    courseService.deleteCourse.and.returnValue(throwError(() => new Error('Delete failed')));
    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteCourse();

    expect(courseService.deleteCourse).toHaveBeenCalledWith('1');
    expect(component.errorMessage).toBe('An error occurred while deleting the course.');
  });

  it('should call searchCourse on init and display course data', () => {
    const mockCourse: CourseDTO = {
      id: '1',
      name: 'Course 1',
      description: 'Description 1',
      professorId: '1'
    };

    courseService.getCourseById.and.returnValue(of(mockCourse));
    component.searchCourse();

    fixture.detectChanges();

    const nameElement = fixture.debugElement.query(By.css('.card-title')).nativeElement;
    expect(nameElement.textContent).toBe('Course 1');
    expect(courseService.getCourseById).toHaveBeenCalledWith('1');
  });

  it('should remove token and navigate to welcome page on onLogout', () => {
    spyOn(localStorage, 'removeItem');
    spyOn(router, 'navigate');
    component.onLogout();
    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(router.navigate).toHaveBeenCalledWith(['/welcome-page']);
  });
});