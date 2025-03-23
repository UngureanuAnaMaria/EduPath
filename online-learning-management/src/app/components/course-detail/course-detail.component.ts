import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { CourseDTO } from '../../models/course.model';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-course-update',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.css']
})
export class CourseDetailComponent implements OnInit {
  courseIdControl: FormControl;
  course: CourseDTO | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private courseService: CourseService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.courseIdControl = new FormControl('', [Validators.required, this.validateGuid]);
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.courseIdControl.setValue(id);
        this.searchCourse();
      }
    });
  }

  searchCourse() {
    if (this.courseIdControl.valid) {
      this.courseService.getCourseById(this.courseIdControl.value).subscribe(
        (data) => {
          if (this.isInvalidCourse(data)) {
            this.course = null;
            this.errorMessage = 'Invalid course ID.';
          } else {
            this.course = data;
            this.errorMessage = null;
          }
          //console.log('Course data:', data);
        },
        (error) => {
          this.course = null;
          this.errorMessage = 'Error fetching course.';
          console.error('Error fetching course', error);
        }
      );
    }
  }

  deleteCourse(): void {
    if (confirm('Are you sure you want to delete this course?')) {
      if (this.course) {
        this.courseService.deleteCourse(this.course.id).subscribe(
          () => {
            console.log('Course deleted successfully');
            alert('Course deleted successfully!');
            this.course = null;
            this.courseIdControl.reset();
            this.router.navigate(['/course-list']);
          },
          (error) => {
            console.error('Error deleting course', error);
            alert('An error occurred while deleting the course.');
          }
        );
      } else {
        alert('No course selected to delete.');
      }
    }
  }

  validateGuid(control: any) {
    const guidPattern = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidPattern.test(control.value) ? null : { invalidGuid: true };
  }

  isInvalidCourse(course: CourseDTO): boolean {
    return course.id === '00000000-0000-0000-0000-000000000000' || !course.name || !course.description;
  }

  onLogout(): void {
    localStorage.removeItem('token');
    alert('Logged out successfully!');
    //this.toastr.success('Logged out successfully!', 'Logout');
    //localStorage.clear();
    this.router.navigate(['/welcome-page']);
  }
}
