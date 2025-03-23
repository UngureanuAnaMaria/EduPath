import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { UserDTO } from '../models/user.model';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });

    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should register a user successfully', () => {
    const mockUser: UserDTO = { id: '', email: 'test@example.com', password: 'password123', admin: false };
    const mockResponse = { message: 'Registration successful' };

    service.register(mockUser).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${service['authApiUrl']}/register`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should handle registration error', () => {
    const mockUser: UserDTO = { id: '', email: 'test@example.com', password: 'password123', admin: false };
    const mockError = { error: { message: 'Registration failed' } };

    service.register(mockUser).subscribe(
      () => fail('expected an error, not a success'),
      error => {
        expect(error.message).toBe('Registration failed');
      }
    );

    const req = httpMock.expectOne(`${service['authApiUrl']}/register`);
    expect(req.request.method).toBe('POST');
    req.flush(mockError, { status: 400, statusText: 'Bad Request' });
  });

  it('should login a user successfully', () => {
    const mockResponse = { token: 'fake-token', admin: false };

    service.login('test@example.com', 'password123', 'student').subscribe(response => {
      expect(response).toEqual(mockResponse);
      expect(localStorage.getItem('token')).toBe('fake-token');
      expect(localStorage.getItem('role')).toBe('student');
    });

    const req = httpMock.expectOne(`${service['authApiUrl']}/login`);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should handle login error', () => {
    const mockError = { error: { message: 'Login failed' } };

    service.login('test@example.com', 'password123', 'student').subscribe(
      () => fail('expected an error, not a success'),
      error => {
        expect(error.message).toBe('Failed to log in.');
      }
    );

    const req = httpMock.expectOne(`${service['authApiUrl']}/login`);
    expect(req.request.method).toBe('POST');
    req.flush(mockError, { status: 400, statusText: 'Bad Request' });
  });
});