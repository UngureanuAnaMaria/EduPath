import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../models/user.model';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],  // Câmpul pentru email
      password: ['', [Validators.required, Validators.minLength(6)]],  // Câmpul pentru parolă
      confirmPassword: ['', [Validators.required]]  // Câmpul pentru confirmarea parolei
    });
  }

  // Metoda pentru a înregistra un utilizator
  onRegister(): void {
    if (this.registerForm.valid && this.passwordsMatch()) {
      const { email, password } = this.registerForm.value;
       // Citim rolul din localStorage
      const role = localStorage.getItem('selectedRole');  // Citim rolul stocat în localStorage
    
      // Verificăm dacă rolul este definit
      if (!role) {
        alert('No role selected. Please go back and choose a role.');
        return;
      }

    // Setăm admin pe baza rolului selectat
    const admin = role === 'instructor' ? true : false;
      const userDTO: UserDTO = {
        id: '',  // ID-ul va fi generat pe server
        email,   // Email-ul utilizatorului
        password,  // Parola utilizatorului
        admin
      };

      // Apelăm serviciul pentru a înregistra utilizatorul
      this.userService.register(userDTO).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          alert('Account created successfully!');
          this.router.navigate(['/login']);  // Navigăm către pagina de login
        },
        error: (error) => {
          console.error('Registration failed:', error);
          alert('Registration failed. Please try again.');
        }
      });
    } else {
      alert('Please fill out the form correctly.');
    }
  }

  // Verificăm dacă parolele se potrivesc
  passwordsMatch(): boolean {
    return this.registerForm.get('password')?.value === this.registerForm.get('confirmPassword')?.value;
  }
}
