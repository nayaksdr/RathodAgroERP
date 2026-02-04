import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification';
import { AuthService } from '../../services/auth';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
hidePassword = true;

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private notify = inject(NotificationService);

  form = this.fb.nonNullable.group({
    username: ['', Validators.required], // ✅ removed email validator
    password: ['', Validators.required]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.notify.showError('Please fill in all required fields');
      return;
    }

    const payload = this.form.getRawValue();

    this.auth.login(payload).subscribe({
      next: () => {
        // ✅ AuthService already stores token + user
        this.router.navigate(['/dashboard']);
        this.notify.showSuccess('Login successful');        
      },
      error: (err) => {
        this.notify.showError('Invalid Username or Password');
        console.error('Login error:', err);        
      }
    });
  }
}
