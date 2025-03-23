import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StudentService } from './student.service';
import { StudentDTO, StudentDataExternDTO, StudentPredictions, StudentPredictionsExtern } from '../models/student.model';

export interface LocalStudentPredictionsExtern {

  averageGrade: number;

  learningPath: string;

}

describe('StudentService', () => {
  let service: StudentService;
  let httpMock: HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StudentService]
    });

    service = TestBed.inject(StudentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch students with pagination', () => {
    const mockResponse = {
      students: [
        { id: '1', name: 'John Doe', email: 'john@example.com', password: 'password123', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [] },
        { id: '2', name: 'Jane Doe', email: 'jane@example.com', password: 'password123', status: false, createdAt: new Date(), lastLogin: new Date(), studentCourses: [] }
      ],
      totalCount: 2
    };

    service.getStudents(1, 10).subscribe(response => expect(response).toEqual(mockResponse));

    const req = httpMock.expectOne(`${service['apiUrl']}?pageNumber=1&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should create a student', () => {
    const mockStudent: StudentDTO = {
      id: '1', name: 'John Doe', email: 'john@example.com', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [],
      password: ''
    };

    service.createStudent(mockStudent).subscribe(response => {
      expect(response).toEqual(mockStudent);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    req.flush(mockStudent);
  });

  it('should fetch a student by ID', () => {
    const mockStudent: StudentDTO = {
      id: '1', name: 'John Doe', email: 'john@example.com', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [],
      password: ''
    };

    service.getStudentById('1').subscribe(response => {
      expect(response).toEqual(mockStudent);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStudent);
  });

  it('should update a student', () => {
    const mockStudent: StudentDTO = {
      id: '1', name: 'John Doe', email: 'john@example.com', status: true, createdAt: new Date(), lastLogin: new Date(), studentCourses: [],
      password: ''
    };

    service.updateStudent(mockStudent).subscribe(response => {
      expect(response).toEqual(mockStudent);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockStudent);
  });

  it('should delete a student', () => {
    service.deleteStudent('1').subscribe(response => {
      expect(response).toBeUndefined();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should fetch student predictions', () => {
    const mockPredictions: StudentPredictions = { averageGrade: 4.0, percentageCompletedCourses: 80, learningPath: 'Data Science' };

    service.getStudentPredictions('1').subscribe(response => {
      expect(response).toEqual(mockPredictions);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1/predictions`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPredictions);
  });
});