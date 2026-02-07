import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
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

  isSignupMode = false;

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  signupForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
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
      this.authService.login(email!, password!);
    }
  }

  async signup() {
    if (this.signupForm.valid) {
      const { email, password, confirmPassword, username } = this.signupForm.value;
      this.errorMessage = '';

      if (password !== confirmPassword) {
        this.errorMessage = 'Passwords do not match';
        return;
      }

      this.authService.signUp(email!, password!, username!).subscribe({
        next: (user) => console.log('User created:', user),
        error: (err) => {
          if (err.message === 'USERNAME_TAKEN') {
            this.errorMessage = 'Username is already taken';
          } else {
            this.errorMessage = this.getErrorMessage(err?.code ?? '');
          }
        }
      });
    }
  }

  private getErrorMessage(errorCode: string): string {
    switch (errorCode) {
      case 'auth/email-already-in-use':
        return 'This email is already registered';
      case 'auth/invalid-email':
        return 'Invalid email address';
      case 'auth/weak-password':
        return 'Password should be at least 6 characters';
      default:
        return 'An error occurred. Please try again';
    }
  }
}