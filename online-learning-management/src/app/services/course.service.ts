import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CourseDTO } from '../models/course.model';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  private apiUrl = 'http://localhost:5020/api/v1/Courses';

  constructor(private http: HttpClient) {}

  public getCourses(pageNumber: number, pageSize: number): Observable<{ courses: CourseDTO[], totalCount: number }> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ courses: CourseDTO[], totalCount: number }>(this.apiUrl, { params });
  }

  public createCourse(course: CourseDTO): Observable<CourseDTO> {
    return this.http.post<CourseDTO>(this.apiUrl, course);
  }

  public getCourseById(id: string): Observable<CourseDTO> {
    return this.http.get<CourseDTO>(`${this.apiUrl}/${id}`);
  }

  public updateCourse(course: CourseDTO): Observable<CourseDTO> {
    return this.http.put<CourseDTO>(`${this.apiUrl}/${course.id}`, course);
  }

  public deleteCourse(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
