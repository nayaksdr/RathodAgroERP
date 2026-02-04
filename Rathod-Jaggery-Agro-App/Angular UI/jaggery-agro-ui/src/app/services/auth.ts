import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface User {
  id?: number;
  name: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {

  private api = `${environment.apiUrl}/api/auth`;

  private userSubject = new BehaviorSubject<User | null>(this.loadUser());
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) {}

  /* ---------- LOGIN ---------- */
 login(data: any) {
  return this.http.post<any>(`${this.api}/login`, data).pipe(
    tap(res => {

      if (res?.token) {
        localStorage.setItem('token', res.token);
      }

      if (res?.user) {
        localStorage.setItem('user', JSON.stringify(res.user));
        this.userSubject.next(res.user);
      } else {
        localStorage.removeItem('user');
        this.userSubject.next(null);
      }

    })
  );
}


  /* ---------- LOGOUT ---------- */
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  /* ---------- HELPERS ---------- */
 isLoggedIn(): boolean {
  return !!this.getToken() && !!this.currentUser;
}


  getToken(): string | null {
    return localStorage.getItem('token');
  }

  get currentUser(): User | null {
    return this.userSubject.value;
  }

  private loadUser(): User | null {
  const userStr = localStorage.getItem('user');

  if (!userStr || userStr === 'undefined' || userStr === 'null') {
    return null;
  }

  try {
    return JSON.parse(userStr) as User;
  } catch (error) {
    console.error('Invalid user in localStorage. Clearing it.');
    localStorage.removeItem('user');
    return null;
  }
}


}
