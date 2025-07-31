import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly FAKE_AUTH_KEY = 'lifenote_auth';
  readonly isAuthenticated = signal<boolean>(false);

  constructor(private router: Router) {
    // Check for auth status on service initialization
    const authStatus = sessionStorage.getItem(this.FAKE_AUTH_KEY);
    this.isAuthenticated.set(authStatus === 'true');
  }

  login(email: string, password: string) {
    // Dummy validation: any non-empty email/password is valid
    if (email && password) {
      sessionStorage.setItem(this.FAKE_AUTH_KEY, 'true');
      this.isAuthenticated.set(true);
      this.router.navigate(['/notes']); // Navigate to a default authenticated route
      return of(true);
    }
    return of(false);
  }

  logout() {
    sessionStorage.removeItem(this.FAKE_AUTH_KEY);
    this.isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }
}
