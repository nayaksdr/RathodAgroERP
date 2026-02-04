import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DashboardService } from '../../services/dashboard.service';
import { DashboardDailyVm } from '../../core/models/dashboard-daily.model';
import Chart from 'chart.js/auto';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../layout/sidebar-component/sidebar-component';
import { FooterComponent } from '../../shared/footer/footer';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, FooterComponent],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit, OnDestroy {

  data!: DashboardDailyVm;
  today: Date = new Date();
  kpis: any[] = [];
  charts: Chart[] = [];
  darkMode = localStorage.getItem('theme') === 'dark';

  // ================= Chart Metadata =================
  chartMeta: {
    id: string;
    title: string;
    icon: string;
    color: string;
    type: 'line' | 'bar';
    key: keyof DashboardDailyVm;
  }[] = [
    { id: 'attendanceChart', title: 'Attendance', icon: 'bi-person-check-fill', color: '#0d6efd', type: 'line', key: 'attendanceDaily' },
    { id: 'advanceChart', title: 'Advance', icon: 'bi-cash-stack', color: '#198754', type: 'bar', key: 'advancesDaily' },
    { id: 'expenseChart', title: 'Expenses', icon: 'bi-credit-card-2-front-fill', color: '#dc3545', type: 'bar', key: 'expensesDaily' },
    { id: 'caneChart', title: 'Cane Purchase Tons', icon: 'bi-bar-chart-fill', color: '#ffc107', type: 'line', key: 'canePurchaseTonsDaily' },
    { id: 'produceChart', title: 'Jaggery Production (Kg)', icon: 'bi-box-seam', color: '#0dcaf0', type: 'bar', key: 'produceQtyDaily' },
    { id: 'JagerySellChart', title: 'Jaggery Sales', icon: 'bi-cart-fill', color: '#fd7e14', type: 'bar', key: 'jagerySellDaily' }
  ];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void { this.loadDashboard(); }
  ngOnDestroy(): void { this.destroyCharts(); }

  toggleTheme(): void {
    this.darkMode = !this.darkMode;
    localStorage.setItem('theme', this.darkMode ? 'dark' : 'light');
  }

  // ================= Load Dashboard =================
  loadDashboard(): void {
    this.dashboardService.getDashboard(14).subscribe({
      next: res => {
        this.data = res;

        const targetLength = 14;
        ['attendanceDaily','advancesDaily','expensesDaily','canePurchaseTonsDaily','produceQtyDaily']
          .forEach(key => this.padDataArray(this.getArray(key as keyof DashboardDailyVm), targetLength));

        this.buildKpis();

        setTimeout(() => { this.initCharts(); this.initMemberPieCharts(); }, 0);
      },
      error: err => console.error('Dashboard Load Error:', err)
    });
  }

  private buildKpis(): void {
    this.kpis = [
      { title: 'Present', val: this.data.todayPresentCount, icon: 'bi-person-check-fill', color: '#0d6efd' },
      { title: 'Advance', val: this.data.todayAdvance, icon: 'bi-cash-stack', color: '#198754' },
      { title: 'Expense', val: this.data.todayExpense, icon: 'bi-credit-card-2-front-fill', color: '#dc3545' },
      { title: 'Cane Tons', val: this.data.todayCaneTons, icon: 'bi-bar-chart-fill', color: '#ffc107' },
      { title: 'Produce Qty', val: this.data.todayProduceQty, icon: 'bi-box-seam', color: '#0dcaf0' },
      { title: 'Sell', val: this.data.todayJaggerySellAmount, icon: 'bi-cart-fill', color: '#fd7e14' }
    ];
  }

  private padDataArray(array: any[], targetLength: number): void {
    while (array.length < targetLength) array.unshift({ value: 0 });
  }

  private getArray(key: keyof DashboardDailyVm): any[] {
    return (this.data[key] ?? []) as any[];
  }

  private destroyCharts(): void {
    this.charts.forEach(chart => chart.destroy());
    this.charts = [];
  }

  // ================= Init Charts =================
  private initCharts(): void {
    this.destroyCharts();
    if (!this.data) return;

    this.chartMeta.forEach(meta => {
      if (meta.id === 'JagerySellChart') {
        const sellLabels = this.data.jagerySellDaily.map(x => new Date(x.date).toLocaleDateString('en-GB'));
        this.charts.push(new Chart(meta.id, {
          type: 'bar',
          data: {
            labels: sellLabels,
            datasets: [
              { label: 'Quantity', data: this.data.jagerySellDaily.map(x => x.qty), backgroundColor: '#0dcaf0', yAxisID: 'y1' },
              { label: 'Amount', data: this.data.jagerySellDaily.map(x => x.amount), backgroundColor: '#ffc107', yAxisID: 'y2' }
            ]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { title: { display: true, text: meta.title, font: { size: 16, weight: 'bold' } } },
            scales: { y1: { beginAtZero: true, position: 'left' }, y2: { beginAtZero: true, position: 'right', grid: { drawOnChartArea: false } } }
          }
        }));
      } else {
        const dataset = this.getArray(meta.key).map(x => x.value);
        this.charts.push(new Chart(meta.id, {
          type: meta.type,
          data: {
            labels: this.data.labels,
            datasets: [{
              label: meta.title,
              data: dataset,
              borderColor: meta.type === 'line' ? meta.color : undefined,
              backgroundColor: meta.type === 'bar' ? meta.color : `rgba(${parseInt(meta.color.slice(1,3),16)},${parseInt(meta.color.slice(3,5),16)},${parseInt(meta.color.slice(5,7),16)},0.3)`,
              fill: meta.type === 'line',
              tension: meta.type === 'line' ? 0.3 : undefined
            }]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { title: { display: true, text: meta.title, font: { size: 16, weight: 'bold' } } }
          }
        }));
      }
    });
  }

  // ================= Member Pie Charts =================
  private initMemberPieCharts(): void {
    if (!this.data?.memberSummary) return;

    this.data.memberSummary.forEach(m => {
      this.charts.push(new Chart(`pieChart_${m.memberId}`, {
        type: 'pie',
        data: {
          labels: ['Paid', 'Earned'],
          datasets: [{ data: [m.splitwisePaid, m.jaggeryEarned], backgroundColor: ['#0d6efd', '#ffc107'] }]
        },
        options: {
          plugins: { title: { display: true, text: `${m.name} Share`, font: { size: 16, weight: 'bold' } } },
          responsive: true,
          maintainAspectRatio: false
        }
      }));
    });
  }
}
