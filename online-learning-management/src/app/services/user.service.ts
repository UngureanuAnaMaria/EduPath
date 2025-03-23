import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { UserDTO } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private authApiUrl = 'http://localhost:5020/api/v1/Auth';
  private usersApiUrl = 'http://localhost:5020/api/v1/Users';

  constructor(private http: HttpClient) {}

  public register(user: UserDTO): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.authApiUrl}/register`, user).pipe(
      catchError((error) => {
        console.error('Registration failed:', error);
        let errorMessage = 'Something went wrong!';
        if (error.error && error.error.message) {
          errorMessage = error.error.message;
        }
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  public login(email: string, password: string, role: string): Observable<{ token: string, admin: boolean }> {
    return this.http.post<{ token: string, admin: boolean }>(
      `${this.authApiUrl}/login`,
      { email, password, role }
    ).pipe(
      catchError((error) => {
        console.error('Login failed:', error);
        return throwError(() => new Error('Failed to log in.'));
      }),
      tap((response) => {
        // Salvează token-ul în localStorage
        localStorage.setItem('token', response.token);
        localStorage.setItem('role', response.admin ? 'instructor' : 'student');
      })
    );
  }
}
