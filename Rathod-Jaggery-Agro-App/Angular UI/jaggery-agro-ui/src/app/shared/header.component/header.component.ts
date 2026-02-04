import { Component, EventEmitter, inject, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { AuthService,User } from '../../services/auth';
import { Inject } from '@angular/core';
import { Router } from '@angular/router';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,MatIcon],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  @Input() title: string = 'Dashboard';
  @Output() menuToggle = new EventEmitter<void>();
private auth = inject(AuthService);
  private router = inject(Router);
  currentTime: string = '';
  currentDate: string = '';

  showUserMenu = false;
  showNotifications = false;
  darkMode = localStorage.getItem('theme') === 'dark';

  private intervalId: any;

  ngOnInit(): void {
    this.updateDateTime();

    this.intervalId = setInterval(() => {
      this.updateDateTime();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  updateDateTime() {
    const now = new Date();
    this.currentTime = now.toLocaleTimeString();
    this.currentDate = now.toDateString();
  }
   get user(): User | null {
    return this.auth.currentUser;
  }

  toggleMenu() {
    this.menuToggle.emit();
  }

  toggleUserMenu() {
    this.showUserMenu = !this.showUserMenu;
    this.showNotifications = false;
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
    this.showUserMenu = false;
  }

  toggleTheme() {
    this.darkMode = !this.darkMode;
    localStorage.setItem('theme', this.darkMode ? 'dark' : 'light');
    document.body.classList.toggle('dark-theme', this.darkMode);
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
