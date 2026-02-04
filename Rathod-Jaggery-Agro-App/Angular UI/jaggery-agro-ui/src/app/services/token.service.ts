import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenService {
  getRoles(): string[] {
    const token = localStorage.getItem('token');
    if (!token) return [];

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['role']
      ? Array.isArray(payload['role']) ? payload['role'] : [payload['role']]
      : [];
  }
}

