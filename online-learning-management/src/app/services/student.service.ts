import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentDTO } from '../models/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private apiUrl = 'http://localhost:5020/api/v1/Students';

  constructor(private http: HttpClient) {}

  public getStudents(pageNumber: number, pageSize: number): Observable<{ students: StudentDTO[], totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
  
    return this.http.get<{ students: StudentDTO[], totalCount: number }>(this.apiUrl, { params });
  }
  
  public createStudent(student: StudentDTO): Observable<StudentDTO> {
    return this.http.post<StudentDTO>(this.apiUrl, student);
  }

  public getStudentById(id: string): Observable<StudentDTO> {
    return this.http.get<StudentDTO>(`${this.apiUrl}/${id}`);
  }

  public updateStudent(student: StudentDTO): Observable<StudentDTO> {
    return this.http.put<StudentDTO>(`${this.apiUrl}/${student.id}`, student);
  }

  public deleteStudent(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
