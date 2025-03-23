import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { CourseDTO } from '../../models/course.model';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-course-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.css']
})
export class CourseCreateComponent implements OnInit {
  courseForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private courseService: CourseService,
    private router: Router
  ) {
    this.courseForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    console.log('Form Data:', this.courseForm.value);
    console.log('Form Valid?', this.courseForm.valid);

    if (this.courseForm.valid) {
      const courseData: CourseDTO = this.courseForm.value;
  
      this.courseService.createCourse(courseData).subscribe(
        (response) => {
          console.log('Course created successfully:', response);
          alert('Course created successfully!');
          this.router.navigate(['/course-list']);
        },
        (error) => {
          console.error('Error creating course:', error);
          alert('An error occurred while creating the course. Please try again.');
        }
      );
    } else {
      alert('Please fill out the form correctly.');
    }
  }  

  onLogout(): void {
    localStorage.removeItem('token');
    alert('Logged out successfully!');
    //this.toastr.success('Logged out successfully!', 'Logout');
    //localStorage.clear();
    this.router.navigate(['/welcome-page']);
  }
}
