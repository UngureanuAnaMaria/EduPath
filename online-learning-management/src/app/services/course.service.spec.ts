import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CourseService } from './course.service';
import { CourseDTO } from '../models/course.model';

describe('CourseService', () => {
  let service: CourseService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CourseService]
    });

    service = TestBed.inject(CourseService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch courses with pagination', () => {
    const mockResponse = {
      courses: [
        { id: '1', name: 'Course 1', description: 'Description 1', professorId: '1' },
        { id: '2', name: 'Course 2', description: 'Description 2', professorId: '2' }
      ],
      totalCount: 2
    };

    service.getCourses(1, 10).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}?pageNumber=1&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should create a course', () => {
    const mockCourse: CourseDTO = { id: '1', name: 'Course 1', description: 'Description 1', professorId: '1' };

    service.createCourse(mockCourse).subscribe(response => {
      expect(response).toEqual(mockCourse);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    req.flush(mockCourse);
  });

  it('should fetch a course by ID', () => {
    const mockCourse: CourseDTO = { id: '1', name: 'Course 1', description: 'Description 1', professorId: '1' };

    service.getCourseById('1').subscribe(response => {
      expect(response).toEqual(mockCourse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockCourse);
  });

  it('should update a course', () => {
    const mockCourse: CourseDTO = { id: '1', name: 'Course 1', description: 'Description 1', professorId: '1' };

    service.updateCourse(mockCourse).subscribe(response => {
      expect(response).toEqual(mockCourse);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockCourse);
  });

  it('should delete a course', () => {
    service.deleteCourse('1').subscribe(response => {
      expect(response).toBeUndefined();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});