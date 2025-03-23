import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const { email, password} = this.loginForm.value; 

      this.userService.login(email, password, "").subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          alert('Logged in successfully!');

          // Redirect based on role
          if (response.admin === false) {
            this.router.navigate(['/student-list']);
          } else if (response.admin === true) {
            this.router.navigate(['/course-list']); 
          }
        },
        error: (error) => {
          console.error('Login failed:', error);
          alert('Login failed. Please check your credentials and try again.');
        }
      });
    } else {
      alert('Please fill out the form correctly.');
    }
  }
}
