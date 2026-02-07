import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isSignupMode = false;

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  signupForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required]
  });

  errorMessage = '';

  toggleMode() {
    this.isSignupMode = !this.isSignupMode;
    this.errorMessage = '';
    // Reset forms when toggling
    this.loginForm.reset();
    this.signupForm.reset();
  }

  async login() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const user = await this.authService.login(email!, password!);
      console.log(user);
    }
  }

  async signup() {
    if (this.signupForm.valid) {
      const { email, password, confirmPassword } = this.signupForm.value;

      if (password !== confirmPassword) {
        this.errorMessage = 'Passwords do not match';
        return;
      }

      const user = await this.authService.signUp(email!, password!);
      console.log(user);
    }
  }

  private getErrorMessage(errorCode: string): string {
    switch (errorCode) {
      case 'auth/email-already-in-use':
        return 'This email is already registered';
      case 'auth/invalid-email':
        return 'Invalid email address';
      case 'auth/user-not-found':
        return 'No account found with this email';
      case 'auth/wrong-password':
        return 'Incorrect password';
      case 'auth/weak-password':
        return 'Password should be at least 6 characters';
      default:
        return 'An error occurred. Please try again';
    }
  }
}