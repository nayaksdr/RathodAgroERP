import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter, Subject, takeUntil } from 'rxjs';

type SidebarMenus = 'employee' | 'payments' | 'produce' | 'cane' | 'expense';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar-component.html',
  styleUrls: ['./sidebar-component.css'],
})
export class SidebarComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  private removeMouseEnter?: () => void;
  private removeMouseLeave?: () => void;

  // Sidebar states
  isCollapsed = true;        // 👈 Start collapsed (Professional ERP style)
  isMobileOpen = false;
  isMobile = false;

  // Theme
  darkMode = localStorage.getItem('theme') === 'dark';

  // Menu open state
  isOpen: Record<SidebarMenus, boolean> = {
    employee: false,
    payments: false,
    produce: false,
    cane: false,
    expense: false
  };

  constructor(private router: Router, private renderer: Renderer2) {}

  ngOnInit(): void {
    this.checkScreen();
    this.applyTheme();
    this.autoOpenMenu();

    // Resize listener
    window.addEventListener('resize', this.handleResize);

    // Router listener
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.autoOpenMenu();
        if (this.isMobile) this.isMobileOpen = false;
      });

    // Hover expand effect (desktop only)
    setTimeout(() => this.initHoverEffect(), 0);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    window.removeEventListener('resize', this.handleResize);

    // Remove hover listeners safely
    if (this.removeMouseEnter) this.removeMouseEnter();
    if (this.removeMouseLeave) this.removeMouseLeave();
  }

  /** Handle window resize */
  private handleResize = (): void => this.checkScreen();

  /** Detect screen type */
  private checkScreen(): void {
    this.isMobile = window.innerWidth < 768;

    if (this.isMobile) {
      this.isCollapsed = false;
    } else {
      this.isCollapsed = true; // Always collapsed by default on desktop
    }
  }

  /** Initialize hover animation */
  private initHoverEffect(): void {
    if (this.isMobile) return;

    const sidebar = document.querySelector('.sidebar');
    if (!sidebar) return;

    this.removeMouseEnter = this.renderer.listen(sidebar, 'mouseenter', () => {
      if (!this.isMobile) this.isCollapsed = false;
    });

    this.removeMouseLeave = this.renderer.listen(sidebar, 'mouseleave', () => {
      if (!this.isMobile) this.isCollapsed = true;
    });
  }

  /** Toggle for mobile */
  toggleMobile(): void {
    if (this.isMobile) {
      this.isMobileOpen = !this.isMobileOpen;
    }
  }

  /** Close mobile sidebar */
  closeMobile(): void {
    if (this.isMobile) {
      this.isMobileOpen = false;
    }
  }

  /** Toggle submenu */
  toggleDropdown(menu: SidebarMenus): void {
    Object.keys(this.isOpen).forEach(key => {
      this.isOpen[key as SidebarMenus] =
        key === menu ? !this.isOpen[key as SidebarMenus] : false;
    });
  }

  /** Auto open based on route */
private autoOpenMenu(): void {
  let url = this.router.url;

  // Normalize URL
  if (!url || url === '/' || url === '') {
    url = '/dashboard'; // 👈 treat root as dashboard
  }

  // Reset all first
  Object.keys(this.isOpen).forEach(key => {
    this.isOpen[key as SidebarMenus] = false;
  });

  if (
    url.includes('dashboard') ||
    url.includes('labor') ||
    url.includes('labor-type') ||
     url.includes('labor-type-rate') ||
    url.includes('ai-agent') ||
    url.includes('attendance')
  ) {
    this.isOpen.employee = true;
  }
  else if (url.includes('payment') || url.includes('advance-payment')) {
    this.isOpen.payments = true;
  }
  else if (url.includes('produce') || url.includes('dealer') || url.includes('jaggery-sale') || url.includes('jaggery-report.component')|| url.includes('cane-purchase') || url.includes('dealer-balance')) {
    this.isOpen.produce = true;
  }
  else if (url.includes('cane') || url.includes('farmer')|| url.includes('cane-purchase')|| url.includes('cane-advance')|| url.includes('cane-payment')) {
    this.isOpen.cane = true;
  }
  else if (url.includes('expense-type') || url.includes('expense') ||  url.includes('splitwise') || url.includes('jaggery-share-dashboard')) {
    this.isOpen.expense = true;
  }
}


  /** Theme toggle */
  toggleTheme(): void {
    this.darkMode = !this.darkMode;
    localStorage.setItem('theme', this.darkMode ? 'dark' : 'light');
    this.applyTheme();
  }

  /** Apply theme */
  private applyTheme(): void {
    document.body.classList.toggle('dark-mode', this.darkMode);
  }
}
