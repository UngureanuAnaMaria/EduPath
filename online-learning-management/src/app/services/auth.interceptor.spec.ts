import { HttpInterceptorFn, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { of } from 'rxjs';
import { authInterceptor } from './auth.interceptor';

describe('authInterceptor', () => {
  let next: HttpHandler;
  let handleSpy: jasmine.Spy;

  beforeEach(() => {
    handleSpy = jasmine.createSpy('handle').and.returnValue(of({} as HttpEvent<any>));
    next = { handle: handleSpy };
  });

  it('should add Authorization header if token is present', () => {
    spyOn(localStorage, 'getItem').and.returnValue('fake-token');
    const req = new HttpRequest('GET', '/test');
    // authInterceptor(req, next);

    expect(handleSpy).toHaveBeenCalled();
    const modifiedReq = handleSpy.calls.mostRecent().args[0] as HttpRequest<any>;
    expect(modifiedReq.headers.get('Authorization')).toBe('Bearer fake-token');
  });

  it('should not add Authorization header if token is not present', () => {
    spyOn(localStorage, 'getItem').and.returnValue(null);
    const req = new HttpRequest('GET', '/test');
    // authInterceptor(req, next);

    expect(handleSpy).toHaveBeenCalled();
    const modifiedReq = handleSpy.calls.mostRecent().args[0] as HttpRequest<any>;
    expect(modifiedReq.headers.has('Authorization')).toBeFalse();
  });

  it('should log messages to the console', () => {
    spyOn(console, 'log');
    spyOn(console, 'error');
    spyOn(localStorage, 'getItem').and.returnValue('fake-token');
    const req = new HttpRequest('GET', '/test');
    // authInterceptor(req, next);

    expect(console.log).toHaveBeenCalledWith('Interceptor is active');
    expect(console.log).toHaveBeenCalledWith('Interceptor: Token found: Bearer fake-token');
    expect(console.log).toHaveBeenCalledWith('Interceptor: Request headers:', jasmine.any(Object));
    expect(console.error).not.toHaveBeenCalled();
  });

  it('should log an error message if token is not found', () => {
    spyOn(console, 'log');
    spyOn(console, 'error');
    spyOn(localStorage, 'getItem').and.returnValue(null);
    const req = new HttpRequest('GET', '/test');
    // authInterceptor(req, next);

    expect(console.log).toHaveBeenCalledWith('Interceptor is active');
    expect(console.error).toHaveBeenCalledWith('Interceptor: No token found in localStorage');
    expect(console.log).toHaveBeenCalledWith('Interceptor: Request headers:', jasmine.any(Object));
  });
});