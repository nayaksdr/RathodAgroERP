import { Component, OnInit } from '@angular/core';
import { JaggeryReportService } from '../../services/jaggery-report.service';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification';

interface ReportFilter {
  dealerId?: number;
  from: string;
  to: string;
}

@Component({
  selector: 'app-jaggery-report',
  standalone: true,
  imports: [MatIconModule, CommonModule,FormsModule],
  templateUrl: './jaggery-report.component.html',
  styleUrls: ['./jaggery-report.component.css']
})
export class JaggeryReportComponent implements OnInit {
searchText: string = '';

  dealers: any[] = [];
  sales: any[] = [];
  filteredSales: any[] = [];
  searchDealer: string = '';

  filter: ReportFilter = {
    dealerId: undefined,
    from: '',
    to: ''
  };

  reportData: any = { sales: [], summary: {} };
  isLoading = false;

  constructor(private reportService: JaggeryReportService, private notify: NotificationService) {}


loadDealers() {
  this.reportService.getDealers().subscribe({
    next: (res) => {
      this.dealers = res || [];
    },
    error: (err) => {
      this.notify.showError('Failed to load dealers');
      console.error('Dealer load failed', err);
    }
  });
}
getDealerName(dealerId: number | undefined): string {
  if (!dealerId) return 'All Dealers';
  const dealer = this.dealers.find(d => d.id === dealerId);
  return dealer ? dealer.name : '';
}
  ngOnInit() {
    this.fetchData();
    this.loadDealers(); 
  }

  fetchData() {
    this.isLoading = true;

    this.reportService
      .getReportData(this.filter.dealerId, this.filter.from, this.filter.to)
      .subscribe({
        next: (res) => {
          this.reportData = res;

          // ✅ Important Fix
          this.sales = res.sales || [];
          this.filteredSales = [...this.sales];

          this.isLoading = false;
          this.notify.close(); // Close loading notification
        },
        error: () => {
          this.isLoading = false;
          this.notify.showError('Failed to load report data');
        }
      });
  }

 applyFilter() {
  const search = this.searchText?.toLowerCase().trim();

  if (!search) {
    this.filteredSales = [...this.sales];
    return;
  }

  this.filteredSales = this.sales.filter(s =>
    (s.dealer?.name?.toLowerCase().includes(search)) ||
    s.totalAmount?.toString().includes(search) ||
    s.quantityInKg?.toString().includes(search) ||
    s.ratePerKg?.toString().includes(search)
  );
}


  export(type: 'excel' | 'pdf') {
    const url = this.reportService.getExportUrl(
      type,
      this.filter.dealerId,
      this.filter.from,
      this.filter.to
    );
    window.open(url, '_blank');
  }
}
