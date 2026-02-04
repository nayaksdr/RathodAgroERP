import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';

import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';

import { SidebarLayoutComponent } from './layout/sidebar-layout-component/sidebar-layout-component';
export const routes: Routes = [

  // ✅ Default redirect when app loads
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  // 🔓 PUBLIC LOGIN
  { path: 'login', component: LoginComponent },

  // 🔐 PROTECTED AREA (Everything after login)
  {
    path: '',
    component: SidebarLayoutComponent,
    canActivate: [authGuard],
    children: [

      // Default inside dashboard layout
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      // DASHBOARD
      { path: 'dashboard', component: DashboardComponent },

      // AI
      {
        path: 'ai-agent',
        loadComponent: () =>
          import('./pages/ai-agent.component/ai-agent.component')
            .then(m => m.AiAgentComponent)
      },

      // LABORS      
    {
      path: 'labor',
      loadComponent: () =>
        import('./pages/labor/labor').then(m => m.LaborComponent)
    },
    {
      path: 'labor-type',
      loadComponent: () =>
        import('./pages/labor-type/labor-type').then(m => m.LaborTypeComponent)
    },
    {
      path: 'labor-type-rate',
      loadComponent: () =>
        import('./pages/labor-type-rate/labor-type-rate').then(m => m.LaborTypeRateComponent)
    },
  

      // ATTENDANCE
      {
        path: 'attendance',
        loadComponent: () => import('./pages/attendance/attendance').then(m => m.AttendanceComponent)
      },

      // PRODUCE
      {
        path: 'produce',
        loadComponent: () => import('./pages/produce/produce').then(m => m.ProduceComponent)
      },

      // DEALERS    
      { 
        path: 'dealer', 
        loadComponent: () => import('./pages/dealer/dealer').then(m => m.DealerComponent),
        title: 'Dealers' 
      },
      { 
        path: 'dealer-advance', 
        loadComponent: () => import('./pages/dealer-advance/dealer-advance').then(m => m.DealerAdvanceListComponent),
        title: 'Dealer Advance'
      },
      { 
        path: 'balance', 
        loadComponent: () => import('./pages/dealer-balance/dealer-balance').then(m => m.DealerBalanceComponent),
        title: 'Dealer Balance'
      },
    

      // FARMERS
      {
        path: 'farmer',
        loadComponent: () => import('./pages/farmer/farmer').then(m => m.FarmerComponent)
      },
      // CANE PURCHES
      {
        path: 'cane-purchase',
        loadComponent: () => import('./pages/cane-purchase/cane-purchase').then(m => m.CanePurchaseDashboardComponent)
      },
      //Cane Payments
      {
        path: 'cane-payment',
        loadComponent: () => import('./pages/cane-payment/cane-payment').then(m => m.CanePaymentComponent)
      },
      // Cane Advance
      {
        path: 'cane-advance',
        loadComponent: () => import('./pages/cane-advance/cane-advance').then(m => m.CaneAdvanceComponent)
      },
      // PAYMENTS
      {
        path: 'labor-payment',
        loadComponent: () => import('./pages/labor-payment/labor-payment').then(m => m.LaborPaymentComponent)
      },
      {
        path: 'advance-payment',
        loadComponent: () => import('./pages/advance-payment/advance-payment').then(m => m.AdvancePaymentComponent)
      },

      // JAGGERY
      {
        path: 'jaggery-sale',
        loadComponent: () => import('./pages/jaggery-sale/jaggery-sale').then(m => m.JaggerySaleComponent)
      },
      // JAGGERY REPORT
      {
        path: 'jaggery-report.component',
          loadComponent: () => import('./pages/jaggery-report.component/jaggery-report.component').then(m => m.JaggeryReportComponent),
       
      },
      {
        path: 'jaggery-share-dashboard',
        loadComponent: () =>
          import('./pages/jaggery-share-dashboard/jaggery-share-dashboard')
            .then(m => m.JaggeryShareDashboardComponent)
      },
      // Expense Type
      {
        path : 'expense-type',
        loadComponent: () => import('./pages/expense-type/expense-type').then(m => m.ExpenseTypeComponent)
      },
      // Expense
      
      // SPLITWISE
      {
        path: 'splitwise',
        loadComponent: () => import('./pages/splitwise/splitwise').then(m => m.SplitwiseComponent)
      }
    ]
  },

  // ❌ Unknown routes
  { path: '**', redirectTo: 'login' }
];
