// auth.service.ts
import { inject, Injectable, signal, computed } from '@angular/core';
import { Auth, user, createUserWithEmailAndPassword, signInWithEmailAndPassword, signOut, GoogleAuthProvider, signInWithPopup } from '@angular/fire/auth';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { toSignal } from '@angular/core/rxjs-interop';
import { from, Observable, switchMap, tap, map } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly isAuthenticated = signal(false);
  private auth = inject(Auth);
  private router = inject(Router);
  private http = inject(HttpClient);

  private apiBase = environment.apiHost;

  // Firebase user as signal
  currentUser = toSignal(user(this.auth), { initialValue: null });
  currentUserDetails = signal({});

  // Convenience computed
  readonly isLoggedIn = computed(() => !!this.currentUser());

  constructor() {
    // Keep isAuthenticated in sync with Firebase
    if (this.currentUser()) {
      this.isAuthenticated.set(true);
    }
  }

  private getIdToken$(): Observable<string> {
    return from(
      (async () => {
        const currentUser = this.auth.currentUser;
        if (currentUser) {
          return await currentUser.getIdToken(false);
        }
        return 'null';
      })()
    );
  }

  private createBackendUser$(username: string): Observable<any> {
    const createUserPayload = {
      firstName: '',
      lastName: '',
      username: username,
    };

    return this.http.post(`${this.apiBase}/user`, createUserPayload);
  }

  checkUsernameAvailability$(username: string): Observable<boolean> {
    return this.http.get<{ available: boolean }>(
      `${this.apiBase}/user/check-username/${username}`
    ).pipe(
      map(res => res.available)
    );
  }

  signUp(email: string, password: string, username: string): Observable<any> {
    return this.checkUsernameAvailability$(username).pipe(
      switchMap((isAvailable) => {
        if (!isAvailable) {
          throw new Error('USERNAME_TAKEN');
        }
        return from(createUserWithEmailAndPassword(this.auth, email, password));
      }),
      switchMap(() => this.getIdToken$()),
      tap((token) => {
        localStorage.setItem('toxin', token);
        this.isAuthenticated.set(true);
      }),
      switchMap(() => this.createBackendUser$(username)),
      switchMap(() => this.http.get(`${this.apiBase}/user/me`)),
      tap(() => {
        this.router.navigate(['/notes']);
      })
    );
  }

  login(email: string, password: string): void {
    from(signInWithEmailAndPassword(this.auth, email, password)).pipe(
      switchMap(() => this.getIdToken$()),
      tap((token) => {
        localStorage.setItem('toxin', token);
        this.isAuthenticated.set(true);
      }),
      // optional: fetch profile if needed
      switchMap(() => this.http.get(`${this.apiBase}/user/me`)),
      tap(() => {
        this.router.navigate(['/notes']);
      })
    ).subscribe(user => {
      console.log(user);
      this.currentUserDetails.set(user);
    });
  }

  googleLogin(): void {
    const provider = new GoogleAuthProvider();

    from(signInWithPopup(this.auth, provider)).pipe(
      switchMap((result) => from(result.user.getIdToken())),
      tap((token) => {
        localStorage.setItem('toxin', token);
        this.isAuthenticated.set(true);
      }),
      switchMap(() => {
        const email = this.currentUser()?.email ?? '';
        const defaultUsername = email ? email.split('@')[0] : 'user';
        return this.createBackendUser$(defaultUsername);
      }),
      tap(() => {
        this.router.navigate(['/notes']);
      })
    ).subscribe();
  }

  logout(): Observable<void> {
    return from(
      (async () => {
        localStorage.clear();
        this.isAuthenticated.set(false);
        await signOut(this.auth);
      })()
    );
  }
}
