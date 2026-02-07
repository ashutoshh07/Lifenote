import { inject, Injectable, signal } from '@angular/core';
import { Auth, user, createUserWithEmailAndPassword, signInWithEmailAndPassword, signOut, GoogleAuthProvider, signInWithPopup } from '@angular/fire/auth';
import { Router } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly isAuthenticated = signal<boolean>(false);
  private auth = inject(Auth);

  constructor(private router: Router) { }

  currentUser = toSignal(user(this.auth), { initialValue: null });

  async getIdToken(): Promise<string> {
    const currentUser = this.auth.currentUser;
    if (currentUser) {
      // forceRefresh = true will get a fresh token if the current one is expired
      return await currentUser.getIdToken(false);
    }
    return "null";
  }

  async signUp(email: string, password: string) {
    return createUserWithEmailAndPassword(this.auth, email, password).then((userCredential) => {
      const user = userCredential.user;
      this.isAuthenticated.set(true);
      this.router.navigate(['/notes']); // Navigate to a default authenticated route
      this.getIdToken().then(token => {
        localStorage.setItem('toxin', token);
      });
      // Optionally send verification email
      return user;
    }).catch((error) => { throw error; });
  }

  async login(email: string, password: string) {
    return signInWithEmailAndPassword(this.auth, email, password).then((userCredential) => {
      const user = userCredential.user;
      this.getIdToken().then(token => {
        localStorage.setItem('toxin', token);
      });
      this.isAuthenticated.set(true);
      this.router.navigate(['/notes']); // Navigate to a default authenticated route
      return user;
    }).catch((error) => { throw error; });
  }

  googleLogin() {
    const provider = new GoogleAuthProvider();
    return signInWithPopup(this.auth, provider).then((result) => {
      const user = result.user;
      result.user.getIdToken().then(token => {
        localStorage.setItem('toxin', token);
      });
      this.isAuthenticated.set(true);
      return user;
    }).catch((error) => { throw error; });
  }

  async logout() {
    localStorage.clear();
    this.isAuthenticated.set(false);
    return signOut(this.auth);
  }
}
