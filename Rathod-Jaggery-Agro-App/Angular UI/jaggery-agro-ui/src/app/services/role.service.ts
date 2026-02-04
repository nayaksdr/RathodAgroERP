import { Injectable } from '@angular/core';
import { TokenService } from './token.service';

@Injectable({ providedIn: 'root' })
export class RoleService {

  constructor(private token: TokenService) {}

  isAdmin() {
    return this.token.getRoles().includes('Admin');
  }

  isLabor() {
    return this.token.getRoles().includes('Labor');
  }
}
