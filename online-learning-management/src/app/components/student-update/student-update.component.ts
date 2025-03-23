import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { StudentDTO } from '../../models/student.model';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-student-update',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './student-update.component.html',
  styleUrls: ['./student-update.component.css']
})
export class StudentUpdateComponent implements OnInit {
  studentForm: FormGroup;
  studentIdControl: FormControl;
  student: StudentDTO | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.studentIdControl = new FormControl('', [Validators.required, this.validateGuid]);
    this.studentForm = this.fb.group({
      id: ['', [Validators.required, this.validateGuid]],
      name: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(320)]],
      password: ['', [Validators.required, Validators.maxLength(200)]],
      status: [false, [Validators.required]],
      createdAt: ['', [Validators.required]],
      lastLogin: ['', [Validators.required, this.validateLastLogin.bind(this)]],
      studentCourses: [[], [Validators.required, Validators.maxLength(200)]]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.studentIdControl.setValue(id);
        this.searchStudent();
      }
    });
  }

  searchStudent() {
    if (this.studentIdControl.valid) {
      this.studentService.getStudentById(this.studentIdControl.value).subscribe(
        (data) => {
          if (this.isInvalidStudent(data)) {
            this.student = null;
            this.errorMessage = 'Invalid student ID.';
          } else {
            this.student = data;
            this.errorMessage = null;
            this.studentForm.patchValue({
              ...data,
              createdAt: this.formatDate(data.createdAt),
              lastLogin: this.formatDate(data.lastLogin)
            });
          }
          console.log('Student data:', data);
        },
        (error) => {
          this.student = null;
          this.errorMessage = 'Error fetching student.';
          console.error('Error fetching student', error);
        }
      );
    }
  }

  updateStudent(): void {
    if (this.studentForm.valid) {
      const formValue = this.studentForm.value;
      const updatedStudent = {
        ...formValue,
        studentCourses: [{courseId: formValue.studentCourses}],
        createdAt: new Date(formValue.createdAt),
        lastLogin: new Date(formValue.lastLogin)
      };

      this.studentService.updateStudent(updatedStudent).subscribe(
        (data) => {
          console.log('Student updated successfully', data);
          alert('Student updated successfully!');
          this.router.navigate(['/student-list']);
        },
        (error) => {
          console.error('Error updating student', error);
          alert('An error occurred while updating the student. Please try again.');
        }
      );
    }
  }

  formatDate(date: Date): string {
    const d = new Date(date);
    const month = '' + (d.getMonth() + 1);
    const day = '' + d.getDate();
    const year = d.getFullYear();

    return [year, month.padStart(2, '0'), day.padStart(2, '0')].join('-');
  }

  validateEmail(control: any) {
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailPattern.test(control.value) ? null : { invalidEmail: true };
  }

  validateGuid(control: any) {
    const guidPattern = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidPattern.test(control.value) ? null : { invalidGuid: true };
  }

  validateLastLogin(control: AbstractControl) {
    const createdAt = this.studentForm?.get('createdAt')?.value;
    if (createdAt && control.value) {
      return new Date(control.value) >= new Date(createdAt) ? null : { invalidLastLogin: true };
    }
    return null;
  }

  isInvalidStudent(student: StudentDTO): boolean {
    return student.id === '00000000-0000-0000-0000-000000000000' || !student.name || !student.email;
  }

  onGetPredictions() {
    this.router.navigate(['/student-predictions-extern']);
  }

  onLogout(): void {
    localStorage.removeItem('token');
    alert('Logged out successfully!');
    //this.toastr.success('Logged out successfully!', 'Logout');
    //localStorage.clear();
    this.router.navigate(['/welcome-page']);
  }
}
