import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { StudentDTO } from '../../models/student.model';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-student-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './student-create.component.html',
  styleUrls: ['./student-create.component.css']
})
export class StudentCreateComponent implements OnInit {
  studentForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router
  ) {
    this.studentForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(320)]],
      password: ['', [Validators.required, Validators.maxLength(200)]],
      status: [true, [Validators.required]],
      createdAt: [new Date(), [Validators.required]],
      lastLogin: [null],
    });

    this.studentForm.get('lastLogin')?.setValidators([this.lastLoginValidator.bind(this)]);
  }

  ngOnInit(): void {}

  lastLoginValidator(control: AbstractControl) {
    const createdAt = this.studentForm?.get('createdAt')?.value;
    const lastLogin = control.value;

    if (lastLogin && new Date(lastLogin) < new Date(createdAt)) {
      return { invalidLastLogin: true };
    }
    return null;
  }

  onSubmit(): void {
    console.log('Form Data:', this.studentForm.value);
    console.log('Form Valid?', this.studentForm.valid);

    if (this.studentForm.valid) {
      const studentData: StudentDTO = this.studentForm.value;
  
      studentData.createdAt = new Date(studentData.createdAt);
  
      if (studentData.lastLogin) {
        studentData.lastLogin = new Date(studentData.lastLogin);
      }
  
      this.studentService.createStudent(studentData).subscribe(
        (response) => {
          console.log('Student created successfully:', response);
          alert('Student created successfully!');
          this.router.navigate(['/student-list']);
        },
        (error) => {
          console.error('Error creating student:', error);
          alert('An error occurred while creating the student. Please try again.');
        }
      );
    } else {
      alert('Please fill out the form correctly.');
    }
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
